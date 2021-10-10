using System;
using System.Collections.Generic;
using System.Globalization;

public class UnableToParseException : System.Exception
{
    public UnableToParseException(string typeName, string parameterName, string token)
        : base($"Failed to parse {parameterName}: cannot parse `{token}` as {typeName}")
    {
    }
}

public class NotEnoughParametersException : System.Exception
{
    public NotEnoughParametersException(string tokenName)
        : base($"Not enough parameters, missing: {tokenName}")
    {
    }
}

public interface IActionLine
{
    ActionName Action { get; }

    string NextString(string input);
    string NextOptionalString(string input);
    int NextInt(string input);
    float NextFloat(string input);
    int NextOneBasedInt(string input);
    bool NextBool(string input);
    T NextEnumValue<T>(string input) where T : struct, IConvertible;
}

public class DocActionLine : IActionLine
{
    private List<string> args = new List<string>();
    public ActionName Action { get; set; }

    public DocActionLine(ActionName action)
    {
        Action = action;
    }


    public bool NextBool(string argName)
    {
        args.Add(argName + ":bool");
        return true;
    }

    public T NextEnumValue<T>(string argName) where T : struct, IConvertible
    {
        args.Add($"{argName}:[{string.Join(",", typeof(T).GetEnumNames())}]");

        // I apologize for this line
        return (typeof(T).GetEnumValues() as T[])[0];
    }

    public float NextFloat(string argName)
    {
        args.Add($"{argName}:decimal number");
        return 0f;
    }

    public int NextInt(string argName)
    {
        args.Add($"{argName}:int");
        return 0;
    }

    public int NextOneBasedInt(string argName)
    {
        args.Add($"{argName}:one-based int");
        return 0;
    }

    public string NextOptionalString(string argName)
    {
        args.Add($"{argName}:string (optional)");
        return null;
    }

    public string NextString(string argName)
    {
        args.Add($"{argName}:string");
        return "";
    }

    public string Description()
    {
        return $"{Action.ToString()} takes args {string.Join(", ", args)}";
    }
}

public class ActionLine : IActionLine
{
    private const char ACTION_SIDE_SEPARATOR = ':';
    private const char ACTION_PARAMETER_SEPARATOR = ',';

    private readonly string fullParametersString;
    private readonly string[] splitParameters;
    private int parameterIndex;

    public ActionName Action { get; set; }

    public ActionLine(string line)
    {
        //Split into action and parameter
        string[] actionAndParam = line.Substring(1, line.Length - 2).Split(ACTION_SIDE_SEPARATOR);

        if (actionAndParam.Length > 2)
        {
            throw new InvalidSyntaxException();
        }

        var actionNameAsString = actionAndParam[0];

        if (Enum.TryParse(actionNameAsString, out ActionName actionNameAsEnum))
        {
            Action = actionNameAsEnum;
        }
        else
        {
            throw new UnknownCommandException(actionNameAsString);
        }
        fullParametersString = (actionAndParam.Length == 2) ? actionAndParam[1] : "";
        this.splitParameters = fullParametersString.Split(ACTION_PARAMETER_SEPARATOR);
    }

    public string[] Parameters()
    {
        return this.splitParameters;
    }

    private string NextToken(string tokenName)
    {
        if (parameterIndex < this.splitParameters.Length)
        {
            var parameter = this.splitParameters[parameterIndex];
            this.parameterIndex++;
            return parameter;
        }
        else
        {
            throw new NotEnoughParametersException(tokenName);
        }
    }

    public string NextString(string tokenName)
    {
        return NextToken(tokenName);
    }

    public float NextFloat(string tokenName)
    {
        var token = NextToken(tokenName);
        if (float.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out float value))
        {
            return value;
        }
        else
        {
            throw new UnableToParseException("decimal value", tokenName, token);
        }
    }

    public bool NextBool(string tokenName)
    {
        var token = NextToken(tokenName);
        if (bool.TryParse(token, out bool value))
        {
            return value;
        }
        else
        {
            throw new UnableToParseException("true or false", tokenName, token);
        }
    }

    public int NextInt(string tokenName)
    {
        var token = NextToken(tokenName);
        if (int.TryParse(token, out int value))
        {
            return value;
        }
        else
        {
            throw new UnableToParseException("integer", tokenName, token);
        }
    }

    public int NextOneBasedInt(string tokenName)
    {
        var nextInt = NextInt(tokenName);

        if (nextInt > 0)
        {
            return nextInt;
        }
        else
        {
            throw new UnableToParseException("one-based integer", tokenName, nextInt.ToString());
        }
    }

    public T NextEnumValue<T>(string tokenName) where T : struct, IConvertible
    {
        var token = NextToken(tokenName);

        if (Enum.TryParse(token, out T value))
        {
            return value;
        }
        else
        {
            throw new UnableToParseException(string.Join("/", typeof(T).GetEnumNames()), tokenName, token);
        }
    }

    public string NextOptionalString(string tokenName)
    {
        try
        {
            return NextString(tokenName);
        }
        catch (NotEnoughParametersException e)
        {
            return null;
        }
    }
}
