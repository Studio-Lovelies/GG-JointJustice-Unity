using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.TestTools;

namespace RuntimeEditing
{
    // Creates and watches a .ink file and restarts the game 
    public class FileSystemWatcher : MonoBehaviour
    {
        [SerializeField] private Texture2D DefaultBackgroundTexture;
        [SerializeField] private Texture2D DefaultForegroundTexture;
        [SerializeField] private GameObject SpriteEditorPrefab;
        
        [SerializeField]
        private NarrativeGameState narrativeGameState;
        private DebugControls _debugControls;

        // reloading is only supported in the main thread, so we need to set a flag and reload in the next frame
        private bool _hasChanges;

        // public to allow testing
        public string AbsolutePathToWatchedScript { get; private set; }

        private string _absolutePathToWatchedBackground;
        private string _absolutePathToWatchedForeground;

        private readonly List<System.IO.FileSystemWatcher> _filesystemWatchers = new();
        
        private readonly string _exampleScript = @"
                &SCENE:SpriteEditor
                &ACTOR:Arin
                Right, let's get this over with.

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
            // Live editing is only supported in debug builds
            if (!Debug.isDebugBuild)
            {
                Destroy(this.gameObject);
                return;
            }
            
            AbsolutePathToWatchedScript = Path.GetFullPath(Application.persistentDataPath + "/../DevKit/Script.ink");
            _absolutePathToWatchedBackground = Path.GetFullPath(Application.persistentDataPath + "/../DevKit/Background.png");
            _absolutePathToWatchedForeground = Path.GetFullPath(Application.persistentDataPath + "/../DevKit/Foreground.png");

            // Initialize and watch for changes in devkit folder and files
            if (!Directory.Exists(Path.GetDirectoryName(AbsolutePathToWatchedScript)!))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(AbsolutePathToWatchedScript)!);
            }
            _filesystemWatchers.Add(ListenForFileChange(AbsolutePathToWatchedScript, () =>
            {
                using var stream = File.Open(AbsolutePathToWatchedScript, FileMode.Create, FileAccess.Write);
                using var writer = new StreamWriter(stream);
                writer.Write(_exampleScript);
            }));
            _filesystemWatchers.Add(ListenForFileChange(_absolutePathToWatchedBackground, () =>
            {
                using var stream = File.Open(_absolutePathToWatchedBackground, FileMode.Create, FileAccess.Write);
                using var writer = new BinaryWriter(stream);
                writer.Write(DefaultBackgroundTexture.EncodeToPNG());
            }));
            _filesystemWatchers.Add(ListenForFileChange(_absolutePathToWatchedForeground, () =>
            {
                using var stream = File.Open(_absolutePathToWatchedForeground, FileMode.Create, FileAccess.Write);
                using var writer = new BinaryWriter(stream);
                writer.Write(DefaultForegroundTexture.EncodeToPNG());
            }));
            
            // listen for shortcut
            _debugControls = new DebugControls();
            _debugControls.Enable();
            _debugControls.KeyboardMouse.ReloadScript.performed += _ =>
            {
                Debug.Log($"{nameof(FileSystemWatcher)}: Explicit reload request");
                ReloadGame();
            };

            _debugControls.KeyboardMouse.OpenEditor.performed += OpenDevKit;
        }

        private System.IO.FileSystemWatcher ListenForFileChange(string path, Action initializeFile)
        {
            if (!File.Exists(path))
            {
                initializeFile();
            }

            var fileSystemWatcher = new System.IO.FileSystemWatcher();
            fileSystemWatcher.Path = Directory.GetParent(path)!.FullName;
            fileSystemWatcher.Filter = Path.GetFileName(path);
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastWrite;
            fileSystemWatcher.Changed += (_, _) =>
            {
                Debug.Log($"{nameof(FileSystemWatcher)}: Change detected: '{path}'");
                _hasChanges = true;
            };
            fileSystemWatcher.EnableRaisingEvents = true;
            Debug.Log($"{nameof(FileSystemWatcher)}: Watching '{path}'...");
            return fileSystemWatcher;
        }

        [ExcludeFromCoverage]
        private void OpenDevKit(InputAction.CallbackContext _)
        {
            Debug.Log($"{nameof(FileSystemWatcher)}: Opening '{AbsolutePathToWatchedScript}'");
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Directory.GetParent(AbsolutePathToWatchedScript)!.FullName;
            process.Start();
        }

        private void OnDestroy()
        {
            _debugControls?.Dispose();
            foreach (var fileSystemWatcher in _filesystemWatchers)
            {
                fileSystemWatcher.EnableRaisingEvents = false;
                fileSystemWatcher.Dispose();
            }
        }

        private void Update()
        {
            if (!_hasChanges)
            {
                return;
            }

            _hasChanges = false;
            ReloadGame();
        }

        private void ReloadGame()
        {
            Debug.Log($"{nameof(FileSystemWatcher)}: Restarting game...");

            if (!Directory.Exists(Directory.GetParent(AbsolutePathToWatchedScript)!.FullName))
            {
                Directory.CreateDirectory(Directory.GetParent(AbsolutePathToWatchedScript)!.FullName);
            }
            _filesystemWatchers.ForEach(watcher => watcher.EnableRaisingEvents = false);

            var narrativeScript = GetNarrativeScriptFromLocalFile();

            narrativeGameState.ActorController.SetActiveSpeakerToNarrator();
            narrativeGameState.NarrativeScriptStorage.NarrativeScript = narrativeScript;
            narrativeGameState.EvidenceController.ClearCourtRecord();
            narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript = narrativeScript;
            narrativeGameState.BGSceneList.ClearBGScenes();
            narrativeGameState.BGSceneList.InstantiateBGScenes(narrativeScript);

            SpriteEditorPrefab.transform.Find("Background").GetComponent<SpriteRenderer>().sprite = GetSpriteFromLocalFile(_absolutePathToWatchedBackground);
            SpriteEditorPrefab.transform.Find("Foreground").GetComponent<SpriteRenderer>().sprite = GetSpriteFromLocalFile(_absolutePathToWatchedForeground);
            narrativeGameState.SceneController.ReloadScene();
        }

        private NarrativeScript GetNarrativeScriptFromLocalFile()
        {
            using var watchedScriptStream = File.Open(AbsolutePathToWatchedScript, FileMode.Open, FileAccess.Read);
            using var watchedScriptReader = new StreamReader(watchedScriptStream);
            var compiler = new Ink.Compiler(watchedScriptReader.ReadToEnd());

            var narrativeScript = new NarrativeScript(new TextAsset(compiler.Compile().ToJson()));
            return narrativeScript;
        }

        private static Sprite GetSpriteFromLocalFile(string absolutePath)
        {
            var texture2dFrom = new Texture2D(320, 180, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            using var stream = File.Open(absolutePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(stream);
            texture2dFrom.LoadImage(reader.ReadBytes((int)stream.Length));

            return Sprite.Create(texture2dFrom, new Rect(0, 0, 320, 180), new Vector2(0.5f, 0.5f));
        }
    }
}
