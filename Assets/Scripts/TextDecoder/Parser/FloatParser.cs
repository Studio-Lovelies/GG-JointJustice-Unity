namespace TextDecoder.Parser
{
    public class FloatParser : Parser<float>
    {
        public override string Parse(string input, out float output)
        {
            if (!float.TryParse(input, out output))
            {
                return $"Must be a number (with decimals delimited with '.' instead of ',')";
            }

            return null;
        }
    }
}