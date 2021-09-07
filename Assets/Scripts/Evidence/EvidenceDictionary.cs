using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvidenceDictionary : EvidenceDictionaryParent<Evidence>
{
    [SerializeField, Tooltip("Drag an EvidenceList here containing all the evidence available in the scene")]
    private EvidenceList _masterEvidenceList;

    /// <summary>
    /// Converts the evidence list into a dictionary on awake
    /// </summary>
    private void Awake()
    {
        CourtRecordObjectsDictionary =
            new CourtRecordObjectDictionary<Evidence>(_masterEvidenceList.EvidenceArray, CurrentEvidenceList);
    }

    /// <summary>
    /// Replaces an Evidence object with its designated alternate evidence.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to substitute.</param>
    public void SubstituteEvidenceWithAlt(string evidenceName)
    {
        Evidence altEvidence = CourtRecordObjectsDictionary.GetEvidence(evidenceName).AltEvidence;
        CourtRecordObjectsDictionary.SubstituteValueWithAlt(evidenceName, altEvidence);
    }
}
