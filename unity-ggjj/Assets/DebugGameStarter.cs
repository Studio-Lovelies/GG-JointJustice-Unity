using UnityEngine;

[RequireComponent(typeof(NarrativeGameState))]
public class DebugGameStarter : MonoBehaviour
{
    [SerializeField] private TextAsset _narrativeScript;
    
    private NarrativeGameState _narrativeGameState;
    
    private void Awake()
    {
        _narrativeGameState = GetComponent<NarrativeGameState>();
    }

    private void Start()
    {
        if (_narrativeScript == null) { return; }
        
        Debug.Log($"DebugGameStarter: Running script {_narrativeScript.name}");

        _narrativeGameState.NarrativeScriptStorage.NarrativeScript = new NarrativeScript(_narrativeScript);
        _narrativeGameState.StartGame();
    }
}
