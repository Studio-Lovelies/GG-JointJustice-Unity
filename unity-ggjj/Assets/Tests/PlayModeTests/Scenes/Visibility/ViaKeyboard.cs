using System.Collections;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scenes.VisibilityTest
{
    public class ViaKeyboard
    {
        [UnityTest]
        [ReloadScene("Game")]
        public IEnumerator RendererChangesVisibility()
        {
            TestTools.StartGame("Assets/Tests/PlayModeTests/TestScripts/VisibilityTest.json");
            
            var storyProgresser = new StoryProgresser();
            
            var arinSprite = TestTools.FindInactiveInSceneByName<Renderer>("Defense_Actor");
            var rossSprite = TestTools.FindInactiveInSceneByName<Renderer>("Witness_Actor");

            yield return storyProgresser.ProgressStory();
            yield return storyProgresser.ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);

            yield return storyProgresser.ProgressStory();
            yield return storyProgresser.ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsFalse(rossSprite.enabled);

            yield return storyProgresser.ProgressStory();
            yield return storyProgresser.ProgressStory();

            Assert.IsFalse(arinSprite.enabled);
            Assert.IsFalse(rossSprite.enabled);

            yield return storyProgresser.ProgressStory();
            yield return storyProgresser.ProgressStory();

            Assert.IsFalse(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);

            yield return storyProgresser.ProgressStory();
            yield return storyProgresser.ProgressStory();

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);
        }
    }
}