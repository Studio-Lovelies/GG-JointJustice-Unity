using System.Collections;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.VisibilityTest
{
    public class ViaKeyboard : InputTest
    {
        [UnityTest]
        [ReloadScene("Assets/Scenes/TestScenes/Visibility - TestScene.unity")]
        public IEnumerator RendererChangesVisibility()
        {
            var storyProgresser = new StoryProgresser(this);
            
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