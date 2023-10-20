using System;
using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using RuntimeEditing;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using FileSystemWatcher = RuntimeEditing.FileSystemWatcher;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts
{
    public class RuntimeEditingTests
    {
        private global::AppearingDialogueController _appearingDialogueController;
        private readonly StoryProgresser _storyProgresser = new();
        private Keyboard Keyboard => _storyProgresser.keyboard;
        
        private FileSystemWatcher _fileSystemWatcher;
        
        [SetUp]
        public void Setup()
        {
            _storyProgresser.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            _storyProgresser.TearDown();
        }

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");

            TestTools.StartGame("ActorControllerTestScript");
            
            _appearingDialogueController = Object.FindObjectOfType<global::AppearingDialogueController>();
            _fileSystemWatcher = Object.FindObjectOfType<FileSystemWatcher>();
        }

        [UnityTearDown]
        public void UnityTearDown()
        {
            _storyProgresser.TearDown();
        }

        [UnityTest]
        public IEnumerator SceneReloadCreatedScriptIfMissing()
        {
            Debug.Log($"Clearing {_fileSystemWatcher.AbsolutePathToWatchedScript}...");
            Directory.Delete(Path.GetDirectoryName(_fileSystemWatcher.AbsolutePathToWatchedScript)!, true);
            
            Assert.IsFalse(File.Exists(_fileSystemWatcher.AbsolutePathToWatchedScript));
            
            SceneManager.LoadScene("Game");
            yield return null;
            
            _appearingDialogueController = Object.FindObjectOfType<global::AppearingDialogueController>();
            _fileSystemWatcher = Object.FindObjectOfType<FileSystemWatcher>();

            TestTools.StartGame("ActorControllerTestScript");
            Assert.IsTrue(File.Exists(_fileSystemWatcher.AbsolutePathToWatchedScript));
        }

        [UnityTest]
        public IEnumerator FileChangeReloadsScene()
        {
            const string TEXT_CONTENT = @"
                &SCENE:TMPH_Court
                &SET_ACTOR_POSITION:Defense,Ross
                &SET_ACTOR_POSITION:Witness,Arin

                &JUMP_TO_POSITION:Defense
                &SPEAK:Arin
                I'm visible!";
            var normalizedTextContent = TEXT_CONTENT
                                                 .Replace("\n", Environment.NewLine)
                                                 .Replace("\r", Environment.NewLine)
                                                 .Replace(Environment.NewLine+ Environment.NewLine, Environment.NewLine);
            
            yield return _storyProgresser.ProgressStory();
            Assert.AreNotEqual(normalizedTextContent.Split(Environment.NewLine).Last(), _appearingDialogueController.Text);
            
            File.WriteAllText(_fileSystemWatcher.AbsolutePathToWatchedScript, normalizedTextContent);
            yield return TestTools.WaitForState(() => normalizedTextContent.Split(Environment.NewLine).Last().Trim() == Object.FindObjectOfType<global::AppearingDialogueController>().Text);
        }

        [UnityTest]
        public IEnumerator ManualKeyboardShortcutReloadsScene()
        {
            yield return _storyProgresser.ProgressStory();
            var textBeforeReload = _appearingDialogueController.Text;
            yield return _storyProgresser.PressForFrame(Keyboard.rKey);
            yield return TestTools.WaitForState(() => textBeforeReload != Object.FindObjectOfType<global::AppearingDialogueController>().Text);
        }
    }
}
