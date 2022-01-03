namespace TextDecoder.Parser
{
    public class AssetNameParser : Parser<AssetName>
    {
        public override string Parse(string input, out AssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new AssetName(input);
            return null;
        }
    }
}
