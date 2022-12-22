using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that contains methods for loading scenes.
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

    public void OnDisable()
    {
        if (_sceneLoadOperation == null)
        {
            return;
        }

        SceneManager.sceneLoaded -= SetupNextNarrativeScript;
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
    /// <remarks>
    /// This wrapper method is required to control SceneLoader instances via UnityEvents mapped inside the editor
    /// </remarks>
    public void LoadSceneCallback()
    {
        StartCoroutine(LoadSceneCoroutine());
    }
    
    /// <summary>
    /// Loads the next scene.
    /// </summary>
    private IEnumerator LoadSceneCoroutine()
    {
        _sceneLoadOperation.allowSceneActivation = true;

        SceneManager.sceneLoaded += SetupNextNarrativeScript;

        while (!_sceneLoadOperation.isDone)
        {
            // TODO: If a loading bar is assigned, update it with `_sceneLoadOperation.progress`
            yield return null;
        }
    }

    /// <summary>
    /// Callback ran when the new scene is loaded
    /// </summary>
    private void SetupNextNarrativeScript(Scene scene, LoadSceneMode mode)
    {
        // If not just a scene but also a specific narrative script is requested...
        if (NarrativeScript == null)
        {
            // If not, just clean up the old scene
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            return;
        }

        // ...make sure no DebugGameStarter can take precedence...
        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            var debugGameStarter = rootGameObject.GetComponent<DebugGameStarter>();
            if (debugGameStarter == null)
            {
                continue;
            }

            if (debugGameStarter.narrativeScript == null)
            {
                continue;
            }
            
            Debug.LogWarning($"The scene '{scene.name}' loaded by this {nameof(SceneLoader)} contained a '{nameof(DebugGameStarter)}'. As a {nameof(NarrativeScript)} was specified when loading this scene, this needs to be removed, otherwise '{debugGameStarter.narrativeScript.name}' (the {nameof(NarrativeScript)} specified inside the {nameof(DebugGameStarter)}) will incorrectly take precedence over '{NarrativeScript.name}' (the {nameof(NarrativeScript)} specified inside the {nameof(SceneLoader)})");
            Destroy(debugGameStarter);
        }

        // ...find the NarrativeGameState instance...
        NarrativeGameState narrativeGameStateInNewScene = null;
        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            narrativeGameStateInNewScene = rootGameObject.GetComponent<NarrativeGameState>();
            if (narrativeGameStateInNewScene != null)
            {
                break;
            }
        }
        
        if (narrativeGameStateInNewScene == null)
        {
            throw new NullReferenceException($"The newly loaded scene '{scene.name}' has no root object containing a '{nameof(NarrativeGameState)}'-component\r\nThis is required to initialize the next '{nameof(NarrativeScript)}'");
        }

        // ...assign the requested narrative script to it, start the scene...
        narrativeGameStateInNewScene.NarrativeScriptStorage.NarrativeScript = new NarrativeScript(NarrativeScript);
        narrativeGameStateInNewScene.StartGame();

        // ...and finally clean up the old scene
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
