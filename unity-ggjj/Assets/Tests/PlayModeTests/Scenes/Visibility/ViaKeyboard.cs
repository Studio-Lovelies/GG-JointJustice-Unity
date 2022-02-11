using System.Collections;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.VisibilityTest
{
    public class ViaKeyboard
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        [UnityTest]
        [ReloadScene("Assets/Scenes/TestScenes/Visibility - TestScene.unity")]
        public IEnumerator RendererChangesVisibility()
        {
            var appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
            var arinSprite = TestTools.FindInactiveInSceneByName<Renderer>("Defense_Actor");
            var rossSprite = TestTools.FindInactiveInSceneByName<Renderer>("Witness_Actor");

            yield return _inputTestTools.ProgressStory(appearingDialogueController);
            yield return _inputTestTools.ProgressStory(appearingDialogueController);

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);

            yield return _inputTestTools.ProgressStory(appearingDialogueController);

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsFalse(rossSprite.enabled);

            yield return _inputTestTools.ProgressStory(appearingDialogueController);
            yield return _inputTestTools.ProgressStory(appearingDialogueController);
            yield return _inputTestTools.ProgressStory(appearingDialogueController);

            Assert.IsFalse(arinSprite.enabled);
            Assert.IsFalse(rossSprite.enabled);

            yield return _inputTestTools.ProgressStory(appearingDialogueController);
            yield return _inputTestTools.ProgressStory(appearingDialogueController);

            Assert.IsFalse(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);

            yield return _inputTestTools.ProgressStory(appearingDialogueController);

            Assert.IsTrue(arinSprite.enabled);
            Assert.IsTrue(rossSprite.enabled);
        }
    }
}