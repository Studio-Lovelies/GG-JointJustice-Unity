using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class UnknownCommandException : Exception
{
    public string CommandName { get; private set; }

    public UnknownCommandException(string commandName)
    {
        CommandName = commandName;
    }
}

public class InvalidSyntaxException : Exception
{
    public string Line { get; private set; }

    public InvalidSyntaxException(string line)
    {
        Line = line;
    }
}

public class ActionDecoder
{
    /// <summary>
    /// Forwarded from the DirectorActionDecoder
    /// </summary>
    public UnityEvent OnActionDone { get; set; }

    public IActorController ActorController { get; set; }
    public ISceneController SceneController { get; set; }
    public IAudioController AudioController { get; set; }
    public IEvidenceController EvidenceController { get; set; }
    public IAppearingDialogueController AppearingDialogueController { get; set; } = null;

    public GameObject GameObject { get; set; }

    public ActionDecoder()
    {
    }

    public void OnNewActionLine(ActionLine actionLine)
    {
        switch (actionLine.Action)
        {
            //Actor controller
            case "ACTOR": SetActor(actionLine.FirstStringParameter()); break;
            case "SET_ACTOR_POSITION": SetActorPosition(actionLine.Parameters()); break;
            case "SHOWACTOR": SetActorVisibility(actionLine.FirstStringParameter()); break;
            case "SPEAK": SetSpeaker(actionLine.FirstStringParameter(), SpeakingType.Speaking); break;
            case "THINK": SetSpeaker(actionLine.FirstStringParameter(), SpeakingType.Thinking); break;
            case "SET_POSE": SetPose(actionLine.Parameters()); break;
            case "PLAY_EMOTION": PlayEmotion(actionLine.Parameters()); break; //Emotion = animation on an actor. Saves PLAY_ANIMATION for other things
            //Audio controller
            case "PLAYSFX": PlaySFX(actionLine.FirstStringParameter()); break;
            case "PLAYSONG": SetBGMusic(actionLine.FirstStringParameter()); break;
            case "STOP_SONG": StopSong(); break;
            //Scene controller
            case "FADE_OUT": FadeOutScene(actionLine.FirstStringParameter()); break;
            case "FADE_IN": FadeInScene(actionLine.FirstStringParameter()); break;
            case "CAMERA_PAN": PanCamera(actionLine.NextFloat(), actionLine.NextInt(), actionLine.NextInt()); break;
            case "CAMERA_SET": SetCameraPosition(actionLine.Parameters()); break;
            case "SHAKESCREEN": ShakeScreen(actionLine.FirstStringParameter()); break;
            case "SCENE": SetScene(actionLine.FirstStringParameter()); break;
            case "WAIT": Wait(actionLine.FirstStringParameter()); break;
            case "SHOW_ITEM": ShowItem(actionLine.Parameters()); break;
            case "HIDE_ITEM": HideItem(); break;
            case "PLAY_ANIMATION": PlayAnimation(actionLine.FirstStringParameter()); break;
            case "JUMP_TO_POSITION": JumpToActorSlot(actionLine.FirstStringParameter()); break;
            case "PAN_TO_POSITION": PanToActorSlot(actionLine.Parameters()); break;
            //Evidence controller
            case "ADD_EVIDENCE": AddEvidence(actionLine.FirstStringParameter()); break;
            case "REMOVE_EVIDENCE": RemoveEvidence(actionLine.FirstStringParameter()); break;
            case "ADD_RECORD": AddToCourtRecord(actionLine.FirstStringParameter()); break;
            case "PRESENT_EVIDENCE": OpenEvidenceMenu(); break;
            case "SUBSTITUTE_EVIDENCE": SubstituteEvidence(actionLine.FirstStringParameter()); break;
            //Dialog controller
            case "DIALOG_SPEED": ChangeDialogSpeed(WaiterType.Dialog, actionLine.FirstStringParameter()); break;
            case "OVERALL_SPEED": ChangeDialogSpeed(WaiterType.Overall, actionLine.FirstStringParameter()); break;
            case "PUNCTUATION_SPEED": ChangeDialogSpeed(WaiterType.Punctuation, actionLine.FirstStringParameter()); break;
            case "CLEAR_SPEED": ClearDialogSpeeds(); break;
            case "DISABLE_SKIPPING": DisableTextSkipping(actionLine.FirstStringParameter()); break;
            case "AUTOSKIP": AutoSkip(actionLine.FirstStringParameter()); break;
            case "CONTINUE_DIALOG": ContinueDialog(); break;
            case "APPEAR_INSTANTLY": AppearInstantly(); break;
            case "HIDE_TEXTBOX": HideTextbox(); break;
            //Do nothing
            case "WAIT_FOR_INPUT": break;
            //Default
            default: throw new UnknownCommandException(actionLine.Action);
        }
    }

    #region DialogStuff
    /// <summary>
    /// Checks if the decoder has an appearing dialog controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an appearing dialog controller is connected</returns>
    private bool HasAppearingDialogController()
    {
        if (AppearingDialogueController == null)
        {
            Debug.LogError("No appearing dialog controller attached to the action decoder", GameObject);
            return false;
        }
        return true;
    }

    ///<summary>
    ///Changes the dialog speed in appearingDialogController if it has beben set.
    ///</summary>
    ///<param name = "currentWaiterType">The current waiters type which appear time should be changed.</param>
    ///<param name = "parameters">Contains all the parameters needed to change the appearing time.</param>
    private void ChangeDialogSpeed(WaiterType currentWaiterType, string parameters)
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.SetTimerValue(currentWaiterType, parameters);
    }

    ///<summary>
    ///Clears all custom set dialog speeds
    ///</summary>
    private void ClearDialogSpeeds()
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.ClearAllWaiters();
    }

    ///<summary>
    ///Toggles skipping on or off
    ///</summary>
    ///<param name = "disable">Should the text skipping be disabled or not</param>
    private void DisableTextSkipping(string disabled)
    {
        if (!HasAppearingDialogController())
            return;

        if (!bool.TryParse(disabled, out bool value))
        {
            Debug.LogError("Bool value wasn't found from DisableTextSkipping command. Please fix.");
            return;
        }

        AppearingDialogueController.ToggleDisableTextSkipping(value);
    }

    ///<summary>
    ///Makes the new dialog appear after current one.
    ///</summary>
    private void ContinueDialog()
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.ContinueDialog();
    }

    ///<summary>
    ///Forces the next line of dialog happen right after current one.
    ///</summary>
    private void AutoSkip(string on)
    {
        if (!HasAppearingDialogController())
            return;

        if (!bool.TryParse(on, out bool value))
        {
            Debug.LogError("Bool value wasn't found from autoskip command. Please fix.");
            return;
        }

        AppearingDialogueController.AutoSkipDialog(value);
    }

    /// <summary>
    /// Makes the next line of dialogue appear instantly instead of one character at a time.
    /// </summary>
    private void AppearInstantly()
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.PrintTextInstantly = true;
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Hides the dialogue textbox.
    /// </summary>
    private void HideTextbox()
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.HideTextbox();
        OnActionDone.Invoke();
    }
    #endregion

    #region EvidenceController
    /// <summary>
    /// Checks if the decoder has an evidence controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an evidence controller is connected</returns>
    private bool HasEvidenceController()
    {
        if (EvidenceController == null)
        {
            Debug.LogError("No evidence controller attached to the action decoder");
            return false;
        }
        return true;
    }

    private void AddEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.AddEvidence(evidence);
        OnActionDone.Invoke();
    }

    private void RemoveEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.RemoveEvidence(evidence);
        OnActionDone.Invoke();
    }

    private void AddToCourtRecord(string actor)
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.AddToCourtRecord(actor);
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Calls the onPresentEvidence event on evidence controller which
    /// opens the evidence menu so evidence can be presented.
    /// </summary>
    private void OpenEvidenceMenu()
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.OpenEvidenceMenu();
    }

    /// <summary>
    /// Used to substitute a specified Evidence object with its assigned alternate Evidence object.
    /// </summary>
    /// <param name="evidence">The name of the evidence to substitute.</param>
    private void SubstituteEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.SubstituteEvidenceWithAlt(evidence);
        OnActionDone.Invoke();
    }

    #endregion


    #region AudioController
    /// <summary>
    /// Checks if the decoder has an audio controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an audio controller is connected</returns>
    private bool HasAudioController()
    {
        if (AudioController == null)
        {
            Debug.LogError("No audio controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Plays a sound effect
    /// </summary>
    /// <param name="sfx">Name of the sound effect</param>
    private void PlaySFX(string sfx)
    {
        if (!HasAudioController())
            return;

        AudioController.PlaySFX(sfx);
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Sets the background music
    /// </summary>
    /// <param name="songName">Name of the new song</param>
    private void SetBGMusic(string songName)
    {
        if (!HasAudioController())
            return;

        AudioController.PlaySong(songName);
        OnActionDone.Invoke();
    }

    /// <summary>
    /// If music is currently playing, stop it!
    /// </summary>
    private void StopSong()
    {
        if (!HasAudioController())
            return;

        AudioController.StopSong();
        OnActionDone.Invoke();
    }
    #endregion

    #region SceneController
    /// <summary>
    /// Checks if the decoder has a scene controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether a scene controller is connected</returns>
    private bool HasSceneController()
    {
        if (SceneController == null)
        {
            Debug.LogError("No scene controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Fades the scene in from black
    /// </summary>
    /// <param name="seconds">Amount of seconds the fade-in should take as a float</param>
    private void FadeInScene(string seconds)
    {
        if (!HasSceneController())
            return;

        float timeInSeconds;

        if (float.TryParse(seconds, NumberStyles.Any, CultureInfo.InvariantCulture, out timeInSeconds))
        {
            SceneController.FadeIn(timeInSeconds);
        }
        else
        {
            Debug.LogError("Invalid paramater " + seconds + " for function FADE_IN");
        }
    }

    /// <summary>
    /// Fades the scene to black
    /// </summary>
    /// <param name="seconds">Amount of seconds the fade-out should take as a float</param>
    private void FadeOutScene(string seconds)
    {
        if (!HasSceneController())
            return;

        float timeInSeconds;

        if (float.TryParse(seconds, NumberStyles.Any, CultureInfo.InvariantCulture, out timeInSeconds))
        {
            SceneController.FadeOut(timeInSeconds);
        }
        else
        {
            Debug.LogError("Invalid paramater " + seconds + " for function FADE_OUT");
        }
    }

    /// <summary>
    /// Shakes the screen
    /// </summary>
    /// <param name="intensity">Max displacement of the screen as a float</param>
    private void ShakeScreen(string intensity)
    {
        if (!HasSceneController())
            return;

        float intensityNumerical;

        if (float.TryParse(intensity, NumberStyles.Any, CultureInfo.InvariantCulture, out intensityNumerical))
        {
            SceneController.ShakeScreen(intensityNumerical);
        }
        else
        {
            Debug.LogError("Invalid paramater " + intensity + " for function SHAKESCREEN");
        }
    }

    /// <summary>
    /// Sets the scene (background, character location on screen, any props (probably prefabs))
    /// </summary>
    /// <param name="sceneName">Scene to change to</param>
    private void SetScene(string sceneName)
    {
        if (!HasSceneController())
            return;

        SceneController.SetScene(sceneName);
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Sets the camera position
    /// </summary>
    /// <param name="parameters">New camera coordinates in the "int x,int y" format</param>
    private void SetCameraPosition(string[] parameters)
    {
        if (!HasSceneController())
            return;

        if (parameters.Length != 2)
        {
            Debug.LogError("Invalid amount of parameters for function CAMERA_SET");
            return;
        }

        int x;
        int y;

        if (int.TryParse(parameters[0], out x)
            && int.TryParse(parameters[1], out y))
        {
            SceneController.SetCameraPos(new Vector2Int(x, y));
        }
        else
        {
            Debug.LogError("Invalid paramater " + string.Join(",", parameters) + " for function CAMERA_SET");
        }
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Pan the camera to a certain x,y position
    /// </summary>
    private void PanCamera(float duration, int x, int y)
    {
        if (!HasSceneController())
            return;

        SceneController.PanCamera(duration, new Vector2Int(x, y));
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Shows an item on the middle, left, or right side of the screen.
    /// </summary>
    /// <param name="parameters">Which item to show and where to show it, in the "string item, itemPosition pos" format</param>
    private void ShowItem(string[] parameters)
    {
        if (!HasSceneController())
            return;

        if (parameters.Length != 2)
        {
            Debug.LogError("Invalid amount of parameters for function SHOW_ITEM");
            return;
        }

        itemDisplayPosition pos;
        if (System.Enum.TryParse<itemDisplayPosition>(parameters[1], out pos))
        {
            SceneController.ShowItem(parameters[0], pos);
        }
        else
        {
            Debug.LogError("Invalid paramater " + parameters[1] + " for function CAMERA_PAN");
        }
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Hides the item displayed on the screen by ShowItem method.
    /// </summary>
    private void HideItem()
    {
        if (!HasSceneController())
            return;

        SceneController.HideItem();
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Waits seconds before automatically continuing.
    /// </summary>
    /// <param name="seconds">Amount of seconds to wait</param>
    private void Wait(string seconds)
    {
        if (!HasSceneController())
            return;

        float secondsFloat;

        if (float.TryParse(seconds, NumberStyles.Any, CultureInfo.InvariantCulture, out secondsFloat))
        {
            SceneController.Wait(secondsFloat);
        }
        else
        {
            Debug.LogError("Invalid paramater " + seconds + " for function WAIT");
        }
    }

    /// <summary>
    /// Plays a full screen animation e.g. Ross' galaxy brain or the gavel hit animations.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    private void PlayAnimation(string animationName)
    {
        if (!HasSceneController())
            return;

        SceneController.PlayAnimation(animationName);
    }

    /// Jump-cuts the camera to the target sub position if the bg-scene has sub positions.
    /// </summary>
    /// <param name="oneBasedSlotIndexAsString">String containing an integer referring to the target sub position, 1 based.</param>
    private void JumpToActorSlot(string oneBasedSlotIndexAsString)
    {
        if (!HasSceneController())
        {
            return;
        }

        if (!int.TryParse(oneBasedSlotIndexAsString, out int oneBasedSlotIndex))
        {
            Debug.LogError("First parameter needs to be a one-based integer index");
            return;
        }

        SceneController.JumpToActorSlot(oneBasedSlotIndex);
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Pans the camera to the target actor slot if the bg-scene has support for actor slots.
    /// </summary>
    /// <param name="parameters">String containing a one-based integer index referring to the target actor slot, and a floating point number referring to the amount of time the pan should take in seconds.</param>
    private void PanToActorSlot(string[] parameters)
    {
        if (!HasSceneController())
        {
            return;
        }

        if (parameters.Length != 2)
        {
            Debug.LogError("PAN_TO_POSITION requires exactly 2 parameters: [slotIndex <int>], [panDurationInSeconds <float>]");
            return;
        }

        if (!int.TryParse(parameters[0], out int oneBasedSlotIndex))
        {
            Debug.LogError("First parameter needs to be a one-based integer index");
            return;
        }

        if (!float.TryParse(parameters[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float timeInSeconds))
        {
            Debug.LogError("Second parameter needs to be a floating point number");
            return;
        }

        SceneController.PanToActorSlot(oneBasedSlotIndex, timeInSeconds);
        OnActionDone.Invoke();
    }

    #endregion


    #region ActorController
    /// <summary>
    /// Checks if the decoder has an actor controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an actor controller is connected</returns>
    private bool HasActorController()
    {
        if (ActorController == null)
        {
            Debug.LogError("No actor controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Sets the shown actor in the scene
    /// </summary>
    /// <param name="actor">Actor to be switched to</param>
    private void SetActor(string actor)
    {
        if (!HasActorController())
            return;

        ActorController.SetActiveActor(actor);
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Shows or hides the actor based on the string parameter.
    /// </summary>
    /// <param name="showActor">Should contain true or false based on showing or hiding the actor respectively</param>
    private void SetActorVisibility(string showActor)
    {
        if (!HasSceneController())
            return;

        bool shouldShow;
        if (bool.TryParse(showActor, out shouldShow))
        {
            if (shouldShow)
            {
                SceneController.ShowActor();
            }
            else
            {
                SceneController.HideActor();
            }
        }
        else
        {
            Debug.LogError("Invalid paramater " + showActor + " for function SHOWACTOR");
        }
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Set the speaker for the current and following lines, until a new speaker is set
    /// </summary>
    /// <param name="actor">Actor to make the speaker</param>
    /// <param name="speakingType">Type of speaking to speak the text with</param>
    private void SetSpeaker(string actor, SpeakingType speakingType)
    {
        if (!HasActorController())
            return;

        ActorController.SetActiveSpeaker(actor);
        ActorController.SetSpeakingType(speakingType);
        OnActionDone.Invoke();
    }

    /// <summary>
    /// Set the pose of the current actor
    /// </summary>
    /// <param name="parameters">"[pose name]" to set pose for current actor OR "[pose name],[actor name]" to set pose for another actor</param>
    private void SetPose(string[] parameters)
    {
        if (!HasActorController())
            return;

        if (parameters.Length == 1)
        {
            ActorController.SetPose(parameters[0]);
            OnActionDone.Invoke();
        }
        else if (parameters.Length == 2)
        {
            ActorController.SetPose(parameters[0], parameters[1]);
            OnActionDone.Invoke();
        }
        else
        {
            Debug.LogError("Invalid number of parameters for function PLAY_EMOTION");
        }


    }

    /// <summary>
    /// Plays an emotion for the current actor. Emotion is a fancy term for animation on an actor.
    /// </summary>
    /// <param name="parameters">"[animation name]" to set pose for current actor OR "[animation name],[actor name]" to queue animation for another actor (gets played as soon as actor is visible)</param>
    private void PlayEmotion(string[] parameters)
    {
        if (!HasActorController())
            return;

        if (parameters.Length == 1)
        {
            ActorController.PlayEmotion(parameters[0]);
        }
        else if (parameters.Length == 2)
        {
            ActorController.PlayEmotion(parameters[0], parameters[1]);
        }
        else
        {
            Debug.LogError("Invalid number of parameters for function PLAY_EMOTION");
        }
    }

    /// <summary>
    /// Sets an actor to a specific slot in the currently active scene.
    /// </summary>
    /// <param name="parameters">String containing the actor name first and one-based slot index second.</param>
    private void SetActorPosition(string[] parameters)
    {
        if (!HasActorController())
        {
            return;
        }

        if (parameters.Length != 2)
        {
            Debug.LogError("SET_ACTOR_POSITION requires exactly 2 parameters: [actorName <string>], [slotIndex <int>]");
            return;
        }

        string actorName = parameters[1];
        if (!int.TryParse(parameters[0], out int oneBasedSlotIndex))
        {
            Debug.LogError("Second parameter needs to be a one-based integer index");
            return;
        }

        ActorController.AssignActorToSlot(actorName, oneBasedSlotIndex);
        OnActionDone.Invoke();
    }
    #endregion

}
