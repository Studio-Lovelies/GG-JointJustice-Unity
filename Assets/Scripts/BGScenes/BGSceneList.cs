using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BGSceneList : MonoBehaviour
{

    private Dictionary<string, BGScene> _scenesInChildren;

    private BGScene _activeScene;

    /// <summary>
    /// Initalizes the background scene dictionary
    /// </summary>
    void Start()
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
    /// <returns>The new active scene. Can be null if an error ocurred.</returns>
    public BGScene SetScene(string sceneName) //TODO: Change this when making file names universal
    {
        if (_activeScene != null)
        {
            _activeScene.gameObject.SetActive(false);
            _activeScene = null;
        }

        try
        {
            _activeScene = _scenesInChildren[sceneName];
            _activeScene.gameObject.SetActive(true);
        }
        catch (KeyNotFoundException exception)
        {
            Debug.Log($"{exception.GetType().Name}: BG-Scene {sceneName} was not found in bg-scenes dictionary");
        }

        return _activeScene;
    }
}
