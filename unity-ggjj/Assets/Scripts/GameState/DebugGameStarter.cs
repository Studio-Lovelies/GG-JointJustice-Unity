using UnityEngine;

[RequireComponent(typeof(NarrativeGameState))]
public class DebugGameStarter : MonoBehaviour
{
    public TextAsset narrativeScript;
    
    private NarrativeGameState _narrativeGameState;
    
    private void Awake()
    {
        _narrativeGameState = GetComponent<NarrativeGameState>();
    }

    private void Start()
    {
        if (narrativeScript == null) { return; }

        Debug.Log($"DebugGameStarter: Running script {narrativeScript.name}");

        _narrativeGameState.NarrativeScriptStorage.NarrativeScript = new NarrativeScript(narrativeScript);
        _narrativeGameState.StartGame();
    }
}
