namespace TextDecoder.Exceptions
{
    public class UnableToParseException : BaseDecoderException
    {
        public UnableToParseException(string typeName, string parameterName, string token)
            : base($"Failed to parse {parameterName}: cannot parse `{token}` as {typeName}")
        {
        }
    }
}