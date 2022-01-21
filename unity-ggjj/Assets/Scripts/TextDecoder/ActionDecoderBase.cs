using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class ActionDecoderBase : IActionDecoder
{
    /// <summary>
    ///     Parse action lines inside from inside .ink files
    /// </summary>
    /// <param name="actionLine">Line of a .ink file that starts with &amp; (and thereby is not a "spoken dialogue" line)</param>
    /// <remarks>
    ///     Writers are able to call methods inside .ink files. This is done by using the following syntax:
    ///     <code>
    ///     &amp;{methodName}:{parameter1},{parameter2},...
    ///     </code>
    ///     This method is responsible for:
    ///         1. Finding a method inside this class, matching `methodName`
    ///         2. Verifying the amount of parameters matches the amount of parameters needed in the method
    ///         3. Attempting to parse each parameter into the correct type using <see cref="Parser&lt;T&gt;"/> of the type
    ///         4. Invoking the method with the parsed parameters
    /// </remarks>
    public void OnNewActionLine(string actionLine)
    {
        actionLine = actionLine.Trim();
        const char actionSideSeparator = ':';
        const char actionParameterSeparator = ',';

        string[] actionNameAndParameters = actionLine.Substring(1, actionLine.Length - 1).Trim().Split(actionSideSeparator);

        if (actionNameAndParameters.Length > 2)
        {
            throw new TextDecoder.Parser.ScriptParsingException($"More than one '{actionSideSeparator}' detected in line '{actionLine}'");
        }

        string action = actionNameAndParameters[0];
        string[] parameters = (actionNameAndParameters.Length == 2) ? actionNameAndParameters[1].Split(actionParameterSeparator) : Array.Empty<string>();

        // Find method with exact same name as action inside script
        MethodInfo method = GetType().GetMethod(action, BindingFlags.Instance | BindingFlags.NonPublic);
        if (method == null)
        {
            throw new TextDecoder.Parser.MethodNotFoundScriptParsingException(GetType().FullName, action);
        }

        ParameterInfo[] methodParameters = method.GetParameters();
        var optionalParameters = methodParameters.Count(parameter => parameter.IsOptional);
        if (parameters.Length < (methodParameters.Length - optionalParameters) || parameters.Length > (methodParameters.Length))
        {
            throw new TextDecoder.Parser.ScriptParsingException($"'{action}' requires {(optionalParameters == 0 ? "exactly" : "between")} {(optionalParameters == 0 ? methodParameters.Length.ToString() : $"{methodParameters.Length-optionalParameters} and {methodParameters.Length}")} parameters (has {parameters.Length} instead)");
        }

        List<object> parsedMethodParameters = new List<object>();
        // For each supplied parameter of that action...
        for (int index = 0; index < parameters.Length; index++)
        {
            if (parameters.Length <= index && methodParameters[index].IsOptional)
            {
                parsedMethodParameters.Add(methodParameters[index].DefaultValue);
            }

            // Determine it's type
            ParameterInfo methodParameter = methodParameters[index];

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
                    Regex pattern = new Regex(@"Requested value '(.*)' was not found\.");
                    Match match = pattern.Match(e.Message);
                    if (match.Groups.Count > 0)
                    {
                        throw new TextDecoder.Parser.ScriptParsingException($"'{parameters[index]}' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': Cannot convert '{match.Groups[1].Captures[0]}' into an {methodParameter.ParameterType} (valid values include: '{string.Join(", ", Enum.GetValues(methodParameter.ParameterType).Cast<object>().Select(a=>a.ToString()))}')");
                    }
                    throw;
                }
            }

            // Construct a parser for it
            Type parser = GetType().Assembly.GetTypes().FirstOrDefault(type => type.BaseType is { IsGenericType: true } && type.BaseType.GenericTypeArguments[0] == methodParameter.ParameterType);
            if (parser == null)
            {
                Debug.LogError($"The TextDecoder.Parser namespace contains no Parser for type {methodParameter.ParameterType}");
                return;
            }

            ConstructorInfo parserConstructor = parser.GetConstructor(Type.EmptyTypes);
            if (parserConstructor == null)
            {
                Debug.LogError($"TextDecoder.Parser for type {methodParameter.ParameterType} has no constructor without parameters");
                return;
            }

            // Find the 'Parse' method on that parser
            MethodInfo parseMethod = parser.GetMethod("Parse");
            if (parseMethod == null)
            {
                Debug.LogError($"TextDecoder.Parser for type {methodParameter.ParameterType} has no 'Parse' method");
                return;
            }

            // Create a parser and call the 'Parse' method
            object parserInstance = parserConstructor.Invoke(Array.Empty<object>());
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
        for (int suppliedParameterCount = parameters.Length; suppliedParameterCount < methodParameters.Length; suppliedParameterCount++)
        {
            parsedMethodParameters.Add(methodParameters[suppliedParameterCount].DefaultValue);
        }

        // Call the method
        method.Invoke(this, parsedMethodParameters.ToArray());
    }

    protected abstract void ADD_EVIDENCE(AssetName evidenceName);
    protected abstract void ADD_RECORD(AssetName actorName);
    protected abstract void PLAY_SFX(AssetName sfx);
    protected abstract void PLAY_SONG(AssetName songName);
    protected abstract void SCENE(AssetName sceneName);
    protected abstract void SHOW_ITEM(AssetName itemName, ItemDisplayPosition itemPos);
    protected abstract void ACTOR(AssetName actorName);
    protected abstract void SPEAK(AssetName actorName);
    protected abstract void SPEAK_UNKNOWN(AssetName actorName);
    protected abstract void THINK(AssetName actorName);
    protected abstract void SET_ACTOR_POSITION(int oneBasedSlotIndex, AssetName actorName);
}
