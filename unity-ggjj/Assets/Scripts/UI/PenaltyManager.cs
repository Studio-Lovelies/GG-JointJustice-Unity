using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PenaltyManager : MonoBehaviour, IPenaltyManager
{
    [FormerlySerializedAs("_game")] [SerializeField] private NarrativeGameState _narrativeGameState;

    [Tooltip("Drag the prefab for the penalty UI object here.")]
    [SerializeField]private Animator _penaltyObject;

    [SerializeField] private int _penaltyCount = 5;
    [SerializeField] private AudioClip _penaltyAudioClip;

    private readonly Queue<Animator> _penaltyObjects = new Queue<Animator>();

    public int PenaltiesLeft { get; private set; }

    /// <summary>
    /// Creates penalty UI objects on examination start
    /// </summary>
    public void OnCrossExaminationStart()
    {
        ResetPenalties();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide the penalty UI objects on cross examination end
    /// </summary>
    public void OnCrossExaminationEnd()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Destroy all penalty objects
    /// </summary>
    public void ResetPenalties()
    {
        PenaltiesLeft = _penaltyCount;
        foreach (var penaltyAnimator in _penaltyObjects)
        {
            Destroy(penaltyAnimator.gameObject);
        }
        _penaltyObjects.Clear();
        
        for (int i = 0; i < _penaltyCount; i++)
        {
            _penaltyObjects.Enqueue(Instantiate(_penaltyObject, transform));
        }
    }

    /// <summary>
    /// Decreases the number of penalties available to the player by one.
    /// </summary>
    /// <returns>True if number of penalties left is greater than 0, false if it is not.</returns>
    public void Decrement()
    {
        Debug.Assert(PenaltiesLeft > 0, "Decrement must not be called with 0 or fewer penalty lifelines left");
        PenaltiesLeft--;
        _penaltyObjects.Dequeue().Play("Explosion");
        _narrativeGameState.AudioController.PlaySfx(_penaltyAudioClip);
        if (PenaltiesLeft == 0)
        {
            _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScriptPlayer.OnNarrativeScriptComplete += OnNoLivesLeft;
        }
    }

    /// <summary>
    /// Tells the NarrativeScriptPlayer to play the game over script.
    /// Is executed when no lives are left at the end of the currently playing narrative script.
    /// </summary>
    private void OnNoLivesLeft()
    {
        var narrativeScriptPlayer = _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer;
        narrativeScriptPlayer.StartSubStory(_narrativeGameState.NarrativeScriptStorage.GameOverScript);
        narrativeScriptPlayer.OnNarrativeScriptComplete -= OnNoLivesLeft;
        _narrativeGameState.ActorController.StopTalking();
    }
}
