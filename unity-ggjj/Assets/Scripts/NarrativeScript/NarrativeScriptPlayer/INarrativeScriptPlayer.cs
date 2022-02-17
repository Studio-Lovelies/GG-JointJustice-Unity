public interface INarrativeScriptPlayer
{
    bool Waiting { get; set; }
    GameMode GameMode { get; set; }
    bool CanPressWitness { get; }
    NarrativeScript ActiveNarrativeScript { get; }

    void Continue();
    void StartSubStory(NarrativeScript gameOverScript);
    void HandleChoice(int choiceIndex);
    void PresentEvidence(ICourtRecordObject evidence);
}
