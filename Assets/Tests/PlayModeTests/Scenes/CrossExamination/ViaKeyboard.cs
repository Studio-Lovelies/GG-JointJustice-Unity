using System.Collections;
using System.Linq;
using System.Reflection;
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
        public IEnumerator CanPresentEvidenceDuringExamination()
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
            Assert.True(evidenceMenu.isActiveAndEnabled);

            Assert.AreEqual(subStoryCount, Resources.FindObjectsOfTypeAll<global::DialogueController>().Count(controller => controller.gameObject.name.Contains("SubStory")));
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/CrossExamination - TestScene.unity")]
        public IEnumerator CantPresentEvidenceDuringPressingDialogue()
        {
            yield return null;
            Keyboard key = _inputTestTools.Keyboard;

            int existingSubstories = Resources.FindObjectsOfTypeAll<DialogueController>().Count(controller => {
                GameObject gameObject = controller.gameObject;
                return gameObject.name.Contains("SubStory") && gameObject.activeInHierarchy;
            });

            yield return _inputTestTools.PressForFrame(key.xKey);
            AppearingDialogueController appearingDialogueController = Resources.FindObjectsOfTypeAll<AppearingDialogueController>()[0];
            FieldInfo writingDialogueField = appearingDialogueController.GetType().GetField("_writingDialog", BindingFlags.NonPublic | BindingFlags.Instance);
            if (writingDialogueField is null) // needed to satisfy Intellisense's "possible NullReferenceException" in line below conditional
            {
                Assert.IsNotNull(writingDialogueField);
            }

            while ((bool)writingDialogueField.GetValue(appearingDialogueController))
            {
                yield return _inputTestTools.WaitForRepaint();
            }

            yield return _inputTestTools.PressForFrame(key.cKey);
            yield return _inputTestTools.PressForFrame(key.xKey);

            while ((bool)writingDialogueField.GetValue(appearingDialogueController))
            {
                yield return _inputTestTools.WaitForRepaint();
            }

            EvidenceMenu evidenceMenu = Resources.FindObjectsOfTypeAll<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, key.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(key.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);

            Assert.AreEqual(existingSubstories, Resources.FindObjectsOfTypeAll<DialogueController>().Count(controller => {
                GameObject gameObject = controller.gameObject;
                return gameObject.name.Contains("SubStory") && gameObject.activeInHierarchy;
            }));
        }
    }
}