using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts
{
    public class SceneLoaderTests
    {
        private const string BLANK_SCENE = "Assets/Tests/PlayModeTests/TestScenes/BlankScene.unity";
        private const string ROSS_COOL_X = "Assets/Tests/PlayModeTests/TestScripts/RossCoolX.json";
        private const string GAME_SCENE_NAME = "Game";
        private const string MAIN_MENU_SCENE_NAME = "MainMenu";
        
        [UnityTest]
        public IEnumerator AttemptingToLoadTwoScenesWhileBusyOnlyLoadsScene1()
        {
            const string SCENE1 = GAME_SCENE_NAME;
            const string SCENE2 = MAIN_MENU_SCENE_NAME;
            
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(BLANK_SCENE, new LoadSceneParameters());
            var sceneLoader = new GameObject().AddComponent<SceneLoader>();
            sceneLoader.NarrativeScript = AssetDatabase.LoadAssetAtPath<TextAsset>(ROSS_COOL_X);
            
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
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(BLANK_SCENE, new LoadSceneParameters());
            var sceneLoader = new GameObject().AddComponent<SceneLoader>();
            sceneLoader.NarrativeScript = AssetDatabase.LoadAssetAtPath<TextAsset>(ROSS_COOL_X);
            
            sceneLoader.LoadScene(GAME_SCENE_NAME);
            
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == GAME_SCENE_NAME);
            var narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            Assert.AreEqual("RossCoolX", narrativeGameState.NarrativeScriptStorage.NarrativeScript.Script.name);
        }
        
        [UnityTest]
        public IEnumerator ThrowsExceptionWithoutNarrativeGameStateComponentWhenNeedingToSetNarrativeScript()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(BLANK_SCENE, new LoadSceneParameters());
            var sceneLoader = new GameObject().AddComponent<SceneLoader>();
            sceneLoader.NarrativeScript = AssetDatabase.LoadAssetAtPath<TextAsset>(ROSS_COOL_X);

            sceneLoader.LoadScene(MAIN_MENU_SCENE_NAME);
            
            yield return TestTools.WaitForState(() =>
            {
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    if (scene.name != MAIN_MENU_SCENE_NAME)
                    {
                        continue;
                    }

                    if (!scene.isLoaded)
                    {
                        continue;
                    }

                    return true;
                }

                return false;
            });
            LogAssert.Expect(LogType.Exception, new Regex($"{nameof(NullReferenceException)}: "));
        }
        
        [UnityTest]
        public IEnumerator SucceedsWithoutNarrativeGameStateIfNoNarrativeScriptIsSet()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(BLANK_SCENE, new LoadSceneParameters());
            var sceneLoader = new GameObject().AddComponent<SceneLoader>();
            sceneLoader.NarrativeScript = null;
            
            sceneLoader.LoadScene(MAIN_MENU_SCENE_NAME);
            
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == MAIN_MENU_SCENE_NAME);
        }

        [UnityTest]
        public IEnumerator DebugGameStarterGetsSkipped()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(BLANK_SCENE, new LoadSceneParameters());
            var sceneLoader = new GameObject().AddComponent<SceneLoader>();
            sceneLoader.NarrativeScript = AssetDatabase.LoadAssetAtPath<TextAsset>(ROSS_COOL_X);

            var debugGameStarterWasSetUp = false;

            void OnSceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= OnSceneManagerOnsceneLoaded;
                if (scene.name != GAME_SCENE_NAME)
                {
                    return;
                }

                DebugGameStarter debugGameStarter = null;
                foreach (var rootGameObject in scene.GetRootGameObjects())
                {
                    debugGameStarter = rootGameObject.GetComponent<DebugGameStarter>();
                    if (debugGameStarter != null)
                    {
                        break;
                    }
                }

                Assert.IsNotNull(debugGameStarter, "The Game scene requires a DebugGameStarter; if this should not be used, unset the narrative script inside it instead");
                debugGameStarter.GetComponent<DebugGameStarter>().narrativeScript = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Tests/PlayModeTests/TestScripts/DynamicAudio.json");

                debugGameStarterWasSetUp = true;
            }

            SceneManager.sceneLoaded += OnSceneManagerOnsceneLoaded;
                
            sceneLoader.LoadScene(GAME_SCENE_NAME);
            
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == GAME_SCENE_NAME);
            Assert.IsTrue(debugGameStarterWasSetUp);
            var narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            Assert.AreEqual("RossCoolX", narrativeGameState.NarrativeScriptStorage.NarrativeScript.Script.name);
        }
    }
}