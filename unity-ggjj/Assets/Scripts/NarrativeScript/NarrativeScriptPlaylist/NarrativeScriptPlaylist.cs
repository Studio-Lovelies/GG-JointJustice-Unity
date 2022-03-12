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
    
    public List<NarrativeScript> FailureScripts { get; private set; }
    public NarrativeScript GameOverScript { get; set; }

    /// <summary>
    /// Call the initialise method on all narrative scripts in this playlist
    /// </summary>
    /// <param name="bgSceneList">A BGSceneList object with which to instantiate BGScenes used by narrative scripts</param>
    public void InitializeNarrativeScripts(BGSceneList bgSceneList)
    {
        if (DefaultNarrativeScript.Script != null)
        {
            DefaultNarrativeScript.Initialize();
            _narrativeGameState.BGSceneList.InstantiateBGScenes(DefaultNarrativeScript);
        }

        if (GameOverScript.Script != null)
        {
            GameOverScript.Initialize();
            _narrativeGameState.BGSceneList.InstantiateBGScenes(GameOverScript);
        }
        
        FailureScripts.ForEach(script =>
        {
            script.Initialize();
            _narrativeGameState.BGSceneList.InstantiateBGScenes(script);
        });
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

    /// <summary>
    /// Adds a failure script to the currently playing narrative script
    /// </summary>
    /// <param name="narrativeScript">The failure narrative script to add</param>
    public void AddFailureScript(NarrativeScript narrativeScript)
    {
        FailureScripts.Add(narrativeScript);
    }
}
