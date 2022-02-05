using UnityEngine;

public class Game : MonoBehaviour
{
    public NarrativeScriptPlayer NarrativeScriptPlayer { get; set; }
    public IDialogueController DialogueController { get; set; }
    public AppearingDialogueController AppearingDialogueController { get; set; }
    public DirectorActionDecoder DirectorActionDecoder { get; set; }
    public NarrativeScriptPlaylist NarrativeScriptPlaylist { get; set; }
    public BGSceneList BGSceneList { get; set; }
    public ChoiceMenu ChoiceMenu { get; set; }

    private void Start()
    {
        NarrativeScriptPlaylist.InitializeNarrativeScripts();
        BGSceneList.InstantiateBGSceneFromPlaylist(NarrativeScriptPlaylist);
        
        NarrativeScriptPlayer.StoryPlayer = new StoryPlayer(NarrativeScriptPlaylist, AppearingDialogueController, DirectorActionDecoder, ChoiceMenu)
        {
            ActiveNarrativeScript = NarrativeScriptPlaylist.GetNextNarrativeScript()
        };
        NarrativeScriptPlayer.Continue();
    }
}
