using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts
{
    public class SceneLoaderTests
    {
        [UnityTest]
        public IEnumerator GameSceneCanBeLoadedWithNarrativeScript()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/BlankScene.unity", new LoadSceneParameters());
            var sceneLoader = new GameObject().AddComponent<SceneLoader>();
            TestTools.SetField(sceneLoader, "_narrativeScript", AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Tests/PlayModeTests/TestScripts/RossCoolX.json"));
            sceneLoader.LoadScene("Game");
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "Game");
            var narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            Assert.AreEqual("RossCoolX", narrativeGameState.NarrativeScriptStorage.NarrativeScript.Script.name);
        }
    }
}