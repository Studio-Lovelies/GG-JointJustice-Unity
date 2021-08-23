using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor List", menuName = "Actors/Actor List")]
public class ActorList : ScriptableObject
{
    [field: SerializeField] public List<ActorData> ActorsList { get; private set; }
    
    public Dictionary<string, ActorData> Actors { get; private set; } = new Dictionary<string, ActorData>();

    public void OnEnable()
    {
        foreach (var actorData in ActorsList)
        {
            Actors.Add(actorData.name, actorData);
        }
    }
}
