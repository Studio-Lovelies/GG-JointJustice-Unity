using System;

namespace TextDecoder.Parser
{
    public class MissingParserException : Exception
    {
        public MissingParserException(string message) : base(message)
        {
        }
    }
}