using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that contains methods for loading scenes.
/// ChangeScene has overloads for scene index and scene name.
/// LoadScene coroutine keeps track of the progress of the scene loading.
/// Unloads the current scene after new scene has loaded.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public bool Busy { get; private set; }

    [SerializeField, Tooltip("Assign a loading bar here if required")]
    private Slider _loadingBar;

    private ITransition _transition;
    private AsyncOperation _sceneLoadOperation;

    private void Awake()
    {
        _transition = GetComponent<ITransition>();
    }

    /// <summary>
    /// Call this method when wanting to change the scene using a specific scene's name.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (Busy)
        {
            return;
        }

        _sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (_sceneLoadOperation == null)
        {
            return;
        }

        Busy = true;
        Transition();
    }

    /// <summary>
    /// Check if there is a _transitionController and call its transition method.
    /// Otherwise load the next scene.
    /// </summary>
    private void Transition()
    {
        Debug.Log("Transitioning");
        if (_transition != null)
        {
            if (_sceneLoadOperation != null)
            {
                _sceneLoadOperation.allowSceneActivation = false;
            }
            _transition.Transition();
            return;
        }
        LoadSceneCallback();
    }

    /// <summary>
    /// Called by a transition controller to load the next scene after a transition.
    /// </summary>
    public void LoadSceneCallback()
    {
        StartCoroutine(LoadSceneCoroutine());
    }
    
    /// <summary>
    /// Loads the next scene.
    /// If a loading bar is assigned it will update it with the current progress of the async operation.
    /// </summary>
    private IEnumerator LoadSceneCoroutine()
    {
        if (_sceneLoadOperation != null)
        {
            _sceneLoadOperation.allowSceneActivation = true;
        }

        yield return null; // Don't show loading bar if it loads in one frame

        if (_sceneLoadOperation != null)
        {
            while (!_sceneLoadOperation.isDone)
            {
                if (_loadingBar != null)
                {
                    if (!_loadingBar.gameObject.activeInHierarchy)
                    {
                        _loadingBar.gameObject.SetActive(true);
                    }

                    _loadingBar.value = _sceneLoadOperation.progress;
                }

                yield return null;
            }

            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
    }
}
