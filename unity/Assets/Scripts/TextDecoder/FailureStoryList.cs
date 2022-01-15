using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FailureList", menuName = "Story/Failure List")]
public class FailureStoryList : ScriptableObject
{
    [SerializeField] private List<TextAsset> _failureStates;

    /// <summary>
    /// Gets a random failure state from the list.
    /// </summary>
    /// <returns>Inky dialogue script containing a failure sub-story.</returns>
    public TextAsset GetRandomFailurestate()
    {
        if (_failureStates == null || _failureStates.Count == 0)
        {
            return null;
        }
        return _failureStates[Random.Range(0, _failureStates.Count)];
    }
}
