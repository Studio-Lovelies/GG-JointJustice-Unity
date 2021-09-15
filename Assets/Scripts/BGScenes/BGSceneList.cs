using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BGSceneList : MonoBehaviour
{

    [ReadOnly] private Dictionary<string, BGScene> _scenesInChildren;

    private BGScene _activeScene;

    // Start is called before the first frame update
    void Start()
    {
        _scenesInChildren = new Dictionary<string, BGScene>();
        foreach(BGScene scene in GetComponentsInChildren<BGScene>(true))
        {
            Debug.Log(scene.gameObject.name);
            _scenesInChildren.Add(scene.gameObject.name, scene);
        }
    }

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
