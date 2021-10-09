using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class DirectorActionDecoder : MonoBehaviour
{
    private const char ACTION_SIDE_SEPARATOR = ':';
    private const char ACTION_PARAMETER_SEPARATOR = ',';

    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    private readonly ActionDecoder _decoder = new ActionDecoder();

    private void Awake()
    {
        _decoder._onActionDone = _onActionDone;
        _decoder._gameObject = gameObject;
    }

    #region API
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
            _decoder._onActionDone.Invoke();
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
            case "PLAYSFX": _decoder.PlaySFX(parameters); break;
            case "PLAYSONG": _decoder.SetBGMusic(parameters); break;
            case "STOP_SONG": _decoder.StopSong(); break;
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
            case "ADD_EVIDENCE": _decoder.AddEvidence(parameters); break;
            case "REMOVE_EVIDENCE": _decoder.RemoveEvidence(parameters); break;
            case "ADD_RECORD": _decoder.AddToCourtRecord(parameters); break;
            case "PRESENT_EVIDENCE": _decoder.OpenEvidenceMenu(); break;
            case "SUBSTITUTE_EVIDENCE": _decoder.SubstituteEvidence(parameters); break;
            //Dialog controller
            case "DIALOG_SPEED": _decoder.ChangeDialogSpeed(WaiterType.Dialog, parameters); break;
            case "OVERALL_SPEED": _decoder.ChangeDialogSpeed(WaiterType.Overall, parameters); break;
            case "PUNCTUATION_SPEED": _decoder.ChangeDialogSpeed(WaiterType.Punctuation, parameters); break;
            case "CLEAR_SPEED": _decoder.ClearDialogSpeeds(); break;
            case "DISABLE_SKIPPING": _decoder.DisableTextSkipping(parameters); break;
            case "AUTOSKIP": _decoder.AutoSkip(parameters); break;
            case "CONTINUE_DIALOG": _decoder.ContinueDialog(); break;
            case "APPEAR_INSTANTLY": _decoder.AppearInstantly(); break;
            case "HIDE_TEXTBOX": _decoder.HideTextbox(); break;
            //Do nothing
            case "WAIT_FOR_INPUT": break;
            //Default
            default: Debug.LogError("Unknown action: " + action); break;
        }
    }

    /// <summary>
    /// Attach a new IActorController to the decoder
    /// </summary>
    /// <param name="newController">New action controller to be added</param>
    public void SetActorController(IActorController newController)
    {
        _decoder._actorController = newController;
    }

    /// <summary>
    /// Attach a new ISceneController to the decoder
    /// </summary>
    /// <param name="newController">New scene controller to be added</param>
    public void SetSceneController(ISceneController newController)
    {
        _decoder._sceneController = newController;
    }

    /// <summary>
    /// Attach a new IAudioController to the decoder
    /// </summary>
    /// <param name="newController">New audio controller to be added</param>
    public void SetAudioController(IAudioController newController)
    {
        _decoder._audioController = newController;
    }

    /// <summary>
    /// Attach a new IEvidenceController to the decoder
    /// </summary>
    /// <param name="newController">New evidence controller to be added</param>
    public void SetEvidenceController(IEvidenceController newController)
    {
        _decoder._evidenceController = newController;
    }

    /// <summary>
    /// Attach a new IAppearingDialogController to the decoder
    /// </summary>
    /// <param name="newController">New appearing dialog controller to be added</param>
    public void SetAppearingDialogController(IAppearingDialogueController newController)
    {
        _decoder._appearingDialogController = newController;
    }

    #endregion

    #region ActorController
    /// <summary>
    /// Sets the shown actor in the scene
    /// </summary>
    /// <param name="actor">Actor to be switched to</param>
    private void SetActor(string actor)
    {
        if (!HasActorController())
            return;

        _decoder._actorController.SetActiveActor(actor);
        _decoder._onActionDone.Invoke();
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
                _decoder._sceneController.ShowActor();
            }
            else
            {
                _decoder._sceneController.HideActor();
            }
        }
        else
        {
            Debug.LogError("Invalid paramater " + showActor + " for function SHOWACTOR");
        }
        _decoder._onActionDone.Invoke();
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

        _decoder._actorController.SetActiveSpeaker(actor);
        _decoder._actorController.SetSpeakingType(speakingType);
        _decoder._onActionDone.Invoke();
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
            _decoder._actorController.SetPose(parameters[0]);
            _decoder._onActionDone.Invoke();
        }
        else if (parameters.Length == 2)
        {
            _decoder._actorController.SetPose(parameters[0], parameters[1]);
            _decoder._onActionDone.Invoke();
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
            _decoder._actorController.PlayEmotion(parameters[0]);
        }
        else if (parameters.Length == 2)
        {
            _decoder._actorController.PlayEmotion(parameters[0], parameters[1]);
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

        _decoder._actorController.AssignActorToSlot(actorName, oneBasedSlotIndex);
        _decoder._onActionDone.Invoke();
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
            _decoder._sceneController.FadeIn(timeInSeconds);
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
            _decoder._sceneController.FadeOut(timeInSeconds);
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

        string[] parameters = intensity.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length < 2)
        {
            Debug.LogError("Invalid number of parameters for function SHAKESCREEN");
            return;
        }
        
        if (!float.TryParse(parameters[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float intensityNumerical))
        {
            Debug.LogError($"Invalid parameter {parameters[0]} for function SHAKESCREEN");
            return;
        }

        if (!float.TryParse(parameters[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float duration))
        {
            Debug.LogError($"Invalid parameter {parameters[1]} for function SHAKESCREEN");
        }

        bool isBlocking = false;
        if (parameters.Length > 2 && !bool.TryParse(parameters[2], out isBlocking))
        {
            Debug.LogError($"Invalid parameter {parameters[2]} for function SHAKESCREEN");
            return;
        }
        
        _sceneController.ShakeScreen(intensityNumerical, duration, isBlocking);
    }

    /// <summary>
    /// Sets the scene (background, character location on screen, any props (probably prefabs))
    /// </summary>
    /// <param name="sceneName">Scene to change to</param>
    void SetScene(string sceneName)
    {
        if (!HasSceneController())
            return;

        _decoder._sceneController.SetScene(sceneName);
        _decoder._onActionDone.Invoke();
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
            _decoder._sceneController.SetCameraPos(new Vector2Int(x, y));
        }
        else
        {
            Debug.LogError("Invalid paramater " + position + " for function CAMERA_SET");
        }
        _decoder._onActionDone.Invoke();
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
            _decoder._sceneController.PanCamera(duration, new Vector2Int(x, y));
        }
        else
        {
            Debug.LogError("Invalid paramater " + durationAndPosition + " for function CAMERA_PAN");
        }
        _decoder._onActionDone.Invoke();
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
            _decoder._sceneController.ShowItem(parameters[0], pos);
        }
        else
        {
            Debug.LogError("Invalid paramater " + parameters[1] + " for function CAMERA_PAN");
        }
        _decoder._onActionDone.Invoke();
    }

    /// <summary>
    /// Hides the item displayed on the screen by ShowItem method.
    /// </summary>
    void HideItem()
    {
        if (!HasSceneController())
            return;

        _decoder._sceneController.HideItem();
        _decoder._onActionDone.Invoke();
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
            _decoder._sceneController.Wait(secondsFloat);
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

        _decoder._sceneController.PlayAnimation(animationName);
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

        _decoder._sceneController.JumpToActorSlot(oneBasedSlotIndex);
        _decoder._onActionDone.Invoke();
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

        _decoder._sceneController.PanToActorSlot(oneBasedSlotIndex, timeInSeconds);
        _decoder._onActionDone.Invoke();
    }

    #endregion

    #region ControllerStuff

    /// <summary>
    /// Checks if the decoder has an actor controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an actor controller is connected</returns>
    private bool HasActorController()
    {
        if (_decoder._actorController == null)
        {
            Debug.LogError("No actor controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if the decoder has a scene controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether a scene controller is connected</returns>
    private bool HasSceneController()
    {
        if (_decoder._sceneController == null)
        {
            Debug.LogError("No scene controller attached to the action decoder");
            return false;
        }
        return true;
    }
    #endregion
}