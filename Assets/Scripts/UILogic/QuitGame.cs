using System.Collections;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField, Tooltip("Assign a transition controller here if a transition is required when changing the scene.")]
    private TransitionController _transitionController;

    /// <summary>
    /// Called "on select" to run the transition which, in turn, will call the `ExecuteQuit()` method when ending the transition
    /// </summary>
    public void QueueTransition()
    {
        if (_transitionController != null)
        {
            _transitionController.Transition();
        }
    }

    /// <summary>
    /// Call this method to quit the game after a transition
    /// </summary>
    public void ExecuteQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
