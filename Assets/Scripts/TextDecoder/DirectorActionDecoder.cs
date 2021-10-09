using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class DirectorActionDecoder : MonoBehaviour
{
    private const char ACTION_SIDE_SEPARATOR = ':';
    private const char ACTION_PARAMETER_SEPARATOR = ',';

    private IActorController _actorController;
    private ISceneController _sceneController;
    private IAudioController _audioController;
    private IEvidenceController _evidenceController;
    private IAppearingDialogueController _appearingDialogController = null;

    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    /// <summary>
    /// Called whenever a new action is executed (encountered and then forwarded here) in the script
    /// </summary>
    /// <param name="line">The full line in the script containing the action and parameters</param>
    public void OnNewActionLine(string line)
    {
        //Split into action and parameter
        string[] actionAndParam = line.Substring(1, line.Length - 2).Split(ACTION_SIDE_SEPARATOR);

        if (actionAndParam.Length > 2)
        {
            Debug.LogError("Invalid action with line: " + line);
            _onActionDone.Invoke();
            return;
        }

        string action = actionAndParam[0];
        string parameters = (actionAndParam.Length == 2) ? actionAndParam[1] : "";

        switch (action)
        {
            //Actor controller
            case "ACTOR": SetActor(parameters); break;
            case "SET_ACTOR_POSITION": SetActorPosition(parameters); break;
            case "SHOWACTOR": SetActorVisibility(parameters); break;
            case "SPEAK": SetSpeaker(parameters, SpeakingType.Speaking); break;
            case "THINK": SetSpeaker(parameters, SpeakingType.Thinking); break;
            case "SET_POSE": SetPose(parameters); break;
            case "PLAY_EMOTION": PlayEmotion(parameters); break; //Emotion = animation on an actor. Saves PLAY_ANIMATION for other things
            //Audio controller
            case "PLAYSFX": PlaySFX(parameters); break;
            case "PLAYSONG": SetBGMusic(parameters); break;
            case "STOP_SONG": StopSong(); break;
            //Scene controller
            case "FADE_OUT": FadeOutScene(parameters); break;
            case "FADE_IN": FadeInScene(parameters); break;
            case "CAMERA_PAN": PanCamera(parameters); break;
            case "CAMERA_SET": SetCameraPosition(parameters); break;
            case "SHAKESCREEN": ShakeScreen(parameters); break;
            case "SCENE": SetScene(parameters); break;
            case "WAIT": Wait(parameters); break;
            case "SHOW_ITEM": ShowItem(parameters); break;
            case "HIDE_ITEM": HideItem(); break;
            case "PLAY_ANIMATION": PlayAnimation(parameters); break;
            case "JUMP_TO_POSITION": JumpToActorSlot(parameters); break;
            case "PAN_TO_POSITION": PanToActorSlot(parameters); break;
            //Evidence controller
            case "ADD_EVIDENCE": AddEvidence(parameters); break;
            case "REMOVE_EVIDENCE": RemoveEvidence(parameters); break;
            case "ADD_RECORD": AddToCourtRecord(parameters); break;
            case "PRESENT_EVIDENCE": OpenEvidenceMenu(); break;
            case "SUBSTITUTE_EVIDENCE": SubstituteEvidence(parameters); break;
            //Dialog controller
            case "DIALOG_SPEED": ChangeDialogSpeed(WaiterType.Dialog, parameters); break;
            case "OVERALL_SPEED": ChangeDialogSpeed(WaiterType.Overall, parameters); break;
            case "PUNCTUATION_SPEED": ChangeDialogSpeed(WaiterType.Punctuation, parameters); break;
            case "CLEAR_SPEED": ClearDialogSpeeds(); break;
            case "DISABLE_SKIPPING": DisableTextSkipping(parameters); break;
            case "AUTOSKIP": AutoSkip(parameters); break;
            case "CONTINUE_DIALOG": ContinueDialog(); break;
            case "APPEAR_INSTANTLY": AppearInstantly(); break;
            case "HIDE_TEXTBOX": HideTextbox(); break;
            //Do nothing
            case "WAIT_FOR_INPUT": break;
            //Default
            default: Debug.LogError("Unknown action: " + action); break;
        }
    }

    #region ActorController
    /// <summary>
    /// Sets the shown actor in the scene
    /// </summary>
    /// <param name="actor">Actor to be switched to</param>
    private void SetActor(string actor)
    {
        if (!HasActorController())
            return;

        _actorController.SetActiveActor(actor);
        _onActionDone.Invoke();
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
                _sceneController.ShowActor();
            }
            else
            {
                _sceneController.HideActor();
            }
        }
        else
        {
            Debug.LogError("Invalid paramater " + showActor + " for function SHOWACTOR");
        }
        _onActionDone.Invoke();
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

        _actorController.SetActiveSpeaker(actor);
        _actorController.SetSpeakingType(speakingType);
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Set the pose of the current actor
    /// </summary>
    /// <param name="poseAndActorName">"[pose name]" to set pose for current actor OR "[pose name],[actor name]" to set pose for another actor</param>
    private void SetPose(string poseAndActorName)
    {
        if (!HasActorController())
            return;

        string[] parameters = poseAndActorName.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length == 1)
        {
            _actorController.SetPose(parameters[0]);
            _onActionDone.Invoke();
        }
        else if (parameters.Length == 2)
        {
            _actorController.SetPose(parameters[0], parameters[1]);
            _onActionDone.Invoke();
        }
        else
        {
            Debug.LogError("Invalid number of parameters for function PLAY_EMOTION");
        }


    }

    /// <summary>
    /// Plays an emotion for the current actor. Emotion is a fancy term for animation on an actor.
    /// </summary>
    /// <param name="animationAndActorName">"[animation name]" to set pose for current actor OR "[animation name],[actor name]" to queue animation for another actor (gets played as soon as actor is visible)</param>
    private void PlayEmotion(string animationAndActorName)
    {
        if (!HasActorController())
            return;

        string[] parameters = animationAndActorName.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length == 1)
        {
            _actorController.PlayEmotion(parameters[0]);
        }
        else if (parameters.Length == 2)
        {
            _actorController.PlayEmotion(parameters[0], parameters[1]);
        }
        else
        {
            Debug.LogError("Invalid number of parameters for function PLAY_EMOTION");
        }
    }

    /// <summary>
    /// Sets an actor to a specific slot in the currently active scene.
    /// </summary>
    /// <param name="slotIndexAndActor">String containing the actor name first and one-based slot index second.</param>
    private void SetActorPosition(string slotIndexAndActor)
    {
        if (!HasActorController())
        {
            return;
        }

        string[] parameters = slotIndexAndActor.Split(ACTION_PARAMETER_SEPARATOR);

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

        _actorController.AssignActorToSlot(actorName, oneBasedSlotIndex);
        _onActionDone.Invoke();
    }
    #endregion

    #region SceneController
    /// <summary>
    /// Fades the scene in from black
    /// </summary>
    /// <param name="seconds">Amount of seconds the fade-in should take as a float</param>
    void FadeInScene(string seconds)
    {
        if (!HasSceneController())
            return;

        float timeInSeconds;

        if (float.TryParse(seconds, NumberStyles.Any, CultureInfo.InvariantCulture, out timeInSeconds))
        {
            _sceneController.FadeIn(timeInSeconds);
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
    void FadeOutScene(string seconds)
    {
        if (!HasSceneController())
            return;

        float timeInSeconds;

        if (float.TryParse(seconds, NumberStyles.Any, CultureInfo.InvariantCulture, out timeInSeconds))
        {
            _sceneController.FadeOut(timeInSeconds);
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
    void ShakeScreen(string intensity)
    {
        if (!HasSceneController())
            return;

        float intensityNumerical;
        float duration;
        bool shouldWaitForShake = false;
        
        string[] parameters = intensity.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length < 2)
        {
            Debug.LogError("Invalid number of parameters for function SHAKESCREEN");
            return;
        }
        
        if (!float.TryParse(parameters[0], NumberStyles.Any, CultureInfo.InvariantCulture, out intensityNumerical))
        {
            Debug.LogError("Invalid parameter " + parameters[0] + " for function SHAKESCREEN");
            return;
        }

        if (!float.TryParse(parameters[1], NumberStyles.Any, CultureInfo.InvariantCulture, out duration))
        {
            Debug.LogError("Invalid parameter " + parameters[1] + " for function SHAKESCREEN");
        }
        
        if (parameters.Length > 2 && !bool.TryParse(parameters[2], out shouldWaitForShake))
        {
            Debug.LogError("Invalid parameter " + parameters[2] + " for function SHAKESCREEN");
            return;
        }
        
        _sceneController.ShakeScreen(intensityNumerical, duration, shouldWaitForShake);
    }

    /// <summary>
    /// Sets the scene (background, character location on screen, any props (probably prefabs))
    /// </summary>
    /// <param name="sceneName">Scene to change to</param>
    void SetScene(string sceneName)
    {
        if (!HasSceneController())
            return;

        _sceneController.SetScene(sceneName);
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Sets the camera position
    /// </summary>
    /// <param name="position">New camera coordinates in the "int x,int y" format</param>
    void SetCameraPosition(string position)
    {
        if (!HasSceneController())
            return;

        string[] parameters = position.Split(ACTION_PARAMETER_SEPARATOR);

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
            _sceneController.SetCameraPos(new Vector2Int(x, y));
        }
        else
        {
            Debug.LogError("Invalid paramater " + position + " for function CAMERA_SET");
        }
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Pan the camera to a certain x,y position
    /// </summary>
    /// <param name="durationAndPosition">Duration the pan should take and the target coordinates in the "float seconds, int x, int y" format</param>
    void PanCamera(string durationAndPosition)
    {
        if (!HasSceneController())
            return;

        string[] parameters = durationAndPosition.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length != 3)
        {
            Debug.LogError("Invalid amount of parameters for function CAMERA_PAN");
            return;
        }

        float duration;
        int x;
        int y;

        if (float.TryParse(parameters[0], NumberStyles.Any, CultureInfo.InvariantCulture, out duration)
            && int.TryParse(parameters[1], out x)
            && int.TryParse(parameters[2], out y))
        {
            _sceneController.PanCamera(duration, new Vector2Int(x, y));
        }
        else
        {
            Debug.LogError("Invalid paramater " + durationAndPosition + " for function CAMERA_PAN");
        }
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Shows an item on the middle, left, or right side of the screen.
    /// </summary>
    /// <param name="ItemNameAndPosition">Which item to show and where to show it, in the "string item, itemPosition pos" format</param>
    void ShowItem(string ItemNameAndPosition)
    {
        if (!HasSceneController())
            return;

        string[] parameters = ItemNameAndPosition.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length != 2)
        {
            Debug.LogError("Invalid amount of parameters for function SHOW_ITEM");
            return;
        }

        itemDisplayPosition pos;
        if (System.Enum.TryParse<itemDisplayPosition>(parameters[1], out pos))
        {
            _sceneController.ShowItem(parameters[0], pos);
        }
        else
        {
            Debug.LogError("Invalid paramater " + parameters[1] + " for function CAMERA_PAN");
        }
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Hides the item displayed on the screen by ShowItem method.
    /// </summary>
    void HideItem()
    {
        if (!HasSceneController())
            return;

        _sceneController.HideItem();
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Waits seconds before automatically continuing.
    /// </summary>
    /// <param name="seconds">Amount of seconds to wait</param>
    void Wait(string seconds)
    {
        if (!HasSceneController())
            return;

        float secondsFloat;

        if (float.TryParse(seconds, NumberStyles.Any, CultureInfo.InvariantCulture, out secondsFloat))
        {
            _sceneController.Wait(secondsFloat);
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

        _sceneController.PlayAnimation(animationName);
    }

    /// Jump-cuts the camera to the target sub position if the bg-scene has sub positions.
    /// </summary>
    /// <param name="oneBasedSlotIndexAsString">String containing an integer referring to the target sub position, 1 based.</param>
    void JumpToActorSlot(string oneBasedSlotIndexAsString)
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

        _sceneController.JumpToActorSlot(oneBasedSlotIndex);
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Pans the camera to the target actor slot if the bg-scene has support for actor slots.
    /// </summary>
    /// <param name="oneBasedSlotIndexAndTimeInSeconds">String containing a one-based integer index referring to the target actor slot, and a floating point number referring to the amount of time the pan should take in seconds.</param>
    void PanToActorSlot(string oneBasedSlotIndexAndTimeInSeconds)
    {
        if (!HasSceneController())
        {
            return;
        }

        string[] parameters = oneBasedSlotIndexAndTimeInSeconds.Split(ACTION_PARAMETER_SEPARATOR);

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
        
        _sceneController.PanToActorSlot(oneBasedSlotIndex, timeInSeconds);
        _onActionDone.Invoke();
    }

    #endregion

    #region AudioController
    /// <summary>
    /// Plays a sound effect
    /// </summary>
    /// <param name="sfx">Name of the sound effect</param>
    void PlaySFX(string sfx)
    {
        if (!HasAudioController())
            return;

        _audioController.PlaySFX(sfx);
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Sets the background music
    /// </summary>
    /// <param name="songName">Name of the new song</param>
    void SetBGMusic(string songName)
    {
        if (!HasAudioController())
            return;

        _audioController.PlaySong(songName);
        _onActionDone.Invoke();
    }

    /// <summary>
    /// If music is currently playing, stop it!
    /// </summary>
    void StopSong()
    {
        if (!HasAudioController())
            return;

        _audioController.StopSong();
        _onActionDone.Invoke();
    }
    #endregion

    #region EvidenceController
    void AddEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        _evidenceController.AddEvidence(evidence);
        _onActionDone.Invoke();
    }

    void RemoveEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        _evidenceController.RemoveEvidence(evidence);
        _onActionDone.Invoke();
    }

    void AddToCourtRecord(string actor)
    {
        if (!HasEvidenceController())
            return;

        _evidenceController.AddToCourtRecord(actor);
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Calls the onPresentEvidence event on evidence controller which
    /// opens the evidence menu so evidence can be presented.
    /// </summary>
    void OpenEvidenceMenu()
    {
        if (!HasEvidenceController())
            return;

        _evidenceController.OpenEvidenceMenu();
    }

    /// <summary>
    /// Used to substitute a specified Evidence object with its assigned alternate Evidence object.
    /// </summary>
    /// <param name="evidence">The name of the evidence to substitute.</param>
    void SubstituteEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        _evidenceController.SubstituteEvidenceWithAlt(evidence);
        _onActionDone.Invoke();
    }

    #endregion

    #region ControllerStuff
    /// <summary>
    /// Attach a new IActorController to the decoder
    /// </summary>
    /// <param name="newController">New action controller to be added</param>
    public void SetActorController(IActorController newController)
    {
        _actorController = newController;
    }

    /// <summary>
    /// Checks if the decoder has an actor controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an actor controller is connected</returns>
    private bool HasActorController()
    {
        if (_actorController == null)
        {
            Debug.LogError("No actor controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Attach a new ISceneController to the decoder
    /// </summary>
    /// <param name="newController">New scene controller to be added</param>
    public void SetSceneController(ISceneController newController)
    {
        _sceneController = newController;
    }

    /// <summary>
    /// Checks if the decoder has a scene controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether a scene controller is connected</returns>
    private bool HasSceneController()
    {
        if (_sceneController == null)
        {
            Debug.LogError("No scene controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Attach a new IAudioController to the decoder
    /// </summary>
    /// <param name="newController">New audio controller to be added</param>
    public void SetAudioController(IAudioController newController)
    {
        _audioController = newController;
    }

    /// <summary>
    /// Checks if the decoder has an audio controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an audio controller is connected</returns>
    private bool HasAudioController()
    {
        if (_audioController == null)
        {
            Debug.LogError("No audio controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Attach a new IEvidenceController to the decoder
    /// </summary>
    /// <param name="newController">New evidence controller to be added</param>
    public void SetEvidenceController(IEvidenceController newController)
    {
        _evidenceController = newController;
    }

    /// <summary>
    /// Checks if the decoder has an evidence controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an evidence controller is connected</returns>
    private bool HasEvidenceController()
    {
        if (_evidenceController == null)
        {
            Debug.LogError("No evidence controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Attach a new IAppearingDialogController to the decoder
    /// </summary>
    /// <param name="newController">New appearing dialog controller to be added</param>
    public void SetAppearingDialogController(IAppearingDialogueController newController)
    {
        _appearingDialogController = newController;
    }

    /// <summary>
    /// Checks if the decoder has an appearing dialog controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an appearing dialog controller is connected</returns>
    private bool HasAppearingDialogController()
    {
        if (_appearingDialogController == null)
        {
            Debug.LogError("No appearing dialog controller attached to the action decoder", gameObject);
            return false;
        }
        return true;
    }
    #endregion

    #region DialogStuff

    ///<summary>
    ///Changes the dialog speed in appearingDialogController if it has beben set.
    ///</summary>
    ///<param name = "currentWaiterType">The current waiters type which appear time should be changed.</param>
    ///<param name = "parameters">Contains all the parameters needed to change the appearing time.</param>
    private void ChangeDialogSpeed(WaiterType currentWaiterType, string parameters)
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.SetTimerValue(currentWaiterType, parameters);
    }

    ///<summary>
    ///Clears all custom set dialog speeds
    ///</summary>
    private void ClearDialogSpeeds()
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.ClearAllWaiters();
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

        _appearingDialogController.ToggleDisableTextSkipping(value);
    }

    ///<summary>
    ///Makes the new dialog appear after current one.
    ///</summary>
    private void ContinueDialog()
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.ContinueDialog();
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

        _appearingDialogController.AutoSkipDialog(value);
    }

    /// <summary>
    /// Makes the next line of dialogue appear instantly instead of one character at a time.
    /// </summary>
    private void AppearInstantly()
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.PrintTextInstantly = true;
        _onActionDone.Invoke();
    }

    /// <summary>
    /// Hides the dialogue textbox.
    /// </summary>
    private void HideTextbox()
    {
        if (!HasAppearingDialogController())
            return;

        _appearingDialogController.HideTextbox();
        _onActionDone.Invoke();
    }
    #endregion
}