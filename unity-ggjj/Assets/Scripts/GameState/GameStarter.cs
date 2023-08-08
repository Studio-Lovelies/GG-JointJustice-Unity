using SceneLoading;
using UnityEngine;
using UnityEngine.Serialization;

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
            }
            else if (_gameStartSettings.NarrativeScriptTextAsset != null)
            {
                _narrativeGameState.NarrativeScriptStorage.NarrativeScript = new NarrativeScript(_gameStartSettings.NarrativeScriptTextAsset);
                _gameStartSettings.NarrativeScriptTextAsset = null;
                _narrativeGameState.StartGame();
            }
            else
            {
                Debug.LogWarning("No narrative script text asset assigned. Game will not start.");
            }
        }
    }
}
