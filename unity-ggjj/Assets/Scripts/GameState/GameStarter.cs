using SceneLoading;
using UnityEngine;

namespace GameState
{
    [RequireComponent(typeof(NarrativeGameState))]
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameStartSettings _gameStartSettings;
        [Tooltip("Assign a narrative script text asset here to have it play on scene start"), SerializeField] private TextAsset _debugNarrativeScriptTextAsset;
    
        private NarrativeGameState _narrativeGameState;
    
        private void Awake()
        {
            _narrativeGameState = GetComponent<NarrativeGameState>();
        }

        private void Start()
        {
            if (_debugNarrativeScriptTextAsset != null)
            {
                Debug.Log($"DebugGameStarter: Running script {_debugNarrativeScriptTextAsset.name}");
                _narrativeGameState.NarrativeScriptStorage.NarrativeScript = new NarrativeScript(_debugNarrativeScriptTextAsset);
                _narrativeGameState.StartGame();
                return;
            }

            if (_gameStartSettings == null)
            {
                Debug.LogWarning("No debug narrative script text asset or GameStartSettings instance assigned. Game will not start.");
                return;
            }
                
            if (_gameStartSettings.IsNarrativeScriptTextAssetAssigned)
            {
                _narrativeGameState.NarrativeScriptStorage.NarrativeScript = new NarrativeScript(_gameStartSettings.GetAndClearNarrativeScriptTextAsset());
                _narrativeGameState.StartGame();
                return;
            }
            
            Debug.LogWarning("No narrative script text asset assigned to GameStartSettings instance. Game will not start.");
        }
    }
}
