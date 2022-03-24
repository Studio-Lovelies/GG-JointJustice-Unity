using System.Collections;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scenes.VisibilityTest
{
    public class ViaKeyboard : StoryProgresser
    {
        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
        }

        [UnityTest]
        public IEnumerator RendererChangesVisibility()
        {
            TestTools.StartGame("VisibilityTest");
            
            var arinSprite = TestTools.FindInactiveInSceneByName<Renderer>("Defense_Actor");
            var rossSprite = TestTools.FindInactiveInSceneByName<Renderer>("Witness_Actor");

            yield return ProgressStory();
            yield return ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);

            yield return ProgressStory();
            yield return ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsFalse(rossSprite.enabled);

            yield return ProgressStory();
            yield return ProgressStory();

            Assert.IsFalse(arinSprite.enabled);
            Assert.IsFalse(rossSprite.enabled);

            yield return ProgressStory();
            yield return ProgressStory();

            Assert.IsFalse(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);

            yield return ProgressStory();
            yield return ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);
        }
    }
}