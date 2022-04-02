using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BGScene : MonoBehaviour
{
    [Serializable]
    public struct ActorSlot
    {
        public Actor AttachedActor { get; set; }
        public Vector2Int Position { get; set; }
    }

    private Dictionary<string, ActorSlot> _actorSlotBySlotName;
    public ActorSlot CurrentActorSlot { get; private set; }

    /// <summary>
    /// Sets the active actor to the first actor in the scene.
    /// </summary>
    private void Awake()
    {
        static string NormalizeActorName(Actor actor) => actor.name.Replace("_Actor", "");
        static ActorSlot GenerateSlotDataForActor(Actor actor)
        {
            var position = actor.GetComponent<Transform>().localPosition * 100;
            return new ActorSlot()
            {
                AttachedActor = actor,
                Position = new Vector2Int((int)position.x, (int)position.y)
            };
        }

        _actorSlotBySlotName = GetComponentsInChildren<Actor>().ToDictionary(NormalizeActorName, GenerateSlotDataForActor);
        CurrentActorSlot = _actorSlotBySlotName.First().Value;
    }

    /// <summary>
    /// Sets the active actor slot used when calculating the camera target position
    /// </summary>
    /// <param name="slotName">Name of an actor slot of the currently active scene</param>
    public void SetActiveActorSlot(string slotName)
    {
        if (_actorSlotBySlotName.Count < 2)
        {
            throw new NotSupportedException("Can't assign actor to slot: This scene has no support for multiple actor slots");
        }

        if (!_actorSlotBySlotName.ContainsKey(slotName))
        {
            throw new KeyNotFoundException($"Slot with name '{slotName}'  not found in scene '{gameObject.name}' - available slots: '{string.Join("', '", _actorSlotBySlotName.Select(slotEntry => slotEntry.Key))}'");
        }

        CurrentActorSlot = _actorSlotBySlotName[slotName];
    }


    /// <summary>
    /// Gets the actor object at a certain slot without activating the system
    /// </summary>
    /// <param name="slotName">Name of an actor slot of the currently active scene</param>
    /// <returns>Actor object at target slot</returns>
    public Actor GetActorAtSlot(string slotName)
    {
        if (_actorSlotBySlotName.Count < 2)
        {
            throw new NotSupportedException("Can't assign actor to slot: This scene has no support for multiple actor slots");
        }

        if (!_actorSlotBySlotName.ContainsKey(slotName))
        {
            throw new KeyNotFoundException($"Slot with name '{slotName}'  not found in scene '{gameObject.name}' - available slots: '{string.Join("', '", _actorSlotBySlotName.Select(slotEntry => slotEntry.Key))}'");
        }

        return _actorSlotBySlotName[slotName].AttachedActor;
    }
}
