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

public abstract class CourtRecordItemName : AssetName
{
    protected CourtRecordItemName(string givenName) : base(givenName) { }
}

public class EvidenceAssetName : CourtRecordItemName
{
    public EvidenceAssetName(string givenName) : base(givenName) { }
}

public class ActorAssetName : CourtRecordItemName
{
    public ActorAssetName(string givenName) : base(givenName) { }
}

public class ActorPoseAssetName : AssetName
{
    public ActorPoseAssetName(string givenName) : base(givenName) { }
}

public class NarrativeScriptAssetName : AssetName
{
    public NarrativeScriptAssetName(string givenName) : base(givenName) { }
}

public class GameOverScriptAssetName : AssetName
{
    public GameOverScriptAssetName(string givenName) : base(givenName) { }
}

public class FailureScriptAssetName : AssetName
{
    public FailureScriptAssetName(string givenName) : base(givenName) { }
}

public class UnitySceneAssetName : AssetName
{
    public UnitySceneAssetName(string givenName) : base(givenName) { }
}