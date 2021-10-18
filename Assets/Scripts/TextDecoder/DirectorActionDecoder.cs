using UnityEngine;
using UnityEngine.Events;

public class DirectorActionDecoder : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    public ActionDecoder Decoder { get; } = new ActionDecoder();

    private void Awake()
    {
        Debug.Log(Decoder.WriteDocsForAllCommands());
        // We wrap this in an Action so we have no ties to UnityEngine in the ActionDecoder
        Decoder.OnActionDone += () => _onActionDone.Invoke();
    }

    #region API
    /// <summary>
    /// Called whenever a new action is executed (encountered and then forwarded here) in the script
    /// </summary>
    /// <param name="line">The full line in the script containing the action and parameters</param>
    public void OnNewActionLine(string line)
    {
        var actionLine = new ActionLine(line);
        try
        {
            Decoder.OnNewActionLine(actionLine);
        }
        catch (UnknownCommandException e)
        {
            Debug.Log($"Unknown action: {e.CommandName}");
        }
        catch (InvalidSyntaxException e)
        {
            Debug.LogError($"Invalid syntax: {line}");
            _onActionDone.Invoke();
        }
        catch (UnableToParseException e)
        {
            Debug.LogError($"Invalid parameters for {actionLine.Action}\n{line}\n{e.Message}");
            _onActionDone.Invoke();
        }
        catch (NotEnoughParametersException e)
        {
            Debug.LogError($"Not enough parameters for {actionLine.Action}\n{line}\n{e.Message}");
            _onActionDone.Invoke();
        }
    }

    /// <summary>
    /// Attach a new IActorController to the decoder
    /// </summary>
    /// <param name="newController">New action controller to be added</param>
    public void SetActorController(IActorController newController)
    {
        Decoder.ActorController = newController;
    }

    /// <summary>
    /// Attach a new ISceneController to the decoder
    /// </summary>
    /// <param name="newController">New scene controller to be added</param>
    public void SetSceneController(ISceneController newController)
    {
        Decoder.SceneController = newController;
    }

    /// <summary>
    /// Attach a new IAudioController to the decoder
    /// </summary>
    /// <param name="newController">New audio controller to be added</param>
    public void SetAudioController(IAudioController newController)
    {
        Decoder.AudioController = newController;
    }

    /// <summary>
    /// Attach a new IEvidenceController to the decoder
    /// </summary>
    /// <param name="newController">New evidence controller to be added</param>
    public void SetEvidenceController(IEvidenceController newController)
    {

    }

    /// <summary>
    /// Attach a new IAppearingDialogController to the decoder
    /// </summary>
    /// <param name="newController">New appearing dialog controller to be added</param>
    public void SetAppearingDialogController(IAppearingDialogueController newController)
    {
        Decoder.AppearingDialogueController = newController;
    }

    #endregion
}