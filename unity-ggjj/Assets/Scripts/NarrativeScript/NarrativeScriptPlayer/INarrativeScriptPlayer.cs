public interface INarrativeScriptPlayer
{
    public INarrativeScript ActiveNarrativeScript { get; }
    public GameMode GameMode { get; set; }
    public bool Waiting { get; set; }
    public bool CanPressWitness { get; }
    public bool HasSubStory { get; }
    
    public void Continue(bool overridePrintingText = false);
    public void HandleChoice(int choiceIndex);
    public void PresentEvidence(ICourtRecordObject courtRecordObject);
    public void StartSubStory(NarrativeScript narrativeScript);
    public void SetWaitingToFalseAndContinue();
}