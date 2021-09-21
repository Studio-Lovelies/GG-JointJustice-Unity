using System.Collections;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Fades an Image between two alpha values.
/// Should be attached to the same object as the Image you wish to fade.
/// </summary>
[RequireComponent(typeof(Image))]
public class ImageFader : MonoBehaviour
{
    [SerializeField, Tooltip("Use this to add smoothing to the fade.")]
    private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private Image _image;

    /// <summary>
    /// Get the image component on Awake
    /// </summary>
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// Starts the coroutine to start fading to or from the assigned image.
    /// </summary>
    /// <param name="startAlpha">The alpha value to fade from</param>
    /// <param name="targetAlpha">The alpha value to fade to</param>
    /// <param name="time">The time in seconds to complete the animation</param>
    /// <param name="onComplete">The event to call once fading is complete</param>
    public void StartFade(float startAlpha, float targetAlpha, float time, UnityEvent onComplete)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeImage(startAlpha, targetAlpha, time, onComplete));
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
        _image.enabled = true;
        float startTime = Time.time;
        Color color = _image.color;

        while (Time.time < startTime + time)
        {
            float completion = (Time.time - startTime) / time;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, _animationCurve.Evaluate(completion));
            _image.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        _image.color = color;

        if (Mathf.Approximately(targetAlpha, 0))
        {
            gameObject.SetActive(false);
        }
        
        onComplete.Invoke();
    }
}
