using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class NarrativeScriptPlaylist : MonoBehaviour
{
    [Tooltip("List of inky dialogue scripts to be played in order")]
    [SerializeField] private List<NarrativeScript> _narrativeScripts;
    
    [Tooltip("Write the name of the next scene to load here.")]
    [SerializeField] private string _nextSceneName;

    private SceneLoader _sceneLoader;
    private int _narrativeScriptIndex = -1;

    public NarrativeScript NarrativeScript => _narrativeScripts[_narrativeScriptIndex];

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Awake()
    {
        _sceneLoader = GetComponent<SceneLoader>();
        foreach (var narrativeScript in _narrativeScripts)
        {
            narrativeScript.Initialize();
        }
    }

    /// <summary>
    /// Increments the narrative script index and returns the
    /// corresponding script in the narrative scripts array
    /// </summary>
    /// <returns>The next narrative script in the array</returns>
    public NarrativeScript GetNextNarrativeScript()
    {
        _narrativeScriptIndex++;
        if (_narrativeScriptIndex < _narrativeScripts.Count)
        {
            return _narrativeScripts[_narrativeScriptIndex];
        }
        
        _sceneLoader.LoadScene(_nextSceneName);
        return null;
    }
}
