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

    [Tooltip("The event is called when an actors animation completes.")]
    [SerializeField] private UnityEvent _onAnimationComplete;

    public bool Animating { get; set; }
    
    private ActorData _activeActor;
    private bool _shouldCallOnAnimationComplete;

    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
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
            _activeActor = _actorDictionary.Actors[actor];
            _animator.runtimeAnimatorController = _activeActor.AnimatorController;
        }
        catch (KeyNotFoundException exception)
        {
            Debug.Log($"{exception.GetType().Name}: Actor was not found in actor dictionary");
        }
    }

    /// <summary>
    /// Sets the emotion of an actor by playing the specified emotion on the actor's animator.
    /// </summary>
    /// <param name="emotion">The emotion to play.</param>
    public void SetEmotion(string emotion)
    {
        if (_activeActor == null)
        {
            Debug.LogError("Actor has not been assigned");
            return;
        }
        
        if (_animator == null)
        {
            Debug.LogError("Current actor has not been assigned an animator controller.");
            return;
        }

        if (!_animator.HasState(0, Animator.StringToHash(emotion)))
        {
            Debug.LogError($"Could not find emotion {emotion} on animator of actor {_activeActor.name}.");
            return;
        }
        
        _animator.Play(emotion);
    }

    public void SetActiveSpeaker(string actor)
    {
        Debug.LogWarning("SetActiveSpeaker not implemented");
    }

    /// <summary>
    /// This method is called by animations when they are completed and required the next line to be to be read.
    /// </summary>
    public void OnAnimationComplete()
    {
        _onAnimationComplete?.Invoke();
    }
}
