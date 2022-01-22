using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using static UnityEngine.GameObject;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.VisibilityTest
{
    public class ViaKeyboard
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        [UnityTest]
        [ReloadScene("Assets/Scenes/Visibility - TestScene.unity")]
        public IEnumerator RendererChangesVisibility()
        {

            yield return null;
            Keyboard key = _inputTestTools.Keyboard;

            var dialogueController = Object.FindObjectOfType<DialogueController>();
            var arinSprite = InputTestTools.FindInactiveInSceneByName<Renderer>("Defense_Actor");
            var rossSprite = InputTestTools.FindInactiveInSceneByName<Renderer>("Witness_Actor");

            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);
            yield return _inputTestTools.PressForFrame(key.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => arinSprite.enabled);
            yield return TestTools.WaitForState(() => rossSprite.enabled);

            yield return _inputTestTools.PressForFrame(key.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => arinSprite.enabled);
            yield return TestTools.WaitForState(() => !rossSprite.enabled);

            yield return _inputTestTools.PressForFrame(key.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return _inputTestTools.PressForFrame(key.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return _inputTestTools.PressForFrame(key.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => !arinSprite.enabled);
            yield return TestTools.WaitForState(() => !rossSprite.enabled);

            yield return _inputTestTools.PressForFrame(key.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return _inputTestTools.PressForFrame(key.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => !arinSprite.enabled);
            yield return TestTools.WaitForState(() => rossSprite.enabled);

            yield return _inputTestTools.PressForFrame(key.xKey);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

            yield return TestTools.WaitForState(() => arinSprite.enabled);
            yield return TestTools.WaitForState(() => rossSprite.enabled);
        }
    }
}