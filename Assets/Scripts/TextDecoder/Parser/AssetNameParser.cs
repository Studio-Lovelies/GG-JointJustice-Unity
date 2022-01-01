namespace TextDecoder.Parser
{
    public class AssetNameParser : Parser<AssetName>
    {
        public override string Parse(string input, out AssetName output)
        {
            output = new AssetName(input);
            return null;
        }
    }
}
