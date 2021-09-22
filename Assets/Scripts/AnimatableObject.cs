using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class AnimatableObject : MonoBehaviour
{
    [SerializeField, Tooltip("This events is called when an animation completes.")]
    private UnityEvent _onAnimationComplete;
    
    protected Animator Animator { get; private set; }
    
    /// <summary>
    /// Get required components
    /// </summary>
    protected virtual void Awake()
    {
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
        
        gameObject.SetActive(true);
        Animator.Play(animationHash);
    }
    
    /// <summary>
    /// This method is called by animations when they are completed and require the next line to be to be read.
    /// </summary>
    public virtual void OnAnimationComplete()
    {
        _onAnimationComplete.Invoke();
    }
}
