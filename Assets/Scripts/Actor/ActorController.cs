using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActorController : MonoBehaviour, IActorController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("Drag an ActorDictionary instance here, containing every required character")]
    [SerializeField] private ActorDictionary _actorDictionary;

    //TODO serialized for debug purposes. Should be set by scene controller.
    [SerializeField] private Actor _activeActor;

    [SerializeField] private UnityEvent _onAnimationStarted;
    [SerializeField] private UnityEvent _onAnimationComplete;

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

        SetActiveActorObject(_activeActor); //Debug thing, should be called by the scene controller
    }

    public void SetActiveActorObject(Actor actor)
    {
        _activeActor = actor;
        actor.AttachController(this);
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
            _activeActor.SetActor(_actorDictionary.Actors[actor]);
        }
        catch (KeyNotFoundException exception)
        {
            Debug.Log($"{exception.GetType().Name}: Actor was not found in actor dictionary");
        }
    }

    public void SetPose(string pose)
    {
        if (_activeActor == null)
        {
            Debug.LogError("Actor has not been assigned");
            return;
        }
        _activeActor.PlayAnimation(pose);
    }

    /// <summary>
    /// Sets the animation of an actor by playing the specified animation on the actor's animator.
    /// Used to set poses, emotions, and play animations.
    /// </summary>
    /// <param name="emotion">The emotion to play.</param>
    public void PlayEmotion(string emotion)
    {
        if (_activeActor == null)
        {
            Debug.LogError("Actor has not been assigned");
            _onAnimationComplete.Invoke();
            return;
        }
        _onAnimationStarted.Invoke();
        _activeActor.PlayAnimation(emotion);
    }

    public void SetActiveSpeaker(string actor)
    {
        Debug.LogWarning("SetActiveSpeaker not implemented");
    }

    public void StartTalking()
    {
        if (_activeActor == null)
        {
            Debug.LogError("Actor has not been assigned");
            return;
        }

        _activeActor.SetTalking(true);
    }

    public void StopTalking()
    {
        if (_activeActor == null)
        {
            Debug.LogError("Actor has not been assigned");
            return;
        }
        _activeActor.SetTalking(false);
    }

    public void OnAnimationDone()
    {
        _onAnimationComplete.Invoke();
    }
}
