using UnityEngine;

public class Game : MonoBehaviour
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

    public IObjectStorage ObjectStorage => _narrativeScriptPlayer.ActiveNarrativeScript.ObjectStorage;
    public INarrativeScriptPlayer NarrativeScriptPlayer => _narrativeScriptPlayer;
    public IAudioController Audio => _audioController;
    
    private void Start()
    {
        _narrativeScriptPlaylist.InitializeNarrativeScripts();
        _bgSceneList.InstantiateBGSceneFromPlaylist(_narrativeScriptPlaylist);
        _directorActionDecoder.Decoder.NarrativeScriptPlayer = _narrativeScriptPlayer;

        _directorActionDecoder.Decoder.ActorController = _actorController;
        _directorActionDecoder.Decoder.AudioController = _audioController;
        _directorActionDecoder.Decoder.EvidenceController = _evidenceController;
        _directorActionDecoder.Decoder.SceneController = _sceneController;
        _directorActionDecoder.Decoder.PenaltyManager = _penaltyManager;
        _directorActionDecoder.Decoder.AppearingDialogueController = _appearingDialogueController;
        _directorActionDecoder.Decoder.NarrativeScriptPlayer = _narrativeScriptPlayer;
        
        _narrativeScriptPlayer.StoryPlayer = new StoryPlayer(_narrativeScriptPlaylist, _appearingDialogueController, _directorActionDecoder.Decoder, _choiceMenu)
        {
            ActiveNarrativeScript = _narrativeScriptPlaylist.GetNextNarrativeScript()
        };
        _narrativeScriptPlayer.Continue();
    }
}
