using UnityEngine;

[CreateAssetMenu(fileName = "New Evidence List", menuName = "Evidence/Evidence List")]
public class EvidenceList : ScriptableObject, IObjectList<Evidence>
{
    [field: SerializeField, Tooltip("Add all the evidence required for this list here.")]
    public Evidence[] ObjectList { get; private set; }
}
