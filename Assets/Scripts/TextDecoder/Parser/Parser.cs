using System;

namespace TextDecoder.Parser
{
    public abstract class Parser<T> : IParser
    {
        public abstract string Parse(string input, out T output);

        public bool Applies(Type type)
        {
            return typeof(T) == type;
        }
    }
}