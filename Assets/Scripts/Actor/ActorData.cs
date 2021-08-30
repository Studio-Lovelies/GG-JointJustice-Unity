using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor Data", menuName = "Actors/Actor Data")]
public class ActorData : ScriptableObject
{
    [field: SerializeField, Tooltip("The actor's sprite that will appear in the profiles menu.")]
    public Sprite Profile { get; private set; }
    
    [field: SerializeField, Tooltip("The sprite that should be displayed if an incompatible animation is played.")]
    public Sprite DefaultSprite { get; private set; }
    
    [field: SerializeField, TextArea, Tooltip("A description of the actor that will appear in the profiles menu.")]
    public string Bio { get; private set; }

    [field: SerializeField, Tooltip("The animator controllers that this actor uses.")]
    public RuntimeAnimatorController AnimatorController { get; private set; }
}
