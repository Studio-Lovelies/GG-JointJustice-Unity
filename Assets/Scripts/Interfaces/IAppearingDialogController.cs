using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAppearingDialogController
{
    void ContinueDialog();
    void SetTimerValue(WaiterType waiterTypeToChange, string valueToTurnFloat);
    void ToggleDisableTextSkipping(bool disabled);
    void ClearAllWaiters();
    void AutoSkipDialog(bool skip);
}
