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

    public ActionDecoder()
    {
    }
}
