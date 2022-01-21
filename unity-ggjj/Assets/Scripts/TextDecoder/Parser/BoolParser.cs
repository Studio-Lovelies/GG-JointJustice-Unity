namespace TextDecoder.Parser
{
    public class BoolParser : Parser<bool>
    {
        public override string Parse(string input, out bool output)
        {
            if (!bool.TryParse(input, out output))
            {
                return $"Must be either 'true' or 'false'";
            }

            return null;
        }
    }
}