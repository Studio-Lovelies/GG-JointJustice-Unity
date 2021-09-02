using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// TransitionController that fades to an image.
/// Requires an Image component to function. If no Image component is present
/// one will be created automatically.
/// </summary>
[RequireComponent(typeof(Image))]
public class FadeToImageController : TransitionController
{
    [SerializeField, Tooltip("Tick this if the scene should fade in.")]
    private bool _shouldFadeInOnAwake;
    
    [SerializeField, Tooltip("Drag a Fade To Image Settings instance here for the controller to use")]
    private FadeToImageSettings _fadeToImageSettings;

    private Image _image;
    private float _fadeTime;
    private AnimationCurve _fadeAnimationCurve;
    
    /// <summary>
    /// Unpack the fading settings and get all required components.
    /// Start the fading in transition if required.
    /// </summary>
    private void Awake()
    {
        _image = GetComponent<Image>();

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
    /// </summary>
    public override void Transition()
    {
        gameObject.SetActive(true);
        _image.enabled = true;
        StartCoroutine(FadeImage(0, 1, _fadeTime, _onTransitionOutComplete));
    }

    /// <summary>
    /// Coroutine that fades an Image component from one level of transparency to another, over a specified time.
    /// </summary>
    /// <param name="startAlpha">The alpha value to fade from</param>
    /// <param name="targetAlpha">The alpha value to fade to</param>
    /// <param name="time">The time in seconds to complete the animation</param>
    /// <param name="onComplete">The event to call once fading is complete</param>
    private IEnumerator FadeImage(float startAlpha, float targetAlpha, float time, UnityEvent onComplete)
    {
        float startTime = Time.time;
        Color color = _image.color;

        while (Time.time < startTime + time)
        {
            float completion = (Time.time - startTime) / time;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, _fadeAnimationCurve.Evaluate(completion));
            _image.color = color;
            yield return null;
        }

        onComplete.Invoke();
    }
}
