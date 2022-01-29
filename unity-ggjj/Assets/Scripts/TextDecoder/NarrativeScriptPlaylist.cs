using System.Collections.Generic;
using UnityEngine;

public class NarrativeScriptPlaylist : MonoBehaviour
{
    [field: Tooltip("List of narrative scripts to be played in order")]
    [field: SerializeField] public List<NarrativeScript> NarrativeScripts { get; private set; }
    
    [field: Tooltip("List of narrative scripts to be used when taking penalties")]
    [field: SerializeField] public List<NarrativeScript> FailureScripts { get; private set; }
    
    [field: Tooltip("A narrative script to be played on game over")]
    [field: SerializeField] public NarrativeScript GameOverScript { get; private set; }
    
    private int _narrativeScriptIndex = -1;

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Awake()
    {
        NarrativeScripts.ForEach(script => script.Initialize());
        FailureScripts.ForEach(script => script.Initialize());
        GameOverScript.Initialize();
    }

    /// <summary>
    /// Increments the narrative script index and returns the
    /// corresponding script in the narrative scripts array
    /// </summary>
    /// <returns>The next narrative script in the array</returns>
    public NarrativeScript GetNextNarrativeScript()
    {
        _narrativeScriptIndex++;
        if (_narrativeScriptIndex >= NarrativeScripts.Count)
        {
            throw new NotSupportedException("This playlist is on its last script already");
        }
        return NarrativeScripts[_narrativeScriptIndex];
    }
    
    /// <summary>
    /// Gets a random failure state from the list.
    /// </summary>
    /// <returns>Inky dialogue script containing a failure sub-story.</returns>
    public NarrativeScript GetRandomFailureScript()
    {
        return FailureScripts.Count == 0 ? null : FailureScripts[Random.Range(0, FailureScripts.Count)];
    }
}
