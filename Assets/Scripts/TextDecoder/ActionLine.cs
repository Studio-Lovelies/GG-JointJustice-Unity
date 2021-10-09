using System.Globalization;

public class UnableToParseException : System.Exception
{
    public UnableToParseException(string typeName, string token) : base($"Unable to parse {token} as {typeName}.")
    {
    }
}

public class NotEnoughParametersException : System.Exception
{
    public NotEnoughParametersException()
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

    public string Action { get; set; }

    public ActionLine(string line)
    {
        //Split into action and parameter
        string[] actionAndParam = line.Substring(1, line.Length - 2).Split(ACTION_SIDE_SEPARATOR);

        if (actionAndParam.Length > 2)
        {
            throw new InvalidSyntaxException(line);
        }

        Action = actionAndParam[0];
        fullParametersString = (actionAndParam.Length == 2) ? actionAndParam[1] : "";
        this.splitParameters = fullParametersString.Split(ACTION_PARAMETER_SEPARATOR);
    }

    public string[] Parameters()
    {
        return this.splitParameters;
    }

    public string FirstStringParameter()
    {
        return this.splitParameters[0];
    }

    private string NextToken()
    {
        if (parameterIndex < this.splitParameters.Length)
        {
            var parameter = this.splitParameters[parameterIndex];
            this.parameterIndex++;
            return parameter;
        }
        else
        {
            throw new NotEnoughParametersException();
        }
    }

    public string NextString()
    {
        return NextToken();
    }

    public float NextFloat()
    {
        var token = NextToken();
        if (float.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out float value))
        {
            return value;
        }
        else
        {
            throw new UnableToParseException("float", token);
        }
    }

    public int NextInt()
    {
        var token = NextToken();
        if (int.TryParse(token, out int value))
        {
            return value;
        }
        else
        {
            throw new UnableToParseException("int", token);
        }
    }
}
