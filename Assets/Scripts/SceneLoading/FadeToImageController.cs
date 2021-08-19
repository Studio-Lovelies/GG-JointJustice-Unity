using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TransitionController that fades to an image.
/// Attach this to a game object with an Image component.
/// </summary>
public class FadeToImageController : TransitionController
{
    [SerializeField, Tooltip("Time taken to fade")]
    private float _fadeTime;

    [SerializeField, Tooltip("The animation curve used when animating the fadeout. Can be adjusted to modify the animation")]
    private AnimationCurve _fadeAnimationCurve;

    private Image _image;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    /// <summary>
    /// Start this coroutine to begin the transition.
    /// </summary>
    public override IEnumerator Transition()
    {
        gameObject.SetActive(true);
        Color imageColor = _image.color;
        imageColor.a = 0;
        _image.color = imageColor;
        yield return StartCoroutine(FadeImage(_image, 0, 1, _fadeTime));
    }
    
    /// <summary>
    /// Coroutine that fades an Image component from one level of transparency to another, over a specified time.
    /// </summary>
    /// <param name="image">The image to fade.</param>
    /// <param name="startAlpha">The alpha value to fade from</param>
    /// <param name="targetAlpha">The alpha value to fade to</param>
    /// <param name="time">The time in seconds to complete the animation</param>
    private IEnumerator FadeImage(Image image, float startAlpha, float targetAlpha, float time)
    {
        float startTime = Time.time;
        Color color = image.color;

        while (Time.time < startTime + time)
        {
            float completion = (Time.time - startTime) / time;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, _fadeAnimationCurve.Evaluate(completion));
            image.color = color;
            yield return null;
        }
    }
}
