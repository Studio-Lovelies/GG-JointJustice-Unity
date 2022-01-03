public class AssetName
{
    public string NormalizedName { get; }
    public static implicit operator string(AssetName assetName)
    {
        return assetName.NormalizedName;
    }

    public AssetName(string givenName)
    {
        NormalizedName = SplitAndCapitalize('_', givenName);
        NormalizedName = SplitAndCapitalize(' ', NormalizedName);
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

}
