public interface INarrativeScriptPlayerComponent
{
    INarrativeScriptPlayer NarrativeScriptPlayer { get; }
    void LoadScriptByName(string narrativeScriptName);
}
