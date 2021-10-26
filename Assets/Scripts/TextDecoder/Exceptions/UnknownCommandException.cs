namespace TextDecoder.Exceptions
{
    public class UnknownCommandException : BaseDecoderException
    {
        public UnknownCommandException(string commandName) : base($"Unknown command '{commandName}'")
        {
        }
    }
}