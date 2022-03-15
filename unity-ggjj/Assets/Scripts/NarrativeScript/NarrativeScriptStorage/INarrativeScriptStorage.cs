public interface INarrativeScriptStorage
{
    NarrativeScript NarrativeScript { get; set; }
    NarrativeScript GameOverScript { get; }
    
    NarrativeScript GetRandomFailureScript();
    void SetGameOverScript(string gameOverScriptName);
    void AddFailureScript(string failureScriptName);
}
