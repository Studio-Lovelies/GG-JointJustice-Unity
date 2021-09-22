using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scenes.CrossExamination
{
    public class ViaKeyboard
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        [UnityTest]
        [ReloadScene("Assets/Scenes/CrossExamination - TestScene.unity")]
        public IEnumerator CanPresentEvidenceOnExamination()
        {
            yield return null;
            Keyboard key = _inputTestTools.Keyboard;

            yield return _inputTestTools.PressForFrame(key.xKey);

            EvidenceMenu evidenceMenu = Resources.FindObjectsOfTypeAll<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, key.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(key.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);

            Assert.AreNotEqual(0, Resources.FindObjectsOfTypeAll<DialogueController>().Count(controller => controller.gameObject.name.Contains("SubStory")));
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/CrossExamination - TestScene.unity")]
        public IEnumerator CantPresentEvidenceDuringExaminationDialogue()
        {
            yield return null;
            Keyboard key = _inputTestTools.Keyboard;

            yield return _inputTestTools.PressForFrame(key.xKey);

            EvidenceMenu evidenceMenu = Resources.FindObjectsOfTypeAll<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, key.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(key.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);

            int subStoryCount = Resources.FindObjectsOfTypeAll<DialogueController>().Count(controller => controller.gameObject.name.Contains("SubStory"));
            Assert.AreNotEqual(0, subStoryCount);

            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, key.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(key.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);

            Assert.AreEqual(Resources.FindObjectsOfTypeAll<DialogueController>().Count(controller => controller.gameObject.name.Contains("SubStory")), subStoryCount);
        }
    }
}