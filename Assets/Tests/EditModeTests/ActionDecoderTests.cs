using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TextDecoder.Parser;
using UnityEngine;

namespace Tests.EditModeTests
{
    public class ActionDecoderTests
    {
        private static IEnumerable<string> AvailableActions => typeof(ActionDecoder).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Select(method=>method.Name).Where(methodName => new Regex("^[A-Z_]+$").IsMatch(methodName)).ToArray();

        private static readonly Dictionary<Type, object> ValidData = new Dictionary<Type, object>{
            {typeof(string), "ValidString"},
            {typeof(bool), "true"},
            {typeof(int), "1"},
            {typeof(float), "1.0"},
            {typeof(ItemDisplayPosition), "Left"},
        };
        private static readonly Dictionary<Type, object> InvalidData = new Dictionary<Type, object>{
            {typeof(string), "IsAlwaysValid"}, // see: TextDecoder.Parser.StringParser.Parser()
            {typeof(bool), "NotABool"},
            {typeof(int), "1.0"},
            {typeof(float), "NotAFloat"},
            {typeof(ItemDisplayPosition), "Invalid"}
        };

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
        [TestCaseSource(nameof(AvailableActions))]
        public void RunValidCommand(string methodName)
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
        [TestCaseSource(nameof(AvailableActions))]
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
        [TestCaseSource(nameof(AvailableActions))]
        public void RunCommandWithTooFewArguments(string methodName)
        {
            var decoder = CreateMockedActionDecoder();

            var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
            var generatedParameters = method.GetParameters().Select(parameterInfo => ValidData[parameterInfo.ParameterType]).Select(validParameter => validParameter.ToString()).ToList();
            if (generatedParameters.Count == 0)
            {
                Debug.LogWarning($"This method doesn't require any parameters, therefore it is impossible to call it with too few arguments");
                return;
            }
            var lineToParse = $"&{methodName}{(generatedParameters.Any() ? ":" : "")}{string.Join(",", string.Join(",", generatedParameters.Skip(1)))}";
            Debug.Log("Attempting to parse:\n"+lineToParse);
            Assert.Throws<ScriptParsingException>(() => {
                decoder.OnNewActionLine(lineToParse);
            }, $"'{methodName}' requires exactly {generatedParameters.Count + 1} parameters (has {generatedParameters.Count} instead)");
        }

        [Test]
        [TestCaseSource(nameof(AvailableActions))]
        public void RunCommandWithIncorrectParameterTypes(string methodName)
        {
            var decoder = CreateMockedActionDecoder();

            var method = decoder.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method, $"Couldn't find method with name '{methodName}' on object of type '{nameof(ActionDecoder)}'");
            var distinctTypes = method.GetParameters().Select(parameter => parameter.ParameterType).Distinct().ToList();
            if (distinctTypes.All(type => type == typeof(string)))
            {
                Debug.LogWarning($"All parameters are of type '{typeof(string)}', therefore we cannot supply invalid values to this method (strings are always valid)"); // see TextDecoder.Parser.StringParser.Parse()
                return;
            }

            var generatedParameters = method.GetParameters().Select(parameterInfo => InvalidData[parameterInfo.ParameterType]).Select(validParameter => validParameter.ToString()).ToList();
            var lineToParse = $"&{methodName}{(generatedParameters.Any() ? ":" : "")}{string.Join(",", generatedParameters)}";
            Debug.Log("Attempting to parse:\n"+lineToParse);
            var thrownException = Assert.Throws<ScriptParsingException>(() => {
                decoder.OnNewActionLine(lineToParse);
            });
            StringAssert.Contains("is incorrect as parameter", thrownException.Message);
        }

        [Test]
        public void OnlyOneColonAllowedPerAction()
        {
            var decoder = CreateMockedActionDecoder();

            var lineToParse = $"&METHODNAME:PARAMETER:INVALID";
            Debug.Log("Attempting to parse:\n" + lineToParse);
            Assert.Throws<ScriptParsingException>(() => {
                decoder.OnNewActionLine(lineToParse);
            }, $"More than one ':' detected in line '{lineToParse}");
        }

    }
}