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
    private ImageFader _imageFader;
    
    [SerializeField, Tooltip("Tick this if the scene should fade in.")]
    private bool _shouldFadeInOnAwake;

    [SerializeField, Tooltip("The time in seconds to fade.")]
    private float _fadeTime;
    
    [SerializeField, Tooltip("This event is called when transition at the end of a scene is complete. Should be subscribed to to load the next scene.")]
    private UnityEvent _onTransitionOutComplete;

    [SerializeField, Tooltip("This event is called when a transition at the start of a scene is complete. Should be subscribed to in order to start scene once faded in.")]
    private UnityEvent _onTransitionInComplete;

    /// <summary>
    /// Unpack the fading settings and get all required components.
    /// Start the fading in transition if required.
    /// </summary>
    private void Start()
    {
        if (_shouldFadeInOnAwake)
        {
            _imageFader.StartFade(1, 0, _fadeTime, _onTransitionInComplete);
        }
    }
    
    /// <summary>
    /// Call this method to begin the transition at the end of a scene.
    /// Used by SceneLoader which does not necessarily know how a scene transition is being handled.
    /// </summary>
    public void Transition()
    {
        _imageFader.StartFade(0, 1, _fadeTime, _onTransitionOutComplete);
    }
}
