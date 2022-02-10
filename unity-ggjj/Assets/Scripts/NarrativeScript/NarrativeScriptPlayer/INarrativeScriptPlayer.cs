public interface INarrativeScriptPlayer
{
    bool Waiting { get; set; }
    GameMode GameMode { get; set; }
    
    void Continue();
    void StartSubStory(NarrativeScript gameOverScript);
    void HandleChoice(int choiceIndex);
}
