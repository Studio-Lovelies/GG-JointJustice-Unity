using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor List", menuName = "Actors/Actor List")]
public class ActorList : ScriptableObject
{
    [field: SerializeField] public List<ActorData> Actors { get; private set; }
}
