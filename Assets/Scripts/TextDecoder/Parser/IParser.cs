using System;

namespace TextDecoder.Parser
{
    public interface IParser
    {
        public bool Applies(Type type);
    }
}