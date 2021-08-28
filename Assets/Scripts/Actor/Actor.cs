using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class Actor : MonoBehaviour
{
    [Tooltip("The event is called when an actor's animation is complete.")]
    [SerializeField] private UnityEvent _onAnimationComplete;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private ActorData _actorData;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
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
        _onAnimationComplete?.Invoke();
    }
}
