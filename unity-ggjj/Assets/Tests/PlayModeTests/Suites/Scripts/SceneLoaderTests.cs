using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GameState;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SceneLoading;
using Tests.PlayModeTests.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Suites.Scripts
{
    public class SceneLoaderTests
    {
        private const string GAME_START_SETTINGS_PATH = "Assets/Scripts/SceneLoading/GameStartSettings.asset";
        private const string BLANK_SCENE = "Assets/Tests/PlayModeTests/TestScenes/BlankScene.unity";
        private const string ROSS_COOL_X = "Assets/Tests/PlayModeTests/TestScripts/RossCoolX.json";
        private const string GAME_SCENE_NAME = "Game";
        private const string MAIN_MENU_SCENE_NAME = "MainMenu";

        private SceneLoader _sceneLoader;
        private GameLoader _gameLoader;
        private GameStartSettings _gameStartSettings;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(BLANK_SCENE, new LoadSceneParameters());
            _sceneLoader = new GameObject().AddComponent<SceneLoader>();
            _gameLoader = _sceneLoader.gameObject.AddComponent<GameLoader>();
            _gameStartSettings = AssetDatabase.LoadAssetAtPath<GameStartSettings>(GAME_START_SETTINGS_PATH);
            Assert.IsNotNull(_gameStartSettings, "Could not load game start settings.");
            TestTools.SetField(_gameLoader, "_gameStartSettings", _gameStartSettings);
            _gameLoader.NarrativeScriptTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(ROSS_COOL_X);
        }

        [TearDown]
        public void TearDown()
        {
            _gameStartSettings.NarrativeScriptTextAsset = null;
        }
        
        [UnityTest]
        public IEnumerator AttemptingToLoadTwoScenesWhileBusyOnlyLoadsScene1()
        {
            const string SCENE1 = GAME_SCENE_NAME;
            const string SCENE2 = MAIN_MENU_SCENE_NAME;
            
            var sceneLoader = new GameObject().AddComponent<SceneLoader>();
            
            sceneLoader.LoadScene(SCENE1);
            sceneLoader.LoadScene(SCENE2);
            
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == SCENE1);
            var sceneNames = new List<string>(SceneManager.sceneCount);
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                sceneNames.Add(SceneManager.GetSceneAt(i).name);
            }
            Assert.True(sceneNames.Any(sceneName => sceneName == SCENE1));
            Assert.False(sceneNames.Any(sceneName => sceneName == SCENE2));
        }
        
        [UnityTest]
        public IEnumerator GameSceneCanBeLoadedWithNarrativeScriptWhenNarrativeGameStateIsPresent()
        {
            _gameLoader.StartGame();
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == GAME_SCENE_NAME);
            var narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            Assert.AreEqual("RossCoolX", narrativeGameState.NarrativeScriptStorage.NarrativeScript.Script.name);
        }

        [UnityTest]
        public IEnumerator DebugGameStarterGetsSkipped()
        {
            const string TEST_DEBUG_STRING_PATH = "Assets/Tests/PlayModeTests/TestScripts/DynamicAudio.json";
            
            _gameLoader.StartGame();
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == GAME_SCENE_NAME);
            var gameStarter = Object.FindObjectOfType<GameStarter>();
            Assert.IsNotNull(gameStarter, "The Game scene requires a GameStarter; if this should not be used, unset the narrative script inside it instead");
            var testDebugScript = AssetDatabase.LoadAssetAtPath<TextAsset>(TEST_DEBUG_STRING_PATH);
            Assert.NotNull(testDebugScript, "Specified test debug script could not be loaded.");
            TestTools.SetField(gameStarter, "_debugNarrativeScriptTextAsset", testDebugScript);
            var narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            Assert.AreEqual("RossCoolX", narrativeGameState.NarrativeScriptStorage.NarrativeScript.Script.name);
        }
    }
}