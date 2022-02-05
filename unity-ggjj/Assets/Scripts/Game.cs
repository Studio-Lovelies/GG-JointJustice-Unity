using UnityEngine;

public class Game : MonoBehaviour
{
    public NarrativeScriptPlayer NarrativeScriptPlayer { get; private set; }
    public IDialogueController DialogueController { get; set; }
    public AppearingDialogueController AppearingDialogueController { get; set; }
    public DirectorActionDecoder DirectorActionDecoder { get; set; }
    public NarrativeScriptPlaylist NarrativeScriptPlaylist { get; set; }
    public BGSceneList BGSceneList { get; set; }

    private void Start()
    {
        NarrativeScriptPlaylist.InitializeNarrativeScripts();
        BGSceneList.InstantiateBGSceneFromPlaylist(NarrativeScriptPlaylist);
        
        NarrativeScriptPlayer = new NarrativeScriptPlayer(AppearingDialogueController, DirectorActionDecoder)
        {
            ActiveNarrativeScript = NarrativeScriptPlaylist.GetNextNarrativeScript()
        };
        NarrativeScriptPlayer.Continue();
    }
}
