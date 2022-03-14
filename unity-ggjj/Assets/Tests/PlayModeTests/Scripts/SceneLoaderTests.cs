using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
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
            TestTools.SetField(sceneLoader, "_narrativeScript", Resources.Load<TextAsset>("InkDialogueScripts/RossCoolX"));
            sceneLoader.LoadScene("Game");
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "Game");
            var narrativeScriptPlaylist = Object.FindObjectOfType<NarrativeScriptPlaylist>();
            Assert.AreEqual("RossCoolX", narrativeScriptPlaylist.NarrativeScript.Script.name);
        }
    }
}