using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class ActorController : MonoBehaviour, IActorController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("Contains all the possible actors that can appear.")]
    [SerializeField] private ActorList _actorList;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private ActorData _activeActor;
    private string _activeEmotion = "Normal";
    private string _previousEmotion;

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
        _activeActor = Resources.Load<ActorData>($"Actors/{actor}");
        if (_activeActor == null)
        {
            Debug.LogError($"Actor \"{actor}\" could not be found.");
            return;
        }

        // try
        // {
        //     _activeActor = _actorList.Actors[actor];
        // }
        // catch (Exception exception)
        // {
        //     Debug.LogError($"{exception.GetType().Name}: {actor} could not be found in the dictionary.");
        //     return;
        // }
        
        _animator.runtimeAnimatorController = _activeActor.AnimatorController;
    }

    public void SetEmotion(string emotion)
    {
        _previousEmotion = _activeEmotion;
        _animator.Play(emotion);
        _activeEmotion = emotion;
    }

    public void SetActiveSpeaker(string actor)
    {
        Debug.LogWarning("SetActiveSpeaker not implemented");
    }

    public void ResumePreviousEmotion()
    {
        SetEmotion(_previousEmotion);
    }
}
