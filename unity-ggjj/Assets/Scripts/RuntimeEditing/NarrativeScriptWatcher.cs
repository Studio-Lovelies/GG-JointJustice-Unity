using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace RuntimeEditing
{
    // Creates and watches a .ink file and restarts the game 
    public class NarrativeScriptWatcher : MonoBehaviour
    {
        [SerializeField]
        private NarrativeGameState narrativeGameState;
        public string AbsolutePathToWatchedScript { get; private set; }

        private DebugControls _debugControls;
        private string _absolutePathToCompiledScript;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _fileHasChanged;
        
        private readonly string _exampleScript = @"
                &SCENE:TMPH_Court
                &SET_ACTOR_POSITION:Defense,Arin
                &SET_ACTOR_POSITION:Witness,Ross

                &JUMP_TO_POSITION:Defense
                &SPEAK:Arin
                Okay, so you claim to have seen my client at the scene of the crime, and you base your entire testimony on a game of Mario Party?

                &JUMP_TO_POSITION:Witness
                &SPEAK:Ross
                Yes, that's correct.

                &JUMP_TO_POSITION:Defense
                &SPEAK:Arin
                And yet, you're the same person who made those incredibly difficult Mario Maker levels that almost broke me?

                &JUMP_TO_POSITION:Witness
                &SPEAK:Ross
                Uh, yeah? Also what does that have to do with anything?

                &JUMP_TO_POSITION:Defense
                &SPEAK:Arin
                Well, maybe look at what the writers of this script came up with!
                ...
                Absolutely nothing.
                Good.
                I am glad.".Replace("  ", "").Trim();

        private void Awake()
        {
            if (!Debug.isDebugBuild) 
            {
                Destroy(this.gameObject);
                return;
            }
        
            const string RELATIVE_SCRIPT_PATH = "/../DevKit/Script.ink";
            AbsolutePathToWatchedScript = Path.GetFullPath(Application.dataPath + RELATIVE_SCRIPT_PATH);
            const string RELATIVE_COMPILATION_TARGET_PATH = "/../Temp/Script.ink.json";
            _absolutePathToCompiledScript = Path.GetFullPath(Application.dataPath + RELATIVE_COMPILATION_TARGET_PATH);
        
            _debugControls = new DebugControls();
            _debugControls.Enable();
            _debugControls.KeyboardMouse.ReloadScript.performed += _ =>
            {
                Debug.Log($"{nameof(NarrativeScriptWatcher)}: Explicit reload request");
                ReloadGame();
            };

            _debugControls.KeyboardMouse.OpenEditor.performed += OpenTextEditorForScript;
        
            if (!File.Exists(AbsolutePathToWatchedScript))
            {
                if (!Directory.Exists(Path.GetDirectoryName(AbsolutePathToWatchedScript)!))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AbsolutePathToWatchedScript)!);
                }
                        
                using var stream = File.Open(AbsolutePathToWatchedScript, FileMode.Create, FileAccess.Write);
                using var writer = new StreamWriter(stream);
                writer.Write(_exampleScript);
            }
        
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.Path = Directory.GetParent(AbsolutePathToWatchedScript)!.FullName;
            _fileSystemWatcher.Filter = Path.GetFileName(AbsolutePathToWatchedScript);
            _fileSystemWatcher.IncludeSubdirectories = false;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastWrite;
            _fileSystemWatcher.Changed += (_, _) =>
            {
                _fileHasChanged = true;
            };
            _fileSystemWatcher.EnableRaisingEvents = true;
            Debug.Log($"{nameof(NarrativeScriptWatcher)}: Watching '{AbsolutePathToWatchedScript}'...");
        }

        [ExcludeFromCoverage]
        private void OpenTextEditorForScript(InputAction.CallbackContext _)
        {
            Debug.Log($"{nameof(NarrativeScriptWatcher)}: Opening '{AbsolutePathToWatchedScript}'");
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = AbsolutePathToWatchedScript;
            process.Start();
        }

        private void OnDestroy()
        {
            _debugControls?.Dispose();
            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher?.Dispose();
        }

        private void Update()
        {
            if (!_fileHasChanged)
            {
                return;
            }

            _fileHasChanged = false;
            Debug.Log($"{nameof(NarrativeScriptWatcher)}: Change detected: '{AbsolutePathToWatchedScript}'");  
            ReloadGame();
        }

        private void ReloadGame()
        {
            if (!Directory.Exists(Application.dataPath + "/../Temp"))
            {
                Directory.CreateDirectory(Application.dataPath + "/../Temp");
            }

            Debug.Log($"{nameof(NarrativeScriptWatcher)}: Compiling debug script to '{_absolutePathToCompiledScript}'");
            Ink.Compiler compiler;
            _fileSystemWatcher.EnableRaisingEvents = false;
            using (var stream = File.Open(AbsolutePathToWatchedScript, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    compiler = new Ink.Compiler(reader.ReadToEnd());
                    compiler.Compile();
                }
            }
            _fileSystemWatcher.EnableRaisingEvents = true;

            using (var stream = File.Open(_absolutePathToCompiledScript, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(compiler.Compile().ToJson());
                }
            }

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
