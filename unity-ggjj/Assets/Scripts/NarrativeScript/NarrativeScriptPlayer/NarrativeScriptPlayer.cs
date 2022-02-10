using UnityEngine;

public class NarrativeScriptPlayer : MonoBehaviour, INarrativeScriptPlayer
{
    public StoryPlayer StoryPlayer { private get; set; }
    public NarrativeScript ActiveNarrativeScript => StoryPlayer.ActiveNarrativeScript;

    public GameMode GameMode
    {
        get => StoryPlayer.GameMode;
        set => StoryPlayer.GameMode = value;
    }

    public bool Waiting
    {
        get => StoryPlayer.Waiting;
        set => StoryPlayer.Waiting = value;
    }
    public bool CanPressWitness => StoryPlayer.CanPressWitness && !Waiting;
    public bool HasSubStory => StoryPlayer.HasSubStory;

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

    public void StartSubStory(NarrativeScript narrativeScript)
    {
        StoryPlayer.StartSubStory(narrativeScript);
    }
}
