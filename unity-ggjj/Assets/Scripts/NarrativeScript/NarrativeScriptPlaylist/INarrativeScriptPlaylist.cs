public interface INarrativeScriptPlaylist
{
    public NarrativeScript GameOverScript { get; set; }
    
    NarrativeScript DefaultNarrativeScript { get; }
    NarrativeScript GetRandomFailureScript();
    void AddFailureScript(NarrativeScript narrativeScript);
}
