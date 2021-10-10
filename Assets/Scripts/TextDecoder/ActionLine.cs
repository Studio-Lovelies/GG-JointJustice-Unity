using System;
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


public class ActionLine
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
