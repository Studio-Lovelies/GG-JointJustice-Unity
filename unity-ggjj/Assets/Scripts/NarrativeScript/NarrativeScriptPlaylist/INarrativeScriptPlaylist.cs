public interface INarrativeScriptPlaylist
{
    NarrativeScript DefaultNarrativeScript { get; }
    NarrativeScript GetRandomFailureScript();
    void SetGameOverScript(NarrativeScript narrativeScript);
    void AddFailureScript(NarrativeScript narrativeScript);
}
