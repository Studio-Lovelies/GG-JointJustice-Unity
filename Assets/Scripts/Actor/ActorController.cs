using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour, IActorController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("Drag an ActorDictionary instance here, containing every required character")]
    [SerializeField] private ActorDictionary _actorDictionary;

    //TODO serialized for debug purposes. Should be set by scene controller.
    [field: SerializeField] public Actor ActiveActor { get; set; }

    public bool Animating { get; set; }

    private void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Actor Controller doesn't have an action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.SetActorController(this);
        }
    }

    /// <summary>
    /// Retrieves actor data from the actor dictionary and uses it set the active actor.
    /// Gives an exception if the actor is not found.
    /// </summary>
    /// <param name="actor">The name of the actor as it appears in the dictionary.
    /// Actors use the same name as their ActorData object.</param>
    public void SetActiveActor(string actor)
    {
        try
        { 
            ActiveActor.SetActor(_actorDictionary.Actors[actor]);
        }
        catch (KeyNotFoundException exception)
        {
            Debug.Log($"{exception.GetType().Name}: Actor was not found in actor dictionary");
        }
    }

    /// <summary>
    /// Sets the animation of an actor by playing the specified animation on the actor's animator.
    /// Used to set poses, emotions, and play animations.
    /// </summary>
    /// <param name="emotion">The emotion to play.</param>
    public void PlayAnimation(string emotion)
    {
        if (ActiveActor == null)
        {
            Debug.LogError("Actor has not been assigned");
            return;
        }

        ActiveActor.PlayAnimation(emotion);
    }

    public void SetActiveSpeaker(string actor)
    {
        Debug.LogWarning("SetActiveSpeaker not implemented");
    }
    
    
}
