using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceDictionary : MonoBehaviour
{
    [field: SerializeField, Tooltip("Add all the evidence required for this list here.")]
    public List<Evidence> EvidenceList { get; private set; }
    
    public Dictionary<string, Evidence> Dictionary { get; private set; }
}
