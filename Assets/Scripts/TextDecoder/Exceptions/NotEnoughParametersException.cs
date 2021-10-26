namespace TextDecoder.Exceptions
{
    public class NotEnoughParametersException : BaseDecoderException
    {
        public NotEnoughParametersException(string tokenName)
            : base($"Not enough parameters, missing: {tokenName}")
        {
        }
    }
}