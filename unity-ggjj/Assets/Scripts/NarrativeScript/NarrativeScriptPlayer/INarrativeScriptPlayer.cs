using UnityEngine.InputSystem;

public interface INarrativeScriptPlayer
{
    INarrativeScript ActiveNarrativeScript { get; set; }
    GameMode GameMode { get; set; }
    bool Waiting { get; set; }
    bool CanPressWitness { get; }
    bool HasSubStory { get; }
    void ToggleSpeedup(InputAction.CallbackContext inputCallbackContext);
    
    void Continue(bool overridePrintingText = false);
    void HandleChoice(int choiceIndex);
    void PresentEvidence(ICourtRecordObject courtRecordObject);
    void StartSubStory(NarrativeScript narrativeScript);
    void SetWaitingToFalseAndContinue();
    void TryPressWitness();
}