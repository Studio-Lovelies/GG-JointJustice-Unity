using UnityEngine;

public class NarrativeScriptPlayerComponent : MonoBehaviour, INarrativeScriptPlayer
{
    public NarrativeScriptPlayer NarrativeScriptPlayer { private get; set; }
    public INarrativeScript ActiveNarrativeScript => NarrativeScriptPlayer.ActiveNarrativeScript;

    public GameMode GameMode
    {
        get => NarrativeScriptPlayer.GameMode;
        set => NarrativeScriptPlayer.GameMode = value;
    }

    public bool Waiting
    {
        get => NarrativeScriptPlayer.Waiting;
        set => NarrativeScriptPlayer.Waiting = value;
    }
    public bool CanPressWitness => NarrativeScriptPlayer.CanPressWitness && !Waiting;
    public bool HasSubStory => NarrativeScriptPlayer.HasSubStory;

    /// <summary>
    /// Exposes StoryPlayer's Continue method
    /// </summary>
    public void Continue()
    {
        NarrativeScriptPlayer.Continue();
    }

    /// <summary>
    /// Exposes StoryPlayer's HandleChoice
    /// </summary>
    public void HandleChoice(int choiceIndex)
    {
        NarrativeScriptPlayer.HandleChoice(choiceIndex);
    }

    /// <summary>
    /// Checks if a witness can be pressed then chooses the correct choice on StoryPlayer
    /// </summary>
    public void TryPressWitness()
    {
        if (!CanPressWitness)
        {
            return;
        }
        
        NarrativeScriptPlayer.HandleChoice(1);
    }

    /// <summary>
    /// Exposes StoryPlayer's PresentEvidence method
    /// </summary>
    public void PresentEvidence(ICourtRecordObject courtRecordObject)
    {
        NarrativeScriptPlayer.PresentEvidence(courtRecordObject);
    }

    /// <summary>
    /// Exposes StoryPlayer's StartSubStory method
    /// </summary>
    public void StartSubStory(NarrativeScript narrativeScript)
    {
        NarrativeScriptPlayer.StartSubStory(narrativeScript);
    }
}
