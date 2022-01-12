using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvidenceController
{
    void AddEvidence(string evidenceName);
    void RemoveEvidence(string evidenceName);
    void AddToCourtRecord(string actorName);
    void RequirePresentEvidence();
    void SubstituteEvidenceWithAlt(string evidenceName);
    void OnPresentEvidence(ICourtRecordObject evidence);
}