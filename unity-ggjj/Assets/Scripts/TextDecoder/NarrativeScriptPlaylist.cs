using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class NarrativeScriptPlaylist : MonoBehaviour
{
    [field: Tooltip("List of inky dialogue scripts to be played in order")]
    [field: SerializeField] public List<NarrativeScript> NarrativeScripts { get; private set; }
    
    [Tooltip("Write the name of the next scene to load here.")]
    [SerializeField] private string _nextSceneName;

    private SceneLoader _sceneLoader;
    private int _narrativeScriptIndex = -1;

    public NarrativeScript NarrativeScript => NarrativeScripts[_narrativeScriptIndex];

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Awake()
    {
        _sceneLoader = GetComponent<SceneLoader>();
        foreach (var narrativeScript in NarrativeScripts)
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
        if (_narrativeScriptIndex < NarrativeScripts.Count)
        {
            return NarrativeScripts[_narrativeScriptIndex];
        }

        if (!string.IsNullOrEmpty(_nextSceneName))
        {
            _sceneLoader.LoadScene(_nextSceneName);
        }

        return null;
    }
}
