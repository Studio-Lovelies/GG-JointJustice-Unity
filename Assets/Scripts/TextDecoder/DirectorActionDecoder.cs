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
        _decoder.OnNewActionLine(line);
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
}