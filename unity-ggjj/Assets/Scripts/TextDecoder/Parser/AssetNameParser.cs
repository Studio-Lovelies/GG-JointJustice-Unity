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
    public class FullscreenAnimationAssetNameParser : Parser<FullscreenAnimationAssetName>
    {
        public override string Parse(string input, out FullscreenAnimationAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new FullscreenAnimationAssetName(input);
            return null;
        }
    }
    public class SceneAssetNameParser : Parser<SceneAssetName>
    {
        public override string Parse(string input, out SceneAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new SceneAssetName(input);
            return null;
        }
    }
    public class SfxAssetNameParser : Parser<SfxAssetName>
    {
        public override string Parse(string input, out SfxAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new SfxAssetName(input);
            return null;
        }
    }
    public class SongAssetNameParser : Parser<SongAssetName>
    {
        public override string Parse(string input, out SongAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new SongAssetName(input);
            return null;
        }
    }
    public class EvidenceAssetNameParser : Parser<EvidenceAssetName>
    {
        public override string Parse(string input, out EvidenceAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new EvidenceAssetName(input);
            return null;
        }
    }
    public class ActorPoseAssetNameParser : Parser<ActorPoseAssetName>
    {
        public override string Parse(string input, out ActorPoseAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new ActorPoseAssetName(input);
            return null;
        }
    }
    public class ActorAssetNameParser : Parser<ActorAssetName>
    {
        public override string Parse(string input, out ActorAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new ActorAssetName(input);
            return null;
        }
    }
}
