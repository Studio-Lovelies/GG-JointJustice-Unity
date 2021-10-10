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
