using UnityEngine;

public class NarrativeScriptPlayer : MonoBehaviour, INarrativeScriptPlayer
{
    public StoryPlayer StoryPlayer { private get; set; }
    public INarrativeScript ActiveNarrativeScript => StoryPlayer.ActiveNarrativeScript;

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

    /// <summary>
    /// Exposes StoryPlayer's Continue method
    /// </summary>
    public void Continue()
    {
        StoryPlayer.Continue();
    }

    /// <summary>
    /// Exposes StoryPlayer's HandleChoice
    /// </summary>
    public void HandleChoice(int choiceIndex)
    {
        StoryPlayer.HandleChoice(choiceIndex);
    }

    /// <summary>
    /// Checks if a witness can be pressed then chooses the correct choice on StoryPlayer
    /// </summary>
    public void TryPressWitness()
    {
        if (CanPressWitness)
        {
            return;
        }
        
        StoryPlayer.HandleChoice(1);
    }

    /// <summary>
    /// Exposes StoryPlayer's PresentEvidence method
    /// </summary>
    public void PresentEvidence(ICourtRecordObject courtRecordObject)
    {
        StoryPlayer.PresentEvidence(courtRecordObject);
    }

    /// <summary>
    /// Exposes StoryPlayer's StartSubStory method
    /// </summary>
    public void StartSubStory(NarrativeScript narrativeScript)
    {
        StoryPlayer.StartSubStory(narrativeScript);
    }
}
