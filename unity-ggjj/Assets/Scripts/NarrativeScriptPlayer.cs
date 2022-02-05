using UnityEngine;

public class NarrativeScriptPlayer : MonoBehaviour
{
    public StoryPlayer StoryPlayer { private get; set; }
    public NarrativeScript ActiveNarrativeScript => StoryPlayer.ActiveNarrativeScript;

    public bool Waiting
    {
        private get => StoryPlayer.Waiting;
        set => StoryPlayer.Waiting = value;
    }
    public bool CanPressWitness => StoryPlayer.CanPressWitness && !Waiting;
    public bool HasSubStory => StoryPlayer.HasSubStory;

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
        if (CanPressWitness)
        {
            StoryPlayer.HandleChoice(1);
        }
    }

    public void PresentEvidence(ICourtRecordObject courtRecordObject)
    {
        StoryPlayer.PresentEvidence(courtRecordObject);
    }
}
