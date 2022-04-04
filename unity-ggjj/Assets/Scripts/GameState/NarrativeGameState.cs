using UnityEngine;

public class NarrativeGameState : MonoBehaviour, INarrativeGameState
{
    [SerializeField] private NarrativeScriptPlayerComponent _narrativeScriptPlayerComponent;
    [SerializeField] private ActionDecoderComponent _actionDecoderComponent;
    [SerializeField] private ActorController _actorController;
    [SerializeField] private AudioController _audioController;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private EvidenceController _evidenceController;
    [SerializeField] private AppearingDialogueController _appearingDialogueController;
    [SerializeField] private PenaltyManager _penaltyManager;
    [SerializeField] private BGSceneList _bgSceneList;
    [SerializeField] private ChoiceMenu _choiceMenu;
    [SerializeField] private SceneLoader _sceneLoader;

    private NarrativeScriptStorage _narrativeScriptStorage;
    
    public IActorController ActorController => _actorController;
    public IAppearingDialogueController AppearingDialogueController => _appearingDialogueController;
    public IObjectStorage ObjectStorage => _narrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript.ObjectStorage;
    public INarrativeScriptPlayerComponent NarrativeScriptPlayerComponent => _narrativeScriptPlayerComponent;
    public IAudioController AudioController => _audioController;
    public IEvidenceController EvidenceController => _evidenceController;
    public ISceneController SceneController => _sceneController;
    public IPenaltyManager PenaltyManager => _penaltyManager;
    public IActionDecoder ActionDecoder => _actionDecoderComponent.Decoder;
    public INarrativeScriptStorage NarrativeScriptStorage => _narrativeScriptStorage;
    public IChoiceMenu ChoiceMenu => _choiceMenu;
    public IBGSceneList BGSceneList => _bgSceneList;
    public SceneLoader SceneLoader => _sceneLoader;

    private void Awake()
    {
        _narrativeScriptStorage = new NarrativeScriptStorage(this);
    }

    /// <summary>
    /// Starts the game, calling the required methods in order
    /// </summary>
    public void StartGame()
    {
        BGSceneList.InstantiateBGScenes(_narrativeScriptStorage.NarrativeScript);
        _actionDecoderComponent.Decoder.NarrativeGameState = this;
        _narrativeScriptPlayerComponent.NarrativeScriptPlayer.Continue();
    }
}
