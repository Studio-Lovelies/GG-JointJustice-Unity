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
    [SerializeField, Tooltip("The name of the scene to load")]
    private string _sceneName;

    [SerializeField, Tooltip("The index of the scene to load")]
    private int _sceneIndex;
    
    [SerializeField, Tooltip("Assign a transition controller here if a transition is required when changing the scene.")]
    private TransitionController _transitionController;

    [SerializeField, Tooltip("Assign a loading bar here if required")]
    private Slider _loadingBar;

    /// <summary>
    /// Call this method when wanting to change the scene using a specific scene's index.
    /// </summary>
    /// <param name="menuNavigator">The menu navigator used to call this method. It is passed in case it needs to be disabled</param>
    public void ChangeSceneBySceneIndex()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Additive);
        StartCoroutine(LoadScene(asyncOperation));
    }

    /// <summary>
    /// Call this method when wanting to change the scene using a specific scene's name.
    /// </summary>
    /// <param name="menuNavigator">The menu navigator used to call this method. It is passed in case it needs to be disabled</param>
    public void ChangeSceneBySceneName()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        StartCoroutine(LoadScene(asyncOperation));
    }
    
    /// <summary>
    /// Loads the next scene.
    /// If a transition controller is assign it will run the transition.
    /// If a loading bar is assigned it will update it with the current progress of the async operation.
    /// </summary>
    /// <param name="asyncOperation">The async operation tasked with loading the scene</param>
    private IEnumerator LoadScene(AsyncOperation asyncOperation)
    {
        if (_transitionController != null)
        {
            asyncOperation.allowSceneActivation = false;
            yield return StartCoroutine(_transitionController.Transition());
            asyncOperation.allowSceneActivation = true;
        }

        if (_loadingBar != null)
        {
            _loadingBar.gameObject.SetActive(true);
        }

        while (!asyncOperation.isDone)
        {
            if (_loadingBar != null)
            {
                _loadingBar.value = asyncOperation.progress;
            }
            yield return null;
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
