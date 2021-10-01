using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BGScene : MonoBehaviour
{
    [Tooltip("List of potential actor slots in the scene that can be filled with ActorData and an associated camera position")]
    [SerializeField] private List<ActorSlot> _actorSlots;

    private ActorSlot _currentActorSlot;

    public Actor ActiveActor { get; private set; }

    /// <summary>
    /// Sets the active actor to the first actor in the scene.
    /// </summary>
    private void Awake()
    {
        if (SupportsActorSlots())
        {
            SetActiveActorSlot(1);
            return;
        }
            
        ActiveActor = GetComponentInChildren<Actor>();
    }

    /// <summary>
    /// Sees whether the current scene has support for actor slots
    /// </summary>
    /// <returns>whether this bg-scene has support for actor slots</returns>
    public bool SupportsActorSlots()
    {
        return _actorSlots.Count > 0;
    }

    /// <summary>
    /// Sets the active actor slot used when calculating the camera target position
    /// </summary>
    /// <param name="oneBasedSlotIndex">new active slot index, 1 based.</param>
    /// <see cref="GetTargetPosition"/>
    public void SetActiveActorSlot(int oneBasedSlotIndex)
    {
        int slotIndex = oneBasedSlotIndex - 1;
        
        if (slotIndex >= _actorSlots.Count)
        {
            Debug.LogError($"Slot with index '{slotIndex}' (one-based index '{oneBasedSlotIndex}') not found in scene {gameObject.name}");
            return;
        }

        _currentActorSlot = _actorSlots[slotIndex];
        ActiveActor = _currentActorSlot.AttachedActor;
    }

    /// <summary>
    /// Gets the target position based on the current actor slot
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetTargetPosition()
    {
        return _currentActorSlot.Position;
    }


    /// <summary>
    /// Gets the actor object at a certain slot without activating the system
    /// </summary>
    /// <param name="oneBasedSlotIndex">Target slot index, 1 based</param>
    /// <returns>Actor object at target slot</returns>
    public Actor GetActorAtSlot(int oneBasedSlotIndex)
    {
        int slotIndex = oneBasedSlotIndex - 1;
        if (slotIndex >= _actorSlots.Count)
        {
            Debug.LogError($"Slot with index '{slotIndex}' (one-based index '{oneBasedSlotIndex}') not found in scene {gameObject.name}");
            return null;
        }
        return _actorSlots[slotIndex].AttachedActor;
    }


    [System.Serializable]
    public struct ActorSlot
    {
        public Actor AttachedActor;
        public Vector2Int Position;
    }
}
