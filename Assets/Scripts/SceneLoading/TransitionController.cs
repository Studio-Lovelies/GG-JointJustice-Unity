using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract class that different types of TransitionControllers can inherit from
/// in order to implement how they should transition.
/// </summary>
public abstract class TransitionController : MonoBehaviour
{
    public abstract void Transition();
    
    [SerializeField, Tooltip("This event is called when transition at the end of a scene is complete. Should be subscribed to to load the next scene.")]
    protected UnityEvent _onTransitionOutComplete;

    [SerializeField, Tooltip("This event is called when a transition at the start of a scene is complete. Should be subscribed to in order to start scene once faded in.")]
    protected UnityEvent _onTransitionInComplete;
}
