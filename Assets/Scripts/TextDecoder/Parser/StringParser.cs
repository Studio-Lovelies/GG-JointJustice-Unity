namespace TextDecoder.Parser
{
    public class StringParser : Parser<string>
    {
        public override string Parse(string input, out string output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = input;
            return null;
        }
    }
}