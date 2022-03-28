using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New EvidenceData", menuName = "EvidenceData/EvidenceData")]
public class EvidenceData : ScriptableObject, ICourtRecordObject
{
    [field: SerializeField, Tooltip("The name of the evidence, display in the evidence menu.")]
    public string DisplayName { get; private set; }

    [field: SerializeField, Tooltip("Icon used to represent the evidence.")]
    public Sprite Icon { get; private set; }
    
    [field: SerializeField, TextArea, Tooltip("Description of the evidence that will appear in the evidence menu.")]
    public string Description { get; private set; }

    public string InstanceName => name;
    public string CourtRecordName => DisplayName;
}
