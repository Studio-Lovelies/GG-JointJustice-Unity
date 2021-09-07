using UnityEngine;

public class ActorDictionaryBehaviour : EvidenceDictionaryParent<ActorData>
{
    [SerializeField, Tooltip("Drag an Actor Dictionary here containing all the actors available in the scene.")]
    private ActorDictionary _masterActorList;
    
    /// <summary>
    /// Converts the actor list into a dictionary on awake
    /// </summary>
    private void Awake()
    {
        CourtRecordObjectsDictionary =
            new CourtRecordObjectDictionary<ActorData>(_masterActorList.ActorList, CurrentEvidenceList);
    }
}