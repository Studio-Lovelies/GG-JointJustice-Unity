using UnityEngine;
using UnityEngine.Events;

public class ActionDecoder
{
    /// <summary>
    /// Forwarded from the DirectorActionDecoder
    /// </summary>
    public UnityEvent _onActionDone { get; set; }

    public IActorController _actorController { get; set; }
    public ISceneController _sceneController { get; set; }
    public IAudioController _audioController { get; set; }
    public IEvidenceController _evidenceController { get; set; }
    public IAppearingDialogueController _appearingDialogController { get; set; } = null;

    public GameObject _gameObject { get; set; }

    public ActionDecoder()
    {
    }

    #region DialogStuff
    /// <summary>
    /// Checks if the decoder has an appearing dialog controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an appearing dialog controller is connected</returns>
    public bool HasAppearingDialogController()
    {
        if (_appearingDialogController == null)
        {
            Debug.LogError("No appearing dialog controller attached to the action decoder", _gameObject);
            return false;
        }
        return true;
    }

    ///<summary>
    ///Changes the dialog speed in appearingDialogController if it has beben set.
    ///</summary>
    ///<param name = "currentWaiterType">The current waiters type which appear time should be changed.</param>
    ///<param name = "parameters">Contains all the parameters needed to change the appearing time.</param>
    public void ChangeDialogSpeed(WaiterType currentWaiterType, string parameters)
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.SetTimerValue(currentWaiterType, parameters);
    }

    ///<summary>
    ///Clears all custom set dialog speeds
    ///</summary>
    public void ClearDialogSpeeds()
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.ClearAllWaiters();
    }

    ///<summary>
    ///Toggles skipping on or off
    ///</summary>
    ///<param name = "disable">Should the text skipping be disabled or not</param>
    public void DisableTextSkipping(string disabled)
    {
        if (!HasAppearingDialogController())
            return;

        if (!bool.TryParse(disabled, out bool value))
        {
            Debug.LogError("Bool value wasn't found from DisableTextSkipping command. Please fix.");
            return;
        }

        _appearingDialogController.ToggleDisableTextSkipping(value);
    }

    ///<summary>
    ///Makes the new dialog appear after current one.
    ///</summary>
    public void ContinueDialog()
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.ContinueDialog();
    }

    ///<summary>
    ///Forces the next line of dialog happen right after current one.
    ///</summary>
    public void AutoSkip(string on)
    {
        if (!HasAppearingDialogController())
            return;

        if (!bool.TryParse(on, out bool value))
        {
            Debug.LogError("Bool value wasn't found from autoskip command. Please fix.");
            return;
        }

        _appearingDialogController.AutoSkipDialog(value);
    }

    /// <summary>
    /// Makes the next line of dialogue appear instantly instead of one character at a time.
    /// </summary>
    public void AppearInstantly()
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.PrintTextInstantly = true;
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Hides the dialogue textbox.
    /// </summary>
    public void HideTextbox()
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.HideTextbox();
        _onActionDone.Invoke();
    }
    #endregion
}
