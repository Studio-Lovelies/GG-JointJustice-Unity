using UnityEngine;
using UnityEngine.Events;

public class DirectorActionDecoder : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    private readonly ActionDecoder _decoder = new ActionDecoder();

    private void Awake()
    {
        _decoder.OnActionDone = _onActionDone;
        _decoder.GameObject = gameObject;
    }

    #region API
    /// <summary>
    /// Called whenever a new action is executed (encountered and then forwarded here) in the script
    /// </summary>
    /// <param name="line">The full line in the script containing the action and parameters</param>
    public void OnNewActionLine(string line)
    {
        try
        {
            _decoder.OnNewActionLine(line);
        }
        catch (UnknownCommandException e)
        {
            Debug.Log($"Unknown action: {e.CommandName}");
        }
    }

    /// <summary>
    /// Attach a new IActorController to the decoder
    /// </summary>
    /// <param name="newController">New action controller to be added</param>
    public void SetActorController(IActorController newController)
    {
        _decoder.ActorController = newController;
    }

    /// <summary>
    /// Attach a new ISceneController to the decoder
    /// </summary>
    /// <param name="newController">New scene controller to be added</param>
    public void SetSceneController(ISceneController newController)
    {
        _decoder.SceneController = newController;
    }

    /// <summary>
    /// Attach a new IAudioController to the decoder
    /// </summary>
    /// <param name="newController">New audio controller to be added</param>
    public void SetAudioController(IAudioController newController)
    {
        _decoder.AudioController = newController;
    }

    /// <summary>
    /// Attach a new IEvidenceController to the decoder
    /// </summary>
    /// <param name="newController">New evidence controller to be added</param>
    public void SetEvidenceController(IEvidenceController newController)
    {
        _decoder.EvidenceController = newController;
    }

    /// <summary>
    /// Attach a new IAppearingDialogController to the decoder
    /// </summary>
    /// <param name="newController">New appearing dialog controller to be added</param>
    public void SetAppearingDialogController(IAppearingDialogueController newController)
    {
        _decoder.AppearingDialogueController = newController;
    }

    #endregion
}