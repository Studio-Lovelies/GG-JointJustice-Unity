using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// TransitionController that fades to an image.
/// If this is being used to transition to a new scene it should be
/// attached to the same game object as the SceneLoader component.
/// </summary>
public class FadeToImageTransition : MonoBehaviour, ITransition
{
    [SerializeField, Tooltip("Drag the image component to fade here")]
    private Image _image;
    
    [SerializeField, Tooltip("Tick this if the scene should fade in.")]
    private bool _shouldFadeInOnAwake;
    
    [SerializeField, Tooltip("Drag a Fade To Image Settings instance here for the controller to use")]
    private FadeToImageSettings _fadeToImageSettings;
    
    [SerializeField, Tooltip("This event is called when transition at the end of a scene is complete. Should be subscribed to to load the next scene.")]
    private UnityEvent _onTransitionOutComplete;

    [SerializeField, Tooltip("This event is called when a transition at the start of a scene is complete. Should be subscribed to in order to start scene once faded in.")]
    private UnityEvent _onTransitionInComplete;
    
    private float _fadeTime;
    private AnimationCurve _fadeAnimationCurve;
    
    /// <summary>
    /// Unpack the fading settings and get all required components.
    /// Start the fading in transition if required.
    /// </summary>
    private void Awake()
    {
        if (!HasImage())
        {
            return;
        }
        
        if (_fadeToImageSettings == null)
        {
            Debug.LogError($"{this} missing FadeToImageSettings instance", this);
            return;
        }
            
        _image.sprite = _fadeToImageSettings.Sprite;
        _image.color = _fadeToImageSettings.Color;
        _fadeTime = _fadeToImageSettings.Time;
        _fadeAnimationCurve = _fadeToImageSettings.AnimationCurve;

        if (_shouldFadeInOnAwake)
        {
            _image.enabled = true;
            StartCoroutine(FadeImage(1, 0, _fadeTime, _onTransitionInComplete));
        }
    }
    
    /// <summary>
    /// Call this method to begin the transition at the end of a scene.
    /// Used by SceneLoader which does not necessarily know how a scene transition is being handled.
    /// </summary>
    public void Transition()
    {
        Fade(0, 1, _fadeTime, _onTransitionOutComplete);
    }

    /// <summary>
    /// Enables the game object and starts the coroutine to start fading to or from the assigned image.
    /// </summary>
    /// <param name="startAlpha">The alpha value to fade from</param>
    /// <param name="targetAlpha">The alpha value to fade to</param>
    /// <param name="time">The time in seconds to complete the animation</param>
    /// <param name="onComplete">The event to call once fading is complete</param>
    public void Fade(float startAlpha, float targetAlpha, float time, UnityEvent onComplete)
    {
        StartCoroutine(FadeImage(startAlpha, targetAlpha, time, onComplete));
    }

    /// <summary>
    /// Coroutine that fades an Image component from one level of transparency to another, over a specified time.
    /// </summary>
    /// <param name="startAlpha">The alpha value to fade from</param>
    /// <param name="targetAlpha">The alpha value to fade to</param>
    /// <param name="time">The time in seconds to complete the animation</param>
    /// <param name="onComplete">The event to call once fading is complete</param>
    public IEnumerator FadeImage(float startAlpha, float targetAlpha, float time, UnityEvent onComplete)
    {
        if (!HasImage())
        {
            yield break;
        }

        _image.enabled = true;
        float startTime = Time.time;
        Color color = _image.color;

        while (Time.time < startTime + time)
        {
            float completion = (Time.time - startTime) / time;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, _fadeAnimationCurve.Evaluate(completion));
            _image.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        _image.color = color;

        if (targetAlpha == 0)
        {
            _image.enabled = false;
        }
        
        onComplete.Invoke();
    }

    /// <summary>
    /// Method to check if an image component has been assigned.
    /// </summary>
    private bool HasImage()
    {
        if (_image != null) return true;
        if (TryGetComponent<Image>(out _image)) return true;
        Debug.LogError($"Image has not been assigned to {name}", gameObject);
        return false;
    }
}
