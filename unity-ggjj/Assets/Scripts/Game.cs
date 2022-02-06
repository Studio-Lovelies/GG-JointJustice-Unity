using UnityEngine;

public class Game : MonoBehaviour
{
    public NarrativeScriptPlayer NarrativeScriptPlayer { get; set; }
    public AppearingDialogueController AppearingDialogueController { get; set; }
    public DirectorActionDecoder DirectorActionDecoder { get; set; }
    public NarrativeScriptPlaylist NarrativeScriptPlaylist { get; set; }
    public BGSceneList BGSceneList { get; set; }
    public ChoiceMenu ChoiceMenu { get; set; }
    public SceneController SceneController { get; set; }
    
    private void Start()
    {
        NarrativeScriptPlaylist.InitializeNarrativeScripts();
        BGSceneList.InstantiateBGSceneFromPlaylist(NarrativeScriptPlaylist);
        DirectorActionDecoder.Decoder.NarrativeScriptPlayer = NarrativeScriptPlayer;
        
        NarrativeScriptPlayer.StoryPlayer = new StoryPlayer(NarrativeScriptPlaylist, AppearingDialogueController, DirectorActionDecoder, ChoiceMenu)
        {
            ActiveNarrativeScript = NarrativeScriptPlaylist.GetNextNarrativeScript()
        };
        NarrativeScriptPlayer.Continue();
    }
}
