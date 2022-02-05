using UnityEngine;

public class NarrativeScriptPlayer : MonoBehaviour
{
    public StoryPlayer StoryPlayer { private get; set; }
    public NarrativeScript ActiveNarrativeScript => StoryPlayer.ActiveNarrativeScript;

    public bool CanPresentEvidence => StoryPlayer.IsAtChoice && StoryPlayer.GameMode == GameMode.CrossExamination;
    
    private void Awake()
    {
        GetComponent<Game>().NarrativeScriptPlayer = this;
    }
    
    public void Continue()
    {
        StoryPlayer.Continue();
    }

    public void HandleChoice(int choiceIndex)
    {
        StoryPlayer.HandleChoice(choiceIndex);
    }

    public void PressWitness()
    {
        StoryPlayer.HandleChoice(1);
    }

    public void PresentEvidence(ICourtRecordObject courtRecordObject)
    {
        StoryPlayer.PresentEvidence(courtRecordObject);
    }
}
