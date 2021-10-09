public class ActionLine
{
    private const char ACTION_SIDE_SEPARATOR = ':';
    public string Action { get; set; }
    public string Parameters { get; set; }

    public ActionLine(string line)
    {
        //Split into action and parameter
        string[] actionAndParam = line.Substring(1, line.Length - 2).Split(ACTION_SIDE_SEPARATOR);

        if (actionAndParam.Length > 2)
        {
            throw new InvalidSyntaxException(line);
        }

        Action = actionAndParam[0];
        Parameters = (actionAndParam.Length == 2) ? actionAndParam[1] : "";
    }
}
