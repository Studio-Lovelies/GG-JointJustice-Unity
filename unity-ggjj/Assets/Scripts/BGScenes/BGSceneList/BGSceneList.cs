using System.Collections.Generic;
using UnityEngine;

public class BGSceneList : MonoBehaviour, IBGSceneList
{
    private readonly Dictionary<string, BGScene> _bgScenes = new Dictionary<string, BGScene>();
    private BGScene _activeScene;

    /// <summary>
    /// Instantiates BGScenes from a given narrative script and adds them to the dictionary
    /// </summary>
    /// <param name="narrativeScript">The narrative script to get BGScenes from</param>
    public void InstantiateBGScenes(INarrativeScript narrativeScript)
    {
        var backgroundScenes = narrativeScript.ObjectStorage.GetObjectsOfType<BGScene>();

        foreach (var bgScene in backgroundScenes)
        {
            if (_bgScenes.ContainsKey(bgScene.name))
            {
                continue;
            }

            var bgSceneClone = Instantiate(bgScene, transform);
            bgSceneClone.name = bgScene.name;
            bgSceneClone.gameObject.SetActive(false);
            _bgScenes.Add(bgScene.name, bgSceneClone);
        }
    }
    
    /// <summary>
    /// Destroys all BGScene objects and clears the BGScenes dictionary
    /// </summary>
    public void ClearBGScenes()
    {
        foreach (var bgScene in _bgScenes.Values)
        {
            Destroy(bgScene.gameObject);
        }
        _bgScenes.Clear();
    }

    /// <summary>
    /// Sets the new active scene based on the provided scene name.
    /// </summary>
    /// <param name="sceneName">name of the target scene</param>
    /// <returns>The new active scene. Can be null if an error occurred.</returns>
    public BGScene SetScene(SceneAssetName sceneName)
    {
        if (!_bgScenes.ContainsKey(sceneName))
        {
            Debug.LogError($"BGScene '{sceneName}' was not found in bg-scenes dictionary");
            return _activeScene;
        }

        BGScene targetScene = _bgScenes[sceneName];
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
