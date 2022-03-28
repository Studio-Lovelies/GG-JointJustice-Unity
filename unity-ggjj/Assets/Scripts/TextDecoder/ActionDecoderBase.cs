using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class ActionDecoderBase : IActionDecoder
{
    public const char ACTION_TOKEN = '&';

    /// <summary>
    ///     Parse action lines inside .ink files
    /// </summary>
    /// <param name="actionLine">Line of a .ink file that starts with &amp; (and thereby is not a "spoken dialogue" line)</param>
    /// <remarks>
    ///     Writers are able to call methods inside .ink files. This is done by using the following syntax:
    ///     <code>
    ///     &amp;{methodName}:{parameter1},{parameter2},...
    ///     </code>
    ///     This method is responsible for...
    ///         1. Getting method details using the GenerateInvocationDetails method
    ///         2. Invoking the found method with its parsed method parameters
    /// </remarks>
    public void InvokeMatchingMethod(string actionLine)
    {
        var method = GenerateInvocationDetails(actionLine, GetType());
        method.MethodInfo.Invoke(this, method.ParsedMethodParameters.ToArray());
    }

    /// <summary>
    /// This method is responsible for:
    ///     1. Finding a method inside this class, matching `methodName`
    ///     2. Verifying the amount of parameters matches the amount of parameters needed in the method
    ///     3. Attempting to parse each parameter into the correct type using <see cref="Parser&lt;T&gt;"/> of the type
    /// </summary>
    /// <param name="actionLine">The action line to parse</param>
    /// <param name="decoderType">The type of decoder to get methods from</param>
    /// <returns>An InvocationDetails with details of the found method and its parameters</returns>
    public static InvocationDetails GenerateInvocationDetails(string actionLine, Type decoderType)
    {
        actionLine = actionLine.Trim();
        const char actionSideSeparator = ':';
        const char actionParameterSeparator = ',';

        var actionNameAndParameters = actionLine.Substring(1, actionLine.Length - 1).Trim().Split(actionSideSeparator);

        if (actionNameAndParameters.Length > 2)
        {
            throw new TextDecoder.Parser.ScriptParsingException($"More than one '{actionSideSeparator}' detected in line '{actionLine}'");
        }

        var action = actionNameAndParameters[0];
        var parameters = (actionNameAndParameters.Length == 2) ? actionNameAndParameters[1].Split(actionParameterSeparator) : Array.Empty<string>();

        // Find method with exact same name as action inside script
        var methodInfo = decoderType.GetMethod(action, BindingFlags.Instance | BindingFlags.NonPublic);
        if (methodInfo == null)
        {
            throw new TextDecoder.Parser.MethodNotFoundScriptParsingException("ActionDecoder", action);
        }

        var methodParameters = methodInfo.GetParameters();
        var optionalParameters = methodParameters.Count(parameter => parameter.IsOptional);
        if (parameters.Length < (methodParameters.Length - optionalParameters) || parameters.Length > (methodParameters.Length))
        {
            throw new TextDecoder.Parser.ScriptParsingException($"'{action}' requires {(optionalParameters == 0 ? "exactly" : "between")} {(optionalParameters == 0 ? methodParameters.Length.ToString() : $"{methodParameters.Length-optionalParameters} and {methodParameters.Length}")} parameters (has {parameters.Length} instead)");
        }

        var parsedMethodParameters = new List<object>();
        // For each supplied parameter of that action...
        for (var index = 0; index < parameters.Length; index++)
        {
            if (parameters.Length <= index && methodParameters[index].IsOptional)
            {
                parsedMethodParameters.Add(methodParameters[index].DefaultValue);
            }

            // Determine it's type
            var methodParameter = methodParameters[index];

            // Edge-case for enums
            if (methodParameter.ParameterType.BaseType == typeof(Enum))
            {
                try
                {
                    parsedMethodParameters.Add(Enum.Parse(methodParameter.ParameterType, parameters[index]));
                    continue;
                }
                catch (ArgumentException e)
                {
                    var pattern = new Regex(@"Requested value '(.*)' was not found\.");
                    var match = pattern.Match(e.Message);
                    if (match.Success)
                    {
                        throw new TextDecoder.Parser.ScriptParsingException($"'{parameters[index]}' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': Cannot convert '{match.Groups[1].Captures[0]}' into an {methodParameter.ParameterType} (valid values include: '{string.Join(", ", Enum.GetValues(methodParameter.ParameterType).Cast<object>().Select(a=>a.ToString()))}')");
                    }

                    if (e.Message == "Must specify valid information for parsing in the string.")
                    {
                        throw new TextDecoder.Parser.ScriptParsingException($"'' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': Cannot convert '' into an {methodParameter.ParameterType} (valid values include: '{string.Join(", ", Enum.GetValues(methodParameter.ParameterType).Cast<object>().Select(a => a.ToString()))}')");
                    }
                    throw;
                }
            }

            // Construct a parser for it
            var parser = decoderType.Assembly.GetTypes().FirstOrDefault(type => type.BaseType is { IsGenericType: true } && type.BaseType.GenericTypeArguments[0] == methodParameter.ParameterType);
            if (parser == null)
            {
                throw new TextDecoder.Parser.MissingParserException($"The TextDecoder.Parser namespace contains no Parser for type {methodParameter.ParameterType}");
            }

            var parserConstructor = parser.GetConstructor(Type.EmptyTypes);
            if (parserConstructor == null)
            {
                throw new ArgumentException($"TextDecoder.Parser for type {methodParameter.ParameterType} has no constructor without parameters");
            }

            // Find the 'Parse' method on that parser
            var parseMethod = parser.GetMethod("Parse");
            if (parseMethod == null)
            {
                throw new MissingMethodException($"TextDecoder.Parser for type {methodParameter.ParameterType} has no 'Parse' method");
            }

            // Create a parser and call the 'Parse' method
            var parserInstance = parserConstructor.Invoke(Array.Empty<object>());
            object[] parseMethodParameters = { parameters[index], null };

            // If we received an error attempting to parse a parameter to the type, expose it to the user
            var humanReadableParseError = parseMethod.Invoke(parserInstance, parseMethodParameters);
            if (humanReadableParseError != null)
            {
                throw new TextDecoder.Parser.ScriptParsingException($"'{parameters[index]}' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': {humanReadableParseError}");
            }

            parsedMethodParameters.Add(parseMethodParameters[1]);
        }

        // If the method supports optional parameters, fill the remaining parameters based on the default value of the method
        for (var suppliedParameterCount = parameters.Length; suppliedParameterCount < methodParameters.Length; suppliedParameterCount++)
        {
            parsedMethodParameters.Add(methodParameters[suppliedParameterCount].DefaultValue);
        }

        return new InvocationDetails
        {
            MethodInfo = methodInfo,
            ParsedMethodParameters = parsedMethodParameters
        };
    }

    /// <summary>
    /// Determines if a line of dialogue is an action.
    /// </summary>
    /// <param name="line">The line to check.</param>
    /// <returns>If the line is an action (true) or not (false)</returns>
    public bool IsAction(string line)
    {
        return line != string.Empty && line[0] == ACTION_TOKEN;
    }

    protected abstract void ADD_EVIDENCE(EvidenceAssetName evidenceName);
    protected abstract void ADD_RECORD(ActorAssetName actorName);
    protected abstract void PLAY_SFX(SfxAssetName sfx);
    protected abstract void PLAY_SONG(SongAssetName songName, float optional_transitionTime = 0);
    protected abstract void SCENE(SceneAssetName sceneName);
    protected abstract void SHOW_ITEM(EvidenceAssetName evidenceName, ItemDisplayPosition itemPos);
    protected abstract void ACTOR(ActorAssetName actorName);
    protected abstract void SPEAK(ActorAssetName actorName);
    protected abstract void SPEAK_UNKNOWN(ActorAssetName actorName);
    protected abstract void THINK(ActorAssetName actorName);
    protected abstract void SET_ACTOR_POSITION(int oneBasedSlotIndex, ActorAssetName actorName);
}
