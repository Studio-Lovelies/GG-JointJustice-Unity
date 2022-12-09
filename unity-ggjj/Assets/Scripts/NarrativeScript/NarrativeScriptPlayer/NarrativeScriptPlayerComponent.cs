using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NarrativeScriptPlayerComponent : MonoBehaviour, INarrativeScriptPlayerComponent
{
    [SerializeField] private NarrativeGameState _narrativeGameState;
    
    private NarrativeScriptPlayer _narrativeScriptPlayer;

    public INarrativeScriptPlayer NarrativeScriptPlayer
    {
        get
        {
            return _narrativeScriptPlayer ??= new NarrativeScriptPlayer(_narrativeGameState)
            {
                ActiveNarrativeScript = _narrativeGameState.NarrativeScriptStorage.NarrativeScript
            };
        }
    }

    public bool Waiting
    {
        get => NarrativeScriptPlayer.Waiting;
        set => NarrativeScriptPlayer.Waiting = value;
    }

    /// <summary>
    /// Exposes NarrativeScriptPlayer's Continue method for use in UnityEvents
    /// </summary>
    public void Continue()
    {
        NarrativeScriptPlayer.Continue();
    }

    public void ToggleSpeedup(InputAction.CallbackContext inputCallbackContext)
    {
        NarrativeScriptPlayer.ToggleSpeedup(inputCallbackContext);
    }

    /// <summary>
    /// Exposes NarrativeScriptPlayer's TryPressWitness method for use in UnityEvents
    /// </summary>
    public void TryPressWitness()
    {
        NarrativeScriptPlayer.TryPressWitness();
    }

    /// <summary>
    /// Loads a narrative script using a given narrative script name,
    /// ending the current narrative script and
    /// continuing at the beginning of the loaded script
    /// </summary>
    /// <param name="narrativeScriptName">The name of the narrative script to load</param>
    public void LoadScript(string narrativeScriptName)
    {
        _narrativeGameState.EvidenceController.ClearCourtRecord();
        var narrativeScriptText = Resources.Load<TextAsset>($"InkDialogueScripts/{narrativeScriptName}");
        Debug.Assert(narrativeScriptText != null, $"Failed to load narrative script resource at 'InkDialogueScripts/{narrativeScriptName}'");
        
        var narrativeScript = new NarrativeScript(narrativeScriptText);
        NarrativeScriptPlayer.ActiveNarrativeScript = narrativeScript;
        _narrativeGameState.BGSceneList.ClearBGScenes();
        _narrativeGameState.BGSceneList.InstantiateBGScenes(narrativeScript);
        NarrativeScriptPlayer.Continue();
    }
}
