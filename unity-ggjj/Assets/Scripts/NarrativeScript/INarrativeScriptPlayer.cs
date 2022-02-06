public interface INarrativeScriptPlayer
{
    GameMode GameMode { get; set; }
    
    void StartSubStory(NarrativeScript narrativeScript);
}
