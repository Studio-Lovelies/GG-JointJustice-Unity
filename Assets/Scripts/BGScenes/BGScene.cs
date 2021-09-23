using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScene : MonoBehaviour
{
    [SerializeField] private List<SubPosition> _subPositions;
    public Actor ActiveActor { get; private set; }

    private SubPosition _currentSubPosition;

    /// <summary>
    /// Sets the active actor to the first actor in the scene.
    /// </summary>
    void Awake()
    {
        if (!HasSubPositions())
            ActiveActor = GetComponentInChildren<Actor>();
        else
            SetSubPosition(1);
    }

    /// <summary>
    /// Sees whether the current scene has sub positions
    /// </summary>
    /// <returns>whether this bg-scene has sub positions</returns>
    public bool HasSubPositions()
    {
        return _subPositions.Count > 0;
    }

    /// <summary>
    /// Sets the active sub position so the actor and position can be used.
    /// </summary>
    /// <param name="position">target position, 1 based.</param>
    public void SetSubPosition(int position)
    {
        int actualPosition = position - 1;
        
        if (actualPosition >= _subPositions.Count)
        {
            Debug.LogError($"No such subposition in scene {gameObject.name}");
            return;
        }

        _currentSubPosition = _subPositions[actualPosition];
        ActiveActor = _currentSubPosition.AttachedActor;
    }

    /// <summary>
    /// Gets the target position based on the current sub position
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetTargetPosition()
    {
        return _currentSubPosition.position;
    }


    /// <summary>
    /// Gets the actor object at a certain position without activating the system
    /// </summary>
    /// <param name="position">Target position, 1 based</param>
    /// <returns>Actor object at target position</returns>
    public Actor GetActorAtSubposition(int position)
    {
        int actualPosition = position - 1;
        if (actualPosition >= _subPositions.Count)
        {
            Debug.LogError($"No such subposition in scene {gameObject.name}");
            return null;
        }
        return _subPositions[actualPosition].AttachedActor;
    }

    
}

[System.Serializable]
public struct SubPosition
{
    public Actor AttachedActor;
    public Vector2Int position;
}
