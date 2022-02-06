public interface INarrativeScriptPlayer
{
    StoryPlayer StoryPlayer { set; }
    GameMode GameMode { get; set; }
    bool HasSubStory { get; }
    bool CanPressWitness { get; }
    NarrativeScript ActiveNarrativeScript { get; }

    void Continue();
    void StartSubStory(NarrativeScript gameOverScript);
}
