using UnityEngine;

public class NarrativeScriptPlayerComponentComponent : MonoBehaviour, INarrativeScriptPlayerComponent
{
    [SerializeField] private NarrativeGameState _narrativeGameState;
    
    private NarrativeScriptPlayer _narrativeScriptPlayer;

    public INarrativeScriptPlayer NarrativeScriptPlayer => _narrativeScriptPlayer;

    public bool Waiting
    {
        get => _narrativeScriptPlayer.Waiting;
        set => _narrativeScriptPlayer.Waiting = value;
    }

    private void Awake()
    {
        _narrativeScriptPlayer = new NarrativeScriptPlayer(_narrativeGameState)
        {
            ActiveNarrativeScript = _narrativeGameState.NarrativeScriptPlaylist.GetNextNarrativeScript()
        };
    }

    /// <summary>
    /// Exposes NarrativeScriptPlayer's Continue method for use in UnityEvents
    /// </summary>
    public void Continue()
    {
        _narrativeScriptPlayer.Continue();
    }

    /// <summary>
    /// Exposes NarrativeScriptPlayer's TryPressWitness method for use in UnityEvents
    /// </summary>
    public void TryPressWitness()
    {
        _narrativeScriptPlayer.TryPressWitness();
    }
}
