using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class EvidenceDictionaryParent<T> : MonoBehaviour where T : Object
{
    [SerializeField, Tooltip("Add any evidence that the player should have at the start of the scene here.")]
    protected T[] CurrentEvidenceList;

    protected CourtRecordObjectDictionary<T> CourtRecordObjectsDictionary;

    public int Count => CourtRecordObjectsDictionary.Count;

    public void AddValue(string valueName)
    {
        CourtRecordObjectsDictionary.AddEvidence(valueName);
    }

    public void RemoveValue(string valueName)
    {
        CourtRecordObjectsDictionary.RemoveEvidence(valueName);
    }

    public T GetValue(string valueName)
    {
        return CourtRecordObjectsDictionary.GetEvidence(valueName);
    }

    public T GetValueAtIndex(int index)
    {
        return CourtRecordObjectsDictionary.GetValueAtIndex(index);
    }
}
