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

public class FullscreenAnimationAssetName : AssetName
{
    public FullscreenAnimationAssetName(string givenName) : base(givenName) { }
}

public class SceneAssetName : AssetName
{
    public SceneAssetName(string givenName) : base(givenName) { }
}

public class SfxAssetName : AssetName
{
    public SfxAssetName(string givenName) : base(givenName) { }
}

public class SongAssetName : AssetName
{
    public SongAssetName(string givenName) : base(givenName) { }
}

public class EvidenceAssetName : AssetName
{
    public EvidenceAssetName(string givenName) : base(givenName) { }
}

public class ActorPoseAssetName : AssetName
{
    public ActorPoseAssetName(string givenName) : base(givenName) { }
}

public class ActorAssetName : AssetName
{
    public ActorAssetName(string givenName) : base(givenName) { }
}