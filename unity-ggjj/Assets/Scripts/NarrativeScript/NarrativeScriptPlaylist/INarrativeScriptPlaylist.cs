public interface INarrativeScriptPlaylist
{
    NarrativeScript DefaultNarrativeScript { get; }
    NarrativeScript GetRandomFailureScript();
    void SetGameOverScript(string gameOverScriptName);
    void AddFailureScript(string failureScriptName);
}
