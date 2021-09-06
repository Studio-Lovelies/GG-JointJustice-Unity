using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FailureList", menuName = "Story/Failure List")]
public class FailureStoryList : ScriptableObject
{
    [SerializeField] private List<TextAsset> _failureStates;

    public TextAsset GetRandomFailurestate()
    {
        if (_failureStates == null || _failureStates.Count == 0)
        {
            return null;
        }
        return _failureStates[Random.Range(0, _failureStates.Count)];
    }
}
