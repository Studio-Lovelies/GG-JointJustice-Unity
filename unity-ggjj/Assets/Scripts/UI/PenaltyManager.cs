using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PenaltyManager : MonoBehaviour, IPenaltyManager
{
    [Tooltip("Drag a DirectorActionDecoder here")]
    [SerializeField] private DirectorActionDecoder _directorActionDecoder;
    
    [Tooltip("Drag the prefab for the penalty UI object here.")]
    [SerializeField]private Animator _penaltyObject;

    [SerializeField] private int _penaltyCount = 5;

    [Header("Events")]
    [SerializeField] private UnityEvent _onPenaltiesDepleted;
    
    private readonly Queue<Animator> _penaltyObjects = new Queue<Animator>();

    public int PenaltiesLeft => _penaltyObjects.Count;

    private void Awake()
    {
        _directorActionDecoder.Decoder.PenaltyManager = this;
    }
    
    /// <summary>
    /// Creates penalty UI objects on examination start
    /// </summary>
    public void OnCrossExaminationStart()
    {
        for (int i = 0; i < _penaltyCount; i++)
        {
            _penaltyObjects.Enqueue(Instantiate(_penaltyObject, transform));
        }
    }

    /// <summary>
    /// Destroy all penalty objects on examination end
    /// </summary>
    public void OnCrossExaminationEnd()
    {
        for (int i = 0; i < _penaltyObjects.Count; i++)
        {
            var penalty = _penaltyObjects.Dequeue();
            Destroy(penalty.gameObject);
        }
    }

    /// <summary>
    /// Decreases the number of penalties available to the player by one.
    /// </summary>
    /// <returns>True if number of penalties left is greater than 0, false if it is not.</returns>
    public void Decrement()
    {
        Debug.Assert(PenaltiesLeft > 0, "Decrement must not be called with 0 or fewer penalty lifelines left");
        _penaltyObjects.Dequeue().Play("Explosion");
        CheckGameOver();
    }

    /// <summary>
    /// Checks if all penalties are depleted and calls the onPenaltiesDepleted event if so.
    /// </summary>
    private void CheckGameOver()
    {
        if (_penaltyCount <= 0)
        {
            _onPenaltiesDepleted.Invoke();
        }
    }
}
