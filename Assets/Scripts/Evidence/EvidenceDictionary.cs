using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceDictionary : MonoBehaviour
{
    [field: SerializeField, Tooltip("Add all the evidence required for this list here.")]
    public List<Evidence> EvidenceList { get; private set; }
    
    public Dictionary<string, Evidence> Dictionary { get; private set; }

    private void OnEnable()
    {
        if (Dictionary == null || Dictionary.Count != EvidenceList.Count)
        {
            Dictionary = new Dictionary<string, Evidence>();

            foreach (var evidence in EvidenceList)
            {
                Dictionary.Add(evidence.name, evidence);
            }
        }
    }
}
