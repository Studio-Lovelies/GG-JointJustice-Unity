namespace TextDecoder.Parser
{
    public class IntParser : Parser<int>
    {
        public override string Parse(string input, out int output)
        {
            if (!int.TryParse(input, out output))
            {
                return $"Must be a number with no decimals";
            }

            return null;
        }
    }
}