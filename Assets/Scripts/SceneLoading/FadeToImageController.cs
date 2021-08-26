using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TransitionController that fades to an image.
/// Requires an Image component to function. If no Image component is present
/// one will be created automatically.
/// </summary>
[RequireComponent(typeof(Image))]
public class FadeToImageController : TransitionController
{
    [SerializeField, Tooltip("Drag a Fade To Image Settings instance here for the controller to use")]
    private FadeToImageSettings _fadeToImageSettings;

    private Image _image;
    private float _fadeTime;
    private AnimationCurve _fadeAnimationCurve;
    
    /// <summary>
    /// Unpack the fading settings and get all required components.
    /// </summary>
    private void Awake()
    {
        _image = GetComponent<Image>();

        if (_fadeToImageSettings == null)
        {
            Debug.LogError($"{this} missing FadeToImageSettings instance", this);
        }
            
        _image.sprite = _fadeToImageSettings.Sprite;
        _image.color = _fadeToImageSettings.Color;
        _fadeTime = _fadeToImageSettings.Time;
        _fadeAnimationCurve = _fadeToImageSettings.AnimationCurve;
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
        yield return null;
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
