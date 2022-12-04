using System.Collections;
using Tests.PlayModeTests.Tools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.PlayModeTests.Suites.Scenes.VisibilityTest
{
    public class ViaKeyboard
    {
        private readonly StoryProgresser _storyProgresser = new StoryProgresser();

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
        }

        [UnityTest]
        public IEnumerator RendererChangesVisibility()
        {
            TestTools.StartGame("VisibilityTest");
            
            var arinSprite = TestTools.FindInactiveInSceneByName<Renderer>("Defense_Actor");
            var rossSprite = TestTools.FindInactiveInSceneByName<Renderer>("Witness_Actor");

            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);

            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsFalse(rossSprite.enabled);

            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();

            Assert.IsFalse(arinSprite.enabled);
            Assert.IsFalse(rossSprite.enabled);

            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();

            Assert.IsFalse(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);

            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);
        }
    }
}