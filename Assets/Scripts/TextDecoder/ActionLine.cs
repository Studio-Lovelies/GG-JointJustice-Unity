public class ActionLine
{
    private const char ACTION_SIDE_SEPARATOR = ':';
    private const char ACTION_PARAMETER_SEPARATOR = ',';

    private readonly string fullParametersString;
    private readonly string[] splitParameters;

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
}
