using System.Collections;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField, Tooltip("Assign a transition controller here if a transition is required when changing the scene.")]
    private TransitionController _transitionController;

    /// <summary>
    /// Call this method to quit the game after a transition
    /// </summary>
    public void QueueTransitionAndQuit()
    {
        _transitionController.Transition();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
