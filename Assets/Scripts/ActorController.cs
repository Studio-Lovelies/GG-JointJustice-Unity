using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class ActorController : MonoBehaviour, IActorController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private ActorData _activeActor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    void Start()
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
        
        _animator.runtimeAnimatorController = _activeActor.AnimatorController;
    }

    public void SetEmotion(string emotion)
    {
        _animator.Play(emotion);
    }

    public void SetActiveSpeaker(string actor)
    {
        Debug.LogWarning("SetActiveSpeaker not implemented");
    }
}
