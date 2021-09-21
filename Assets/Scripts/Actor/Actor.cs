using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class Actor : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private ActorData _actorData;
    private IActorController _attachedController;

    /// <summary>
    /// Called on awake, before Start
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.keepAnimatorControllerStateOnDisable = true;
    }

    /// <summary>
    /// Extracts required data from an ActorData object and stores it.
    /// </summary>
    /// <param name="actorData"></param>
    public void SetActor(ActorData actorData)
    {
        _actorData = actorData;
        _animator.runtimeAnimatorController = actorData.AnimatorController;
        _animator.Play("Normal");
    }

    /// <summary>
    /// Checks if an animation is on the animator controller (if one is present) and plays it.
    /// </summary>
    /// <param name="animation"></param>
    public void PlayAnimation(string animation)
    {
        int animationHash = Animator.StringToHash(animation); // Required for HasState method
        
        if (_animator.runtimeAnimatorController == null)
        {
            Debug.LogError("Current actor has not been assigned an animator controller.");
            return;
        }

        if (!_animator.HasState(0, animationHash))
        {
            Debug.LogError($"Could not find emotion {animation} on animator of actor {_actorData.name}.");
            return;
        }

        _animator.Play(animationHash);
    }
    
    /// <summary>
    /// This method is called by animations when they are completed and require the next line to be to be read.
    /// </summary>
    public void OnAnimationComplete()
    {
        if (_attachedController != null)
        {
            _attachedController.OnAnimationDone();
        }
    }

    /// <summary>
    /// Used to attach an actor controller for callbacks
    /// </summary>
    /// <param name="controller">Actor controller to attach to this object</param>
    public void AttachController(IActorController controller)
    {
        _attachedController = controller;
    }

    /// <summary>
    /// Makes the actor talk while set to true
    /// </summary>
    /// <param name="isTalking">Move mouth or not</param>
    public void SetTalking(bool isTalking)
    {
        _animator.SetBool("Talking", isTalking);
    }
    
    /// <summary>
    /// Checks if this actor is the actor passed
    /// </summary>
    /// <param name="actor">Actor to compare to</param>
    /// <returns>If the actor is the actor given</returns>
    public bool MatchesActorData(ActorData actor)
    {
        return _actorData == actor;
    }
}
