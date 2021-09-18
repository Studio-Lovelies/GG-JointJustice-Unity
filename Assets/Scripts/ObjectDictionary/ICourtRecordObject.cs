using UnityEngine;

public interface ICourtRecordObject
{ 
    public string InstanceName { get; }
    public string DisplayName { get; }
    public string CourtRecordName { get; }
    public Sprite Icon { get; }
    public string Description { get; }
}