public interface IAppearingDialogueController
{
    bool PrintTextInstantly { get; set; }

    void ContinueDialog();
    void SetTimerValue(WaiterType waiterTypeToChange, float valueToTurnFloat);
    void ToggleDisableTextSkipping(bool disabled);
    void ClearAllWaiters();
    void AutoSkipDialog(bool skip);
    void HideTextbox();
}

public interface IAppearingText
{
    float DefaultPunctuationDelay { set; }
    float CharactersPerSecond { set; }
    bool SkippingDisabled { get; set; }
    bool ContinueDialogue { get; set; }
    bool AutoSkip { get; set; }
    bool AppearInstantly { get; set; }
    bool TextBoxHidden { set; }
}
