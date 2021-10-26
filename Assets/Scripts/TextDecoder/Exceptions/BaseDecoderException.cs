using System;

namespace TextDecoder.Exceptions
{
    public abstract class BaseDecoderException : Exception
    {
        protected BaseDecoderException(string message) : base(message)
        {

        }
    }
}