using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Moq;
using SaveFiles;
using TextDecoder.Parser;
using UnityEngine;
using UnityEngine.TestTools;

public class ActionDecoderTests
{
    // This block generates lists of method names, based on their characteristics
    private static IEnumerable<MethodInfo> AvailableActionMethods => typeof(ActionDecoder).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(method => new Regex("^[A-Z_]+$").IsMatch(method.Name)).ToArray();
    private static IEnumerable<string> AllAvailableActionsWithoutOptionalParameters => AvailableActionMethods.Where(method => method.GetParameters().All(parameter => !parameter.IsOptional)).Select(methodInfo => methodInfo.Name);
    private static IEnumerable<object[]> AvailableActionsWithOptionalParametersAndRange => AvailableActionMethods
        .Where(method => method.GetParameters().Any(parameter => parameter.IsOptional))
        .Select(method => { 
            return Enumerable.Range(method.GetParameters().Count(parameter => !parameter.IsOptional), method.GetParameters().Length).Select(parameterCount => new object[]{method.Name, parameterCount});
        })
        .SelectMany(list => list);
    private static IEnumerable<string> AvailableActionsWithNoOptionalParameters => AvailableActionMethods.Where(method => method.GetParameters().Any() && method.GetParameters().All(parameter => !parameter.IsOptional)).Select(methodInfo => methodInfo.Name);
    private static IEnumerable<string> AvailableActionsWithInvalidParameters => AvailableActionMethods.Where(method => method.GetParameters().Any(parameter => InvalidData.Keys.Contains(parameter.ParameterType))).Select(methodInfo => methodInfo.Name);

    // These are example values for parameters of "Ink"-script action lines for the ActionDecoder
    private static readonly Dictionary<Type, object> ValidData = new Dictionary<Type, object> {
        {typeof(string), "ValidString"},
        {typeof(AssetName), "ValidString"},
        {typeof(FullscreenAnimationAssetName), "ValidString"},
        {typeof(SceneAssetName), "ValidString"},
        {typeof(SfxAssetName), "ValidString"},
        {typeof(SongAssetName), "ValidString"},
        {typeof(CourtRecordItemName), "ValidString"},
        {typeof(EvidenceAssetName), "ValidString"},
        {typeof(ActorAssetName), "ValidString"},
        {typeof(ActorPoseAssetName), "ValidString"},
        {typeof(NarrativeScriptAssetName), "ValidString" },
        {typeof(GameOverScriptAssetName), "ValidString"},
        {typeof(FailureScriptAssetName), "ValidString"},
        {typeof(bool), "true"},
        {typeof(int), "1"},
        {typeof(float), "1.0"},
        {typeof(ItemDisplayPosition), nameof(ItemDisplayPosition.Left)},
        {typeof(GameMode), nameof(GameMode.CrossExamination)},
        {typeof(SaveData.Progression.Chapters), nameof(SaveData.Progression.Chapters.Chapter1)}
    };
    private static readonly Dictionary<Type, object> InvalidData = new Dictionary<Type, object> {
        {typeof(bool), "NotABool"},
        {typeof(int), "1.0"},
        {typeof(float), "NotAFloat"},
        {typeof(ItemDisplayPosition), "Invalid"},
        {typeof(GameMode), "Invalid"},
        {typeof(SaveData.Progression.Chapters), "Invalid"}
    };

    private class RawActionDecoder : ActionDecoderBase
    {
        protected override void ADD_EVIDENCE(EvidenceAssetName evidenceName)
        {
            throw new NotImplementedException();
        }

        protected override void ADD_RECORD(ActorAssetName actorName)
        {
            throw new NotImplementedException();
        }

        protected override void PLAY_SFX(SfxAssetName sfx)
        {
            throw new NotImplementedException();
        }

        protected override void PLAY_SONG(SongAssetName songName, float transitionTime = 0)
        {
            throw new NotImplementedException();
        }

        protected override void SCENE(SceneAssetName sceneName)
        {
            throw new NotImplementedException();
        }

        protected override void SHOW_ITEM(CourtRecordItemName itemName, ItemDisplayPosition itemPos)
        {
            throw new NotImplementedException();
        }

        protected override void ACTOR(ActorAssetName actorName)
        {
            throw new NotImplementedException();
        }

        protected override void SPEAK(ActorAssetName actorName)
        {
            throw new NotImplementedException();
        }

        protected override void SPEAK_UNKNOWN(ActorAssetName actorName)
        {
            throw new NotImplementedException();
        }

        protected override void THINK(ActorAssetName actorName)
        {
            throw new NotImplementedException();
        }

        protected override void SET_ACTOR_POSITION(string slotName, ActorAssetName actorName)
        {
            throw new NotImplementedException();
        }
    }

    #region Utilities
    /// <summary>
    /// Helper method to create a fully mocked ActionDecoder
    /// </summary>
    /// <returns>A fully mocked ActionDecoder</returns>
    private static ActionDecoder CreateMockedActionDecoder()
    {
        var narrativeGameStateMock = new Mock<INarrativeGameState>();
        narrativeGameStateMock.SetupSet(mock => mock.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.GameMode = It.IsAny<GameMode>());
        narrativeGameStateMock.Setup(mock => mock.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript.ObjectStorage.GetObject<EvidenceData>(""));
        narrativeGameStateMock.Setup(mock => mock.SceneController.ShakeScreen(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<bool>()));
        narrativeGameStateMock.Setup(mock => mock.SceneController.SetScene(It.IsAny<string>()));
        narrativeGameStateMock.Setup(mock => mock.ActorController.SetPose(It.IsAny<string>(), It.IsAny<string>()));
        narrativeGameStateMock.SetupSet(mock => mock.AppearingDialogueController.CharacterDelay = It.IsAny<float>());
        narrativeGameStateMock.Setup(mock => mock.EvidenceController.AddEvidence(It.IsAny<EvidenceData>()));
        narrativeGameStateMock.Setup(mock => mock.ObjectStorage.GetObject<EvidenceData>(It.IsAny<string>()));
        narrativeGameStateMock.Setup(mock => mock.AudioController.PlaySfx(It.IsAny<AudioClip>()));
        narrativeGameStateMock.Setup(mock => mock.PenaltyManager.Decrement());
        narrativeGameStateMock.Setup(mock => mock.NarrativeScriptStorage.AddFailureScript(It.IsAny<string>()));

        return new ActionDecoder
        {
            NarrativeGameState = narrativeGameStateMock.Object
        };
    }

    /// <summary>
    /// Helper method to generate valid parameters for a given method as strings identical to how they're used inside .ink files
    /// </summary>
    /// <returns>Enumerable of string representations of valid data for all optional and required arguments for a method</returns>
    private static IEnumerable<string> GenerateParametersForMethod(MethodInfo method)
    {
        Assert.NotNull(method, $"Couldn't find method with name '{method.Name}' on object of type '{nameof(ActionDecoder)}'");
        return method.GetParameters().Select(parameterInfo => {
            if (!ValidData.ContainsKey(parameterInfo.ParameterType))
            {
                throw new NotImplementedException($"In order for this test to run, you need to specify a valid example for '{parameterInfo.ParameterType}' inside '{nameof(ActionDecoderTests)}.{nameof(ValidData)}'");
            }

            return ValidData[parameterInfo.ParameterType];
        }).Select(validParameter => validParameter.ToString());
    }
    #endregion

    [Test]
    public void ThrowsIfMethodIsNotImplemented()
    {
        const string MISSING_METHOD_NAME = "MISSING_METHOD";
        var decoder = new RawActionDecoder();
        
        var lineToParse = $"&{MISSING_METHOD_NAME}";
        Debug.Log("Attempting to parse:\n" + lineToParse);
        var expectedException = Assert.Throws<MethodNotFoundScriptParsingException>(() => {
            decoder.InvokeMatchingMethod(lineToParse);
        });
        StringAssert.Contains(MISSING_METHOD_NAME, expectedException.Message);
    }

    [Test]
    [TestCaseSource(nameof(AllAvailableActionsWithoutOptionalParameters))]
    public void RunValidCommandWithoutOptionalParameters(string methodName)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        var validParametersForMethod = GenerateParametersForMethod(method).ToList();

        var lineToParse = $"&{methodName}{(validParametersForMethod.Any()?":":"")}{string.Join(",", validParametersForMethod)}";
        Debug.Log("Attempting to parse:\n" + lineToParse);
        Assert.DoesNotThrow(() => {
            decoder.InvokeMatchingMethod(lineToParse);
        });
    }

    [Test]
    [TestCaseSource(nameof(AvailableActionsWithOptionalParametersAndRange))]
    public void RunCommandWithOptionalParameters(string methodName, int parameterCount)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        var validParametersForMethod = GenerateParametersForMethod(method).Take(parameterCount).ToList();

        var lineToParse = $"&{methodName}{(validParametersForMethod.Any() ? ":" : "")}{string.Join(",", validParametersForMethod)}";
        Debug.Log("Attempting to parse:\n" + lineToParse);
        Assert.DoesNotThrow(() => {
            decoder.InvokeMatchingMethod(lineToParse);
        });
    }

    [Test]
    [TestCaseSource(nameof(AllAvailableActionsWithoutOptionalParameters))]
    public void RunCommandWithTooManyArguments(string methodName)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        var validParametersPlusSuperfluousArgumentForMethod = GenerateParametersForMethod(method).Append("NewElement").ToList();

        var lineToParse = $"&{methodName}:{string.Join(",", validParametersPlusSuperfluousArgumentForMethod)}";
        Debug.Log("Attempting to parse:\n"+lineToParse);
        Assert.Throws<ScriptParsingException>(() => {
            decoder.InvokeMatchingMethod(lineToParse);
        }, $"'{methodName}' requires exactly {validParametersPlusSuperfluousArgumentForMethod.Count-1} parameters (has {validParametersPlusSuperfluousArgumentForMethod.Count} instead)");
    }

    [Test]
    [TestCaseSource(nameof(AvailableActionsWithNoOptionalParameters))]
    public void RunCommandWithTooFewArguments(string methodName)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
        var generatedParameters = GenerateParametersForMethod(method).ToList();
        var lineToParse = $"&{methodName}{(generatedParameters.Count > 1 ? ":" : "")}{string.Join(",", string.Join(",", generatedParameters.Skip(1)))}";
        Debug.Log("Attempting to parse:\n"+lineToParse);
        Assert.Throws<ScriptParsingException>(() => {
            decoder.InvokeMatchingMethod(lineToParse);
        }, $"'{methodName}' requires exactly {generatedParameters.Count + 1} parameters (has {generatedParameters.Count} instead)");
    }

    [Test]
    [TestCaseSource(nameof(AvailableActionsWithInvalidParameters))]
    public void RunCommandWithIncorrectParameterTypes(string methodName)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
        var generatedParameters = method.GetParameters().Select(parameterInfo => {
            if (!InvalidData.ContainsKey(parameterInfo.ParameterType) && !ValidData.ContainsKey(parameterInfo.ParameterType))
            {
                throw new NotImplementedException($"In order for this test to run, you need to specify either an invalid ink-script example for '{parameterInfo.ParameterType}' inside '{nameof(ActionDecoderTests)}.{nameof(InvalidData)}' or a valid ink-script example inside '{nameof(ActionDecoderTests)}.{nameof(ValidData)}'");
            }
            return (InvalidData.ContainsKey(parameterInfo.ParameterType) ? InvalidData : ValidData)[parameterInfo.ParameterType];
        }).Select(validParameter => validParameter.ToString()).ToList();
        var lineToParse = $"&{methodName}{(generatedParameters.Any() ? ":" : "")}{string.Join(",", generatedParameters)}";
        Debug.Log("Attempting to parse:\n"+lineToParse);
        var thrownException = Assert.Throws<ScriptParsingException>(() => {
            decoder.InvokeMatchingMethod(lineToParse);
        });
        StringAssert.Contains("is incorrect as parameter", thrownException.Message);
    }

    [Test]
    public void VerifyOnlyOneColonAllowedPerActionLine()
    {
        var decoder = CreateMockedActionDecoder();

        const string LINE_TO_PARSE = "&METHODNAME:PARAMETER:INVALID";
        Debug.Log("Attempting to parse:\n" + LINE_TO_PARSE);
        Assert.Throws<ScriptParsingException>(() => {
            decoder.InvokeMatchingMethod(LINE_TO_PARSE);
        }, $"More than one ':' detected in line '{LINE_TO_PARSE}");
    }

    [Test]
    public void VerifyActionDecoderTrimsExcessWhitespace()
    {
        var decoder = CreateMockedActionDecoder();
        var narrativeGameStateMock = new Mock<INarrativeGameState>();
        narrativeGameStateMock.Setup(mock => mock.SceneController.SetScene(new AssetName("NewScene")));
        decoder.NarrativeGameState = narrativeGameStateMock.Object;

        const string LINE_TO_PARSE = " &SCENE:NewScene \n\n\n";
        const string LOG_MESSAGE = "Attempting to parse:\n" + LINE_TO_PARSE;
        Debug.Log(LOG_MESSAGE);
        Assert.DoesNotThrow(() => { decoder.InvokeMatchingMethod(LINE_TO_PARSE); });

        LogAssert.Expect(LogType.Log, LOG_MESSAGE);
        LogAssert.NoUnexpectedReceived();

        narrativeGameStateMock.Verify(controller => controller.SceneController.SetScene(new AssetName("NewScene")));
    }

}