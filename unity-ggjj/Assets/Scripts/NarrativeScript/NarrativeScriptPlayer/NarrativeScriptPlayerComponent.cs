using UnityEngine;

public class NarrativeScriptPlayerComponent : MonoBehaviour, INarrativeScriptPlayer
{
    [SerializeField] private NarrativeGameState _narrativeGameState;
    
    private NarrativeScriptPlayer _narrativeScriptPlayer;
    
    public INarrativeScript ActiveNarrativeScript => _narrativeScriptPlayer.ActiveNarrativeScript;

    public GameMode GameMode
    {
        get => _narrativeScriptPlayer.GameMode;
        set => _narrativeScriptPlayer.GameMode = value;
    }

    public bool Waiting
    {
        get => _narrativeScriptPlayer.Waiting;
        set => _narrativeScriptPlayer.Waiting = value;
    }
    public bool CanPressWitness => _narrativeScriptPlayer.CanPressWitness && !Waiting;
    public bool HasSubStory => _narrativeScriptPlayer.HasSubStory;

    private void Awake()
    {
        _narrativeScriptPlayer = new NarrativeScriptPlayer(_narrativeGameState)
        {
            ActiveNarrativeScript = _narrativeGameState.NarrativeScriptPlaylist.GetNextNarrativeScript()
        };
    }
    
    /// <summary>
    /// Exposes StoryPlayer's Continue method
    /// </summary>
    public void Continue()
    {
        _narrativeScriptPlayer.Continue();
    }

    /// <summary>
    /// Exposes StoryPlayer's HandleChoice
    /// </summary>
    public void HandleChoice(int choiceIndex)
    {
        _narrativeScriptPlayer.HandleChoice(choiceIndex);
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
        
        _narrativeScriptPlayer.HandleChoice(1);
    }

    /// <summary>
    /// Exposes StoryPlayer's PresentEvidence method
    /// </summary>
    public void PresentEvidence(ICourtRecordObject courtRecordObject)
    {
        _narrativeScriptPlayer.PresentEvidence(courtRecordObject);
    }

    /// <summary>
    /// Exposes StoryPlayer's StartSubStory method
    /// </summary>
    public void StartSubStory(NarrativeScript narrativeScript)
    {
        _narrativeScriptPlayer.StartSubStory(narrativeScript);
    }
    
    /// <summary>
    /// Sets Waiting to false and continues the story
    /// </summary>
    public void SetWaitingToFalseAndContinue()
    {
        _narrativeScriptPlayer.Waiting = false;
        _narrativeScriptPlayer.Continue();
    }
}
