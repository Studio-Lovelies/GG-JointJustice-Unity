public interface INarrativeScriptPlaylist
{
    NarrativeScript NarrativeScript { get; set; }
    NarrativeScript GetRandomFailureScript();
}
