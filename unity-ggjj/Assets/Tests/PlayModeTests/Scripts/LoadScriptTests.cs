using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts
{
    public class LoadScriptTests
    {
        private StoryProgresser _storyProgresser;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            TestTools.StartGame("LoadScriptTest");
            _storyProgresser = new StoryProgresser();
        }
        
        [UnityTest]
        public IEnumerator NarrativeScriptsCanBeLoaded()
        {
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayerComponent>().NarrativeScriptPlayer;
            Assert.AreEqual("LoadScriptTest", narrativeScriptPlayer.ActiveNarrativeScript.Script.name);
            yield return _storyProgresser.ProgressStory();
            Assert.AreEqual("TMPHFAIL1", narrativeScriptPlayer.ActiveNarrativeScript.Script.name);
        }

        [UnityTest]
        public IEnumerator BGScenesAreDestroyedAndCreatedOnScriptLoad()
        {
            var bgSceneListTransform = Object.FindObjectOfType<BGSceneList>().transform;
            Assert.AreEqual(1, bgSceneListTransform.childCount);
            Assert.AreEqual("TMPHLobby", bgSceneListTransform.GetChild(0).name);
            yield return _storyProgresser.ProgressStory();
            Assert.AreEqual(3, bgSceneListTransform.childCount);
            var children = bgSceneListTransform.Cast<Transform>().ToList();
            Assert.IsTrue(children.Any(bgScene => bgScene.gameObject.name == "TMPHAssistant"));
            Assert.IsTrue(children.Any(bgScene => bgScene.gameObject.name == "TMPHCourt"));
            Assert.IsTrue(children.Any(bgScene => bgScene.gameObject.name == "TMPHJudge"));
        }
    }
}
