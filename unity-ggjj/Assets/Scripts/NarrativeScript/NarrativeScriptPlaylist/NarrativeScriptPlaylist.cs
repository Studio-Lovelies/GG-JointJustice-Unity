using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NarrativeScriptPlaylist : MonoBehaviour, INarrativeScriptPlaylist
{
    [SerializeField]
    public NarrativeGameState _narrativeGameState;
    
    [field: Tooltip("List of narrative scripts to be played in order")]
    [field: SerializeField] public NarrativeScript DefaultNarrativeScript { get; private set; }
    
    [field: Tooltip("List of narrative scripts to be used when taking penalties")]
    [field: SerializeField] public List<NarrativeScript> FailureScripts { get; private set; }
    
    [field: Tooltip("A narrative script to be played on game over")]
    [field: SerializeField] public NarrativeScript GameOverScript { get; private set; }

    /// <summary>
    /// Call the initialise method on all narrative scripts in this playlist
    /// </summary>
    public void InitializeNarrativeScripts(BGSceneList bgSceneList)
    {
        DefaultNarrativeScript.Initialize();

        FailureScripts.ForEach(script =>
        {
            script.Initialize();
            _narrativeGameState.BGSceneList.InstantiateBGScenes(script);
        }); 
        GameOverScript.Initialize();
        
        _narrativeGameState.BGSceneList.InstantiateBGScenes(DefaultNarrativeScript);
        _narrativeGameState.BGSceneList.InstantiateBGScenes(GameOverScript);
    }

    /// <summary>
    /// Gets a random failure state from the list.
    /// </summary>
    /// <returns>Inky dialogue script containing a failure sub-story.</returns>
    public NarrativeScript GetRandomFailureScript()
    {
        if (FailureScripts.Count <= 0)
        {
            throw new NotSupportedException("This playlist contains no failure scripts");
        }
        var failureScript = FailureScripts[Random.Range(0, FailureScripts.Count)];
        failureScript.Reset();
        return failureScript;
    }
}
