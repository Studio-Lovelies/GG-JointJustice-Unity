using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
    private static IEnumerable<string> AvailableActionsWithNonStringParameters => AvailableActionMethods.Where(method => method.GetParameters().Any(parameter => parameter.ParameterType != typeof(string))).Select(methodInfo => methodInfo.Name);

    // These are example values for parameters of "Ink"-script action lines for the ActionDecoder
    private static readonly Dictionary<Type, object> ValidData = new Dictionary<Type, object> {
        {typeof(string), "ValidString"},
        {typeof(bool), "true"},
        {typeof(int), "1"},
        {typeof(float), "1.0"},
        {typeof(ItemDisplayPosition), "Left"},
    };
    private static readonly Dictionary<Type, object> InvalidData = new Dictionary<Type, object> {
        {typeof(string), "IsAlwaysValid"}, // see: TextDecoder.Parser.StringParser.Parser()
        {typeof(bool), "NotABool"},
        {typeof(int), "1.0"},
        {typeof(float), "NotAFloat"},
        {typeof(ItemDisplayPosition), "Invalid"}
    };
    
    /// <summary>
    /// Helper method to create a fully mocked ActionDecoder
    /// </summary>
    /// <returns>A fully mocked ActionDecoder</returns>
    private static ActionDecoder CreateMockedActionDecoder()
    {
        return new ActionDecoder()
        {
            ActorController = new Moq.Mock<IActorController>().Object,
            AppearingDialogueController = new Moq.Mock<IAppearingDialogueController>().Object,
            AudioController = new Moq.Mock<IAudioController>().Object,
            EvidenceController = new Moq.Mock<IEvidenceController>().Object,
            SceneController = new Moq.Mock<ISceneController>().Object,
        };
    }

    [Test]
    [TestCaseSource(nameof(AllAvailableActionsWithoutOptionalParameters))]
    public void RunValidCommandWithoutOptionalParameters(string methodName)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
        var validParameters = method.GetParameters().Select(parameterInfo => ValidData[parameterInfo.ParameterType]).Select(validParameter => validParameter.ToString()).ToList();
        var lineToParse = $"&{methodName}{(validParameters.Any()?":":"")}{string.Join(",", validParameters)}";
        Debug.Log("Attempting to parse:\n"+lineToParse);
        Assert.DoesNotThrow(() => {
            decoder.OnNewActionLine(lineToParse);
        });
    }

    [Test]
    [TestCaseSource(nameof(AvailableActionsWithOptionalParametersAndRange))]
    public void RunCommandWithOptionalParameters(string methodName, int parameterCount)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
        var validParameters = method.GetParameters().Select(parameterInfo => ValidData[parameterInfo.ParameterType]).Select(validParameter => validParameter.ToString()).Take(parameterCount).ToList();
        var lineToParse = $"&{methodName}{(validParameters.Any() ? ":" : "")}{string.Join(",", validParameters)}";
        Debug.Log("Attempting to parse:\n" + lineToParse);
        Assert.DoesNotThrow(() => {
            decoder.OnNewActionLine(lineToParse);
        });
    }

    [Test]
    [TestCaseSource(nameof(AllAvailableActionsWithoutOptionalParameters))]
    public void RunCommandWithTooManyArguments(string methodName)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
        var generatedParameters = method.GetParameters().Select(parameterInfo => ValidData[parameterInfo.ParameterType]).Select(validParameter => validParameter.ToString()).Append("NewElement").ToList();
        var lineToParse = $"&{methodName}{(generatedParameters.Any()?":":"")}{string.Join(",", generatedParameters)}";
        Debug.Log("Attempting to parse:\n"+lineToParse);
        Assert.Throws<ScriptParsingException>(() => {
            decoder.OnNewActionLine(lineToParse);
        }, $"'{methodName}' requires exactly {generatedParameters.Count-1} parameters (has {generatedParameters.Count} instead)");
    }

    [Test]
    [TestCaseSource(nameof(AvailableActionsWithNoOptionalParameters))]
    public void RunCommandWithTooFewArguments(string methodName)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
        var generatedParameters = method.GetParameters().Select(parameterInfo => ValidData[parameterInfo.ParameterType]).Select(validParameter => validParameter.ToString()).ToList();
        var lineToParse = $"&{methodName}{(generatedParameters.Any() ? ":" : "")}{string.Join(",", string.Join(",", generatedParameters.Skip(1)))}";
        Debug.Log("Attempting to parse:\n"+lineToParse);
        Assert.Throws<ScriptParsingException>(() => {
            decoder.OnNewActionLine(lineToParse);
        }, $"'{methodName}' requires exactly {generatedParameters.Count + 1} parameters (has {generatedParameters.Count} instead)");
    }

    [Test]
    [TestCaseSource(nameof(AvailableActionsWithNonStringParameters))]
    public void RunCommandWithIncorrectParameterTypes(string methodName)
    {
        var decoder = CreateMockedActionDecoder();

        var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
        var generatedParameters = method.GetParameters().Select(parameterInfo => InvalidData[parameterInfo.ParameterType]).Select(validParameter => validParameter.ToString()).ToList();
        var lineToParse = $"&{methodName}{(generatedParameters.Any() ? ":" : "")}{string.Join(",", generatedParameters)}";
        Debug.Log("Attempting to parse:\n"+lineToParse);
        var thrownException = Assert.Throws<ScriptParsingException>(() => {
            decoder.OnNewActionLine(lineToParse);
        });
        StringAssert.Contains("is incorrect as parameter", thrownException.Message);
    }

    [Test]
    public void VerifyOnlyOneColonAllowedPerActionLine()
    {
        var decoder = CreateMockedActionDecoder();

        var lineToParse = $"&METHODNAME:PARAMETER:INVALID";
        Debug.Log("Attempting to parse:\n" + lineToParse);
        Assert.Throws<ScriptParsingException>(() => {
            decoder.OnNewActionLine(lineToParse);
        }, $"More than one ':' detected in line '{lineToParse}");
    }

    [Test]
    public void VerifyActionDecoderTrimsExcessWhitespace()
    {
        var decoder = CreateMockedActionDecoder();
        var sceneControllerMock = new Moq.Mock<ISceneController>();
        sceneControllerMock.Setup(controller => controller.SetScene("NEW_SCENE"));
        decoder.SceneController = sceneControllerMock.Object;

        var lineToParse = " &SCENE:NEW_SCENE \n\n\n";
        var logMessage = "Attempting to parse:\n" + lineToParse;
        Debug.Log(logMessage);
        Assert.DoesNotThrow(() => { decoder.OnNewActionLine(lineToParse); });

        LogAssert.Expect(LogType.Log, logMessage);
        LogAssert.NoUnexpectedReceived();

        sceneControllerMock.Verify(controller => controller.SetScene("NEW_SCENE"));
    }

}