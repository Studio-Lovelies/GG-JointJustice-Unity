using UnityEngine;

public class Game : MonoBehaviour
{
    public IDialogueController DialogueController { get; set; }
    public NarrativeScriptPlaylist NarrativeScriptPlaylist { get; set; }
    public BGSceneList BGSceneList { get; set; }

    private void Start()
    {
        NarrativeScriptPlaylist.InitializeNarrativeScripts();
        BGSceneList.InstantiateBGSceneFromPlaylist(NarrativeScriptPlaylist);
        DialogueController.SetNewDialogue(NarrativeScriptPlaylist.GetNextNarrativeScript());
    }
}
