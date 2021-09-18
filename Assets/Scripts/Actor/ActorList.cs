using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor List", menuName = "Actors/Actor List")]
public class ActorList : ScriptableObject, IObjectList<ActorData>
{
    [field: SerializeField]
    public ActorData[] ObjectArray { get; private set; } = Array.Empty<ActorData>();
}
