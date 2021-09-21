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

    public bool HasSubPositions()
    {
        return _subPositions.Count > 0;
    }

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

    public Vector2Int GetTargetPosition()
    {
        return _currentSubPosition.position;
    }

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
