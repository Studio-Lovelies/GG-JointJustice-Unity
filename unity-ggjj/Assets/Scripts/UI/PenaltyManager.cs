using System.Collections.Generic;
using UnityEngine;

public class PenaltyManager : MonoBehaviour, IPenaltyManager
{
    [SerializeField] private Game _game;
    
    [Tooltip("Drag a NarrativeScriptPlaylist here")]
    [SerializeField] private NarrativeScriptPlaylist _narrativeScriptPlaylist;
    
    [Tooltip("Drag the prefab for the penalty UI object here.")]
    [SerializeField]private Animator _penaltyObject;

    [SerializeField] private int _penaltyCount = 5;

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
        CreatePenalties();
    }

    /// <summary>
    /// Creates penalties equal to the value of _penaltyCount
    /// </summary>
    private void CreatePenalties()
    {
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
        CheckGameOver();
    }

    /// <summary>
    /// Checks if all penalties are depleted and calls the onPenaltiesDepleted event if so.
    /// </summary>
    private void CheckGameOver()
    {
        if (_penaltyObjects.Count == 0)
        {
            _game.NarrativeScriptPlayer.StartSubStory(_narrativeScriptPlaylist.GameOverScript);
        }
    }
}
