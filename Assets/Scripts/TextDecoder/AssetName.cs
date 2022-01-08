public class AssetName
{
    private readonly string _internalString;
    public static implicit operator string(AssetName assetName)
    {
        return assetName.ToString();
    }

    public AssetName(string givenName)
    {
        _internalString = SplitAndCapitalize('_', givenName);
        _internalString = SplitAndCapitalize(' ', _internalString);
    }

    private string SplitAndCapitalize(char c, string givenName)
    {
        var result = string.Empty;
        foreach (var item in givenName.Split(c))
        {
            var firstChar = item[0].ToString().ToUpper()[0];
            var node = $"{firstChar}{item.Substring(1, item.Length - 1)}";
            result += node;
        }

        return result;
    }

    public override string ToString()
    {
        return _internalString;
    }
}
