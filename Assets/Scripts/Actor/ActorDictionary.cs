using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor Dictionary", menuName = "Actors/Actor Dictionary")]
public class ActorDictionary : ScriptableObject
{
    [SerializeField] private List<ActorData> _actorDataObjects = new List<ActorData>(); // Initialise to default value to prevent null reference exception on asset creation
    public Dictionary<string, ActorData> Actors { get; } = new Dictionary<string, ActorData>();

    public ActorData[] ActorList => _actorDataObjects.ToArray();
    
    /// <summary>
    /// Translate the _actorDatas list into a dictionary so
    /// an actor's data can be retrieved using its name.
    /// </summary>
    private void OnEnable()
    {
        // Don't bother doing this if the dictionary already contains actors
        if (Actors.Count == _actorDataObjects.Count) return;
            
        foreach (var actorData in _actorDataObjects)
        {
            Actors.Add(actorData.name, actorData);
        }
    }
}
