using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class AnimatableObject : MonoBehaviour
{
    [SerializeField, Tooltip("This event is called when an animation begins.")]
    private UnityEvent _onAnimationBegin;
    
    [SerializeField, Tooltip("This events is called when an animation completes.")]
    private UnityEvent _onAnimationComplete;

    private SpriteRenderer _spriteRenderer;

    protected Animator Animator { get; private set; }
    
    /// <summary>
    /// Get required components
    /// </summary>
    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }
    
    /// <summary>
    /// Checks if an animation is on the animator controller (if one is present) and plays it.
    /// </summary>
    /// <param name="animation"></param>
    public void PlayAnimation(string animation)
    {
        int animationHash = Animator.StringToHash(animation); // Required for HasState method
        
        if (Animator.runtimeAnimatorController == null)
        {
            Debug.LogError($"Animator on {gameObject.name} has not been assigned an animator controller.");
            return;
        }

        if (!Animator.HasState(0, animationHash))
        {
            Debug.LogError($"Could not find animation {animation} on animator {Animator.runtimeAnimatorController.name}.");
            return;
        }
        
        Animator.enabled = true;
        _onAnimationBegin.Invoke();
        Animator.Play(animationHash);
    }

    /// <summary>
    /// Method to end the animation currently playing.
    /// Should be called when an animation should not pause on the final frame (e.g. Ross' galaxy brain animation).
    /// </summary>
    public void EndAnimation()
    {
        Animator.enabled = false;
        _spriteRenderer.sprite = null;
    }
    
    /// <summary>
    /// This method is called by animations when they are completed and require the next line to be to be read.
    /// </summary>
    public virtual void OnAnimationComplete()
    {
        _onAnimationComplete.Invoke();
    }
}
