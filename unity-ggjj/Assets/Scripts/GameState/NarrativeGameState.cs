using UnityEngine;

public class NarrativeGameState : MonoBehaviour, INarrativeGameState
{
    [SerializeField] private NarrativeScriptPlayer _narrativeScriptPlayer;
    [SerializeField] private NarrativeScriptPlaylist _narrativeScriptPlaylist;
    [SerializeField] private DirectorActionDecoder _directorActionDecoder;
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
    public IObjectStorage ObjectStorage => _narrativeScriptPlayer.ActiveNarrativeScript.ObjectStorage;
    public INarrativeScriptPlayer NarrativeScriptPlayer => _narrativeScriptPlayer;
    public IAudioController AudioController => _audioController;
    public IEvidenceController EvidenceController => _evidenceController;
    public ISceneController SceneController => _sceneController;
    public IPenaltyManager PenaltyManager => _penaltyManager;

    private void Start()
    {
        _narrativeScriptPlaylist.InitializeNarrativeScripts();
        _bgSceneList.InstantiateBGSceneFromPlaylist(_narrativeScriptPlaylist);
        _directorActionDecoder.Decoder.NarrativeGameState = this;
        
        _narrativeScriptPlayer.StoryPlayer = new StoryPlayer(_narrativeScriptPlaylist, _appearingDialogueController, _directorActionDecoder.Decoder, _choiceMenu)
        {
            ActiveNarrativeScript = _narrativeScriptPlaylist.GetNextNarrativeScript()
        };
        _narrativeScriptPlayer.Continue();
    }
}
