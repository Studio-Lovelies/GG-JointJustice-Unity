using System;
using UnityEngine;

namespace SceneLoading
{
    /// <summary>
    /// TransitionController that fades to an image.
    /// If this is being used to transition to a new scene it should be
    /// attached to the same game object as the SceneLoader component.
    /// </summary>
    public class FadeToImageTransition : MonoBehaviour, ITransition
    {
        [SerializeField, Tooltip("Drag the image component to fade here")]
        private ImageFader _imageFader;

        [SerializeField, Tooltip("The time in seconds to fade.")]
        private float _fadeTime;
    
        /// <summary>
        /// Call this method to begin the transition at the end of a scene.
        /// Used by SceneLoader which does not necessarily know how a scene transition is being handled.
        /// </summary>
        public void Transition(Action callback)
        {
            _imageFader.StartFade(0, 1, _fadeTime, callback);
        }
    }
}
