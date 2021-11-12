namespace TextDecoder.Parser
{
    public abstract class Parser<T>
    {
        public abstract string Parse(string input, out T output);
    }
}