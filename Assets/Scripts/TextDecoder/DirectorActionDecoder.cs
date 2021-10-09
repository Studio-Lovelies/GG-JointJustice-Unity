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
            case "FADE_OUT": _decoder.FadeOutScene(parameters); break;
            case "FADE_IN": _decoder.FadeInScene(parameters); break;
            case "CAMERA_PAN": _decoder.PanCamera(parameters); break;
            case "CAMERA_SET": _decoder.SetCameraPosition(parameters); break;
            case "SHAKESCREEN": _decoder.ShakeScreen(parameters); break;
            case "SCENE": _decoder.SetScene(parameters); break;
            case "WAIT": _decoder.Wait(parameters); break;
            case "SHOW_ITEM": _decoder.ShowItem(parameters); break;
            case "HIDE_ITEM": _decoder.HideItem(); break;
            case "PLAY_ANIMATION": _decoder.PlayAnimation(parameters); break;
            case "JUMP_TO_POSITION": _decoder.JumpToActorSlot(parameters); break;
            case "PAN_TO_POSITION": _decoder.PanToActorSlot(parameters); break;
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
        if (!_decoder.HasSceneController())
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
    #endregion
}