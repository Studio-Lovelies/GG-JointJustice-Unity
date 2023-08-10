using UnityEngine;

namespace SceneLoading
{
    public class GameLoader : MonoBehaviour
    {
        private const string GAME_SCENE_NAME = "Game";
        
        [field: SerializeField] public TextAsset NarrativeScriptTextAsset { get; set; }
        [SerializeField] private GameStartSettings _gameStartSettings;
            
        private SceneLoader _sceneLoader;

        private void Awake()
        {
            _sceneLoader = GetComponent<SceneLoader>();
        }

        public void StartGame()
        {
            _gameStartSettings.SetNarrativeScriptTextAsset(NarrativeScriptTextAsset);
            _sceneLoader.LoadScene(GAME_SCENE_NAME);
        }
    }
}
