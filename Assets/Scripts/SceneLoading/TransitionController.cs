using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract class that different types of TransitionControllers can inherit from
/// in order to implement how they should transition.
/// </summary>
public abstract class TransitionController : MonoBehaviour
{
    public abstract IEnumerator Transition();
}
