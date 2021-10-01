using System.Collections.Generic;
using UnityEngine;

public class BGSceneList : MonoBehaviour
{
    private Dictionary<string, BGScene> _scenesInChildren;

    private BGScene _activeScene;

    /// <summary>
    /// Initializes the background scene dictionary
    /// </summary>
    private void Start()
    {
        _scenesInChildren = new Dictionary<string, BGScene>();
        foreach(BGScene scene in GetComponentsInChildren<BGScene>(true))
        {
            _scenesInChildren.Add(scene.gameObject.name, scene);
        }
    }

    /// <summary>
    /// Sets the new active scene based on the provided scene name.
    /// </summary>
    /// <param name="sceneName">name of the target scene</param>
    /// <returns>The new active scene. Can be null if an error occurred.</returns>
    public BGScene SetScene(string sceneName) //TODO: Change this when making file names universal
    {
        if (!_scenesInChildren.ContainsKey(sceneName))
        {
            Debug.LogError($"BGScene '{sceneName}' was not found in bg-scenes dictionary");
            return _activeScene;
        }

        BGScene targetScene = _scenesInChildren[sceneName];
        if (_activeScene != null && targetScene == _activeScene)
        {
            return _activeScene;
        }

        if (_activeScene != null)
        {
            _activeScene.gameObject.SetActive(false);
        }
        _activeScene = targetScene;
        targetScene.gameObject.SetActive(true);

        return _activeScene;
    }
}
