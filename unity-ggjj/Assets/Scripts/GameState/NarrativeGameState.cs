using UnityEngine;

public class NarrativeGameState : MonoBehaviour, INarrativeGameState
{
    [SerializeField] private NarrativeScriptPlayerComponent _narrativeScriptPlayerComponent;
    [SerializeField] private NarrativeScriptPlaylist _narrativeScriptPlaylist;
    [SerializeField] private ActionDecoderComponent _actionDecoderComponent;
    [SerializeField] private ActorController _actorController;
    [SerializeField] private AudioController _audioController;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private EvidenceController _evidenceController;
    [SerializeField] private AppearingDialogueController _appearingDialogueController;
    [SerializeField] private PenaltyManager _penaltyManager;
    [SerializeField] private BGSceneList _bgSceneList;
    [SerializeField] private ChoiceMenu _choiceMenu;

    public IActorController ActorController => _actorController;
    public IAppearingDialogueController AppearingDialogueController => _appearingDialogueController;
    public IObjectStorage ObjectStorage => _narrativeScriptPlayerComponent.ActiveNarrativeScript.ObjectStorage;
    public INarrativeScriptPlayer NarrativeScriptPlayer => _narrativeScriptPlayerComponent;
    public IAudioController AudioController => _audioController;
    public IEvidenceController EvidenceController => _evidenceController;
    public ISceneController SceneController => _sceneController;
    public IPenaltyManager PenaltyManager => _penaltyManager;
    public IActionDecoder ActionDecoder => _actionDecoderComponent.Decoder;
    public INarrativeScriptPlaylist NarrativeScriptPlaylist => _narrativeScriptPlaylist;
    public IChoiceMenu ChoiceMenu => _choiceMenu;

    private void Start()
    {
        _narrativeScriptPlaylist.InitializeNarrativeScripts();
        _bgSceneList.InstantiateBGSceneFromPlaylist(_narrativeScriptPlaylist);
        _actionDecoderComponent.Decoder.NarrativeGameState = this;
        _narrativeScriptPlayerComponent.Continue();
    }
}
