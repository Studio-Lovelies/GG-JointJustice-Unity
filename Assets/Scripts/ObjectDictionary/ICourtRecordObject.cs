using UnityEngine;

public interface ICourtRecordObject
{ 
    public string name { get; }
    public string DisplayName { get; }
    public string CourtRecordName { get; }
    public Sprite Icon { get; }
    public string Description { get; }
}