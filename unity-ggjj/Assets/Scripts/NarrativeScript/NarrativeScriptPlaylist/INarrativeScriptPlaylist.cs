public interface INarrativeScriptPlaylist
{
    NarrativeScript NarrativeScript { get; set; }
    NarrativeScript GetRandomFailureScript();
    void SetGameOverScript(string gameOverScriptName);
    void AddFailureScript(string failureScriptName);
}
