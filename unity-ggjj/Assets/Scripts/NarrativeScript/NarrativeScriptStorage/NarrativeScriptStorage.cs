using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NarrativeScriptStorage : INarrativeScriptStorage
{
    private readonly INarrativeGameState _narrativeGameState;
    
    public NarrativeScript NarrativeScript { get; set; }
    public List<NarrativeScript> FailureScripts { get; } = new List<NarrativeScript>();
    public NarrativeScript GameOverScript { get; set; }

    public NarrativeScriptStorage(INarrativeGameState narrativeGameState)
    {
        _narrativeGameState = narrativeGameState;
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
    /// Sets a game over script for the currently playing narrative script
    /// </summary>
    /// <param name="gameOverScriptName">The name of the game over script to set</param>
    public void SetGameOverScript(string gameOverScriptName)
    {
        GameOverScript = new NarrativeScript(Resources.Load<TextAsset>($"InkDialogueScripts/Failures/{gameOverScriptName}"));
        _narrativeGameState.BGSceneList.InstantiateBGScenes(GameOverScript);
    }
    
    /// <summary>
    /// Adds a failure script to the currently playing narrative script
    /// </summary>
    /// <param name="failureScriptName">The name of the failure narrative script to add</param>
    public void AddFailureScript(string failureScriptName)
    {
        var narrativeScript = new NarrativeScript(Resources.Load<TextAsset>($"InkDialogueScripts/Failures/{failureScriptName}"));
        FailureScripts.Add(narrativeScript);
        _narrativeGameState.BGSceneList.InstantiateBGScenes(narrativeScript);
    }
}
