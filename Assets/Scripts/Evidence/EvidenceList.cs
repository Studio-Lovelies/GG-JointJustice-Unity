using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Evidence Dictionary", menuName = "Evidence/Evidence Dictionary")]
public class EvidenceList : ScriptableObject
{
    [field: SerializeField, Tooltip("Add all the evidence required for this list here.")]
    public Evidence[] EvidenceArray { get; private set; }
}
