public interface INarrativeScriptPlaylist
{
    NarrativeScript DefaultNarrativeScript { get; }
    NarrativeScript GetRandomFailureScript();
}
