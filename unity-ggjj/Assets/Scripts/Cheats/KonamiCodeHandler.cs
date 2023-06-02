namespace Cheats
{
    using UnityEngine;

    public class KonamiCodeHandler : MonoBehaviour
    {
        private static string NormalizeInput(string controlName) => controlName
                                                                        .ToUpper()
                                                                        .Replace("ARROW", "")
                                                                        .Replace("ENTER", "START");
        
        private readonly string[] _cheat = { "UP", "UP", "DOWN", "DOWN", "LEFT", "RIGHT", "LEFT", "RIGHT", "B", "A", "START" };
        
        [SerializeField] private SceneLoader sceneLoaderToChange;
        [SerializeField] private TextAsset newScene;
        
        private int _currentCheatIndex;
        private void Awake()
        {
            var cheatInput = new Controls();
            cheatInput.Enable();
            cheatInput.Keyboard.ValidKeys.performed += ctx =>
            {
                var pressedKey = NormalizeInput(ctx.control.name);
                
                if (_cheat[_currentCheatIndex] != pressedKey)
                {
                    if (_currentCheatIndex == 0)
                    {
                        return;
                    }
                    
                    _currentCheatIndex = 0;
                    return;
                }

                _currentCheatIndex++;
                if (_currentCheatIndex != _cheat.Length)
                {
                    return;
                }
                
                Debug.Log("[KONAMICODE] Cheat Activated!");
                sceneLoaderToChange.NarrativeScript = newScene;
            };
        }
    }

}