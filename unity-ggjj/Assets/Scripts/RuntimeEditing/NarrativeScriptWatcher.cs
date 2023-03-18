using System.IO;
using UnityEngine;

namespace RuntimeEditing
{
    // Creates and watches a .ink file and restarts the game 
    public class NarrativeScriptWatcher : MonoBehaviour
    {
        [SerializeField]
        private NarrativeGameState narrativeGameState;
        private DebugControls _debugControls;
        private string _absolutePathToScript;
        private string _absolutePathToCompiledScript;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _fileHasChanged;

        private void Awake()
        {
            if (!Debug.isDebugBuild) 
            {
                Destroy(this.gameObject);
                return;
            }
        
            const string RELATIVE_SCRIPT_PATH = "/../DevKit/Script.ink";
            _absolutePathToScript = Path.GetFullPath(Application.dataPath + RELATIVE_SCRIPT_PATH);
            const string RELATIVE_COMPILATION_TARGET_PATH = "/../Temp/Script.ink.json";
            _absolutePathToCompiledScript = Path.GetFullPath(Application.dataPath + RELATIVE_COMPILATION_TARGET_PATH);
        
            narrativeGameState = GetComponent<NarrativeGameState>();
            _debugControls = new DebugControls();
            _debugControls.Enable();
            _debugControls.KeyboardMouse.ReloadScript.performed += _ =>
            {
                Debug.Log($"{nameof(NarrativeScriptWatcher)}: Explicit reload request");
                ReloadGame();
            };

            _debugControls.KeyboardMouse.OpenEditor.performed += _ =>
            {
                Debug.Log($"{nameof(NarrativeScriptWatcher)}: Opening '{_absolutePathToScript}'");
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = _absolutePathToScript;
                process.Start();
            };
        
            if (!File.Exists(_absolutePathToScript))
            {
                File.Create(_absolutePathToScript);
            }
        
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.Path = Directory.GetParent(_absolutePathToScript)!.FullName;
            _fileSystemWatcher.Filter = Path.GetFileName(_absolutePathToScript);
            _fileSystemWatcher.IncludeSubdirectories = false;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastWrite;
            _fileSystemWatcher.Changed += (sender, args) =>
            {
                _fileHasChanged = true;
            };
            _fileSystemWatcher.EnableRaisingEvents = true;
            Debug.Log($"{nameof(NarrativeScriptWatcher)}: Watching '{_absolutePathToScript}'...");
        }

        private void Update()
        {
            if (!_fileHasChanged)
            {
                return;
            }

            _fileHasChanged = false;
            Debug.Log($"{nameof(NarrativeScriptWatcher)}: Change detected: '{_absolutePathToScript}'");  
            ReloadGame();
        }

        private void ReloadGame()
        {
            if (!Directory.Exists(Application.dataPath + "/../Temp"))
            {
                Directory.CreateDirectory(Application.dataPath + "/../Temp");
            }

            Debug.Log($"{nameof(NarrativeScriptWatcher)}: Compiling debug script to '{_absolutePathToCompiledScript}'");
            var compiler = new Ink.Compiler(File.ReadAllText(_absolutePathToScript));
            var output = compiler.Compile().ToJson();
            File.WriteAllText(_absolutePathToCompiledScript, output);

            Debug.Log($"{nameof(NarrativeScriptWatcher)}: Restarting game...");
            var narrativeScript = new NarrativeScript(new TextAsset(File.ReadAllText(_absolutePathToCompiledScript)));
            narrativeGameState.ActorController.SetActiveSpeakerToNarrator();
            narrativeGameState.SceneController.FadeIn(0);
            narrativeGameState.NarrativeScriptStorage.NarrativeScript = narrativeScript;
            narrativeGameState.EvidenceController.ClearCourtRecord();
            narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript = narrativeScript;
            narrativeGameState.BGSceneList.ClearBGScenes();
            narrativeGameState.BGSceneList.InstantiateBGScenes(narrativeScript);
            narrativeGameState.SceneController.ReloadScene();
        }
    }
}
