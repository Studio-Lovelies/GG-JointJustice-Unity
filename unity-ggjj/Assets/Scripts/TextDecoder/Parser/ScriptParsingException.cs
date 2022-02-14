using System;

namespace TextDecoder.Parser
{
    public class ScriptParsingException : Exception
    {
        public ScriptParsingException(string message) : base(message)
        {
        }
    }
}