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
    public class StaticSongAssetNameParser : Parser<StaticSongAssetName>
    {
        public override string Parse(string input, out StaticSongAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new StaticSongAssetName(input);
            return null;
        }
    }
    public class DynamicSongAssetNameParser : Parser<DynamicSongAssetName>
    {
        public override string Parse(string input, out DynamicSongAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new DynamicSongAssetName(input);
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
    public class CourtRecordItemNameParser : Parser<CourtRecordItemName>
    {
        public override string Parse(string input, out CourtRecordItemName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            // EvidenceAssetName is mismatched by design; CourtRecordItemName is abstract and can't be constructed directly
            output = new EvidenceAssetName(input);
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
    public class NarrativeScriptAssetNameParser : Parser<NarrativeScriptAssetName>
    {
        public override string Parse(string input, out NarrativeScriptAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new NarrativeScriptAssetName(input);
            return null;
        }
    }
    public class GameOverScriptAssetNameParser : Parser<GameOverScriptAssetName>
    {
        public override string Parse(string input, out GameOverScriptAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new GameOverScriptAssetName(input);
            return null;
        }
    }
    public class FailureScriptAssetNameParser : Parser<FailureScriptAssetName>
    {
        public override string Parse(string input, out FailureScriptAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new FailureScriptAssetName(input);
            return null;
        }
    }
    public class UnitySceneAssetNameParser : Parser<UnitySceneAssetName>
    {
        public override string Parse(string input, out UnitySceneAssetName output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }

            output = new UnitySceneAssetName(input);
            return null;
        }
    }
}
