using System.Collections;
using Tests.PlayModeTests.Tools;
using UnityEngine;
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
            yield return null;
            var keyboard = _inputTestTools.Keyboard;

            var dialogueController = Object.FindObjectOfType<DialogueController>();
            var arinSprite = TestTools.FindInactiveInSceneByName<Renderer>("Defense_Actor");
            var rossSprite = TestTools.FindInactiveInSceneByName<Renderer>("Witness_Actor");

            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);
            yield return _inputTestTools.PressForFrame(keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => arinSprite.enabled);
            yield return TestTools.WaitForState(() => rossSprite.enabled);

            yield return _inputTestTools.PressForFrame(keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => arinSprite.enabled);
            yield return TestTools.WaitForState(() => !rossSprite.enabled);

            yield return _inputTestTools.PressForFrame(keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return _inputTestTools.PressForFrame(keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return _inputTestTools.PressForFrame(keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => !arinSprite.enabled);
            yield return TestTools.WaitForState(() => !rossSprite.enabled);

            yield return _inputTestTools.PressForFrame(keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return _inputTestTools.PressForFrame(keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => !arinSprite.enabled);
            yield return TestTools.WaitForState(() => rossSprite.enabled);

            yield return _inputTestTools.PressForFrame(keyboard.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => arinSprite.enabled);
            yield return TestTools.WaitForState(() => rossSprite.enabled);
        }
    }
}