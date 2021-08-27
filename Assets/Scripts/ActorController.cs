using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class ActorController : MonoBehaviour, IActorController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("Drag an ActorDictionary instance here, containing every required character")]
    [SerializeField] private ActorDictionary _actorDictionary;

    public bool Animating { get; set; }
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private ActorData _activeActor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void ShowActor()
    {
        _spriteRenderer.enabled = true;
    }

    public void HideActor()
    {
        _spriteRenderer.enabled = false;
    }
    
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

    public void FinishedAnimating()
    {
        Animating = false;
    }
}
