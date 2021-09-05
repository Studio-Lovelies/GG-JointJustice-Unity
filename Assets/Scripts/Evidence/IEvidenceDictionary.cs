using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvidenceDictionary
{
    int Count { get; }
    void AddEvidence(string evidenceName);
    void RemoveEvidence(string evidenceName);
    Evidence GetEvidence(string evidenceName);
    void SubstituteEvidenceWithAlt(string evidenceName);
    Evidence GetEvidenceAtIndex(int index);
}
