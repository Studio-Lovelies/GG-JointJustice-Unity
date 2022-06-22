using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that contains methods for loading scenes.
/// ChangeScene has overloads for scene index and scene name.
/// LoadScene coroutine keeps track of the progress of the scene loading.
/// Unloads the current scene after new scene has loaded.
/// </summary>
public class SceneLoader : MonoBehaviour, ISceneLoader
{
    public bool Busy { get; private set; }

    [field: Tooltip("Assign a narrative script to play on scene load")]
    [field: SerializeField]
    public TextAsset NarrativeScript { get; set; }

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

        if (EventSystem.current != null)
        {
            Destroy(EventSystem.current.gameObject);
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
            _sceneLoadOperation.allowSceneActivation = false;
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

        if (NarrativeScript != null)
        {
            SetNarrativeScript();
        }
        
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    /// <summary>
    /// Passes the narrative script of this SceneLoader instance to the NarrativeScriptStorage and starts the game
    /// </summary>
    private void SetNarrativeScript()
    {
        var gameState = FindObjectOfType<NarrativeGameState>();
        gameState.NarrativeScriptStorage.NarrativeScript = new NarrativeScript(NarrativeScript);
        gameState.StartGame();
    }
}
