using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that contains methods for loading scenes.
/// ChangeScene has overloads for scene index and scene name.
/// LoadScene coroutine keeps track of the progress of the scene loading.
/// Unloads the current scene after new scene has loaded.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public bool Busy { get; private set; }

    [Tooltip("Assign a narrative script to play on scene load")]
    [SerializeField]
    private TextAsset _narrativeScript;
    
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
        if (Busy) { return; }

        _sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        if (_sceneLoadOperation == null)
        {
            throw new Exception("Scene failed to load");
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
        _sceneLoadOperation.allowSceneActivation = true;
        
        while (!_sceneLoadOperation.isDone)
        {
            yield return null;
        }

        if (_narrativeScript != null)
        {
            SetNarrativeScript();
        }
        
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    private void SetNarrativeScript()
    {
        var gameState = FindObjectOfType<NarrativeGameState>();
        gameState.NarrativeScriptPlaylist.NarrativeScript = new NarrativeScript(_narrativeScript);
        gameState.StartGame();
    }
}
