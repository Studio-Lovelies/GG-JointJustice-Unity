using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using static UnityEngine.GameObject;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.CrossExamination
{
    public class ViaKeyboard
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        [UnityTest]
        [ReloadScene("Assets/Scenes/TestScenes/CrossExamination - TestScene.unity")]
        public IEnumerator CanPresentEvidenceDuringExamination()
        {
            yield return null;
            Keyboard key = _inputTestTools.Keyboard;

            yield return _inputTestTools.PressForFrame(key.xKey);
            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, key.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(key.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);

            Assert.AreNotEqual(0,  TestTools.FindInactiveInScene<DialogueController>().Count(controller => controller.gameObject.name.Contains("SubStory")));
            
            Object.Destroy(Find("SubStory(Clone)"));
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/TestScenes/CrossExamination - TestScene.unity")]
        public IEnumerator CantPresentEvidenceDuringExaminationDialogue()
        {
            yield return null;
            Keyboard key = _inputTestTools.Keyboard;

            yield return _inputTestTools.PressForFrame(key.xKey);

            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, key.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(key.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);

            int subStoryCount = TestTools.FindInactiveInScene<DialogueController>().Count(controller => controller.gameObject.name.Contains("SubStory"));
            Assert.AreNotEqual(0, subStoryCount);

            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, key.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(key.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            Assert.AreEqual(subStoryCount,  TestTools.FindInactiveInScene<DialogueController>().Count(controller => controller.gameObject.name.Contains("SubStory")));

            Object.Destroy(Find("SubStory(Clone)"));
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/TestScenes/CrossExamination - TestScene.unity")]
        public IEnumerator CantPresentEvidenceDuringPressingDialogue()
        {
            yield return null;
            Keyboard key = _inputTestTools.Keyboard;

            int existingSubstories = TestTools.FindInactiveInScene<DialogueController>().Count(controller => {
                GameObject gameObject = controller.gameObject;
                return gameObject.name.Contains("SubStory") && gameObject.activeInHierarchy;
            });

            yield return _inputTestTools.PressForFrame(key.xKey);
            AppearingDialogueController appearingDialogueController = TestTools.FindInactiveInScene<AppearingDialogueController>()[0];

            while (appearingDialogueController.PrintingText)
            {
                yield return _inputTestTools.WaitForRepaint();
            }

            yield return _inputTestTools.PressForFrame(key.cKey);

            while (appearingDialogueController.PrintingText)
            {
                yield return _inputTestTools.WaitForRepaint();
            }

            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, key.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(key.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);

            Assert.AreEqual(existingSubstories,  TestTools.FindInactiveInScene<DialogueController>().Count(controller => {
                GameObject gameObject = controller.gameObject;
                return gameObject.name.Contains("SubStory") && gameObject.activeInHierarchy;
            }));
            
            Object.Destroy(Find("SubStory(Clone)"));
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/TestScenes/CrossExamination - TestScene.unity")]
        public IEnumerator GameOverPlaysOnNoLivesLeft()
        {
            var penaltyManager = Object.FindObjectOfType<PenaltyManager>();
            var dialogueController = Object.FindObjectOfType<DialogueController>();
            
            for (int i = penaltyManager.PenaltiesLeft; i > 0; i--)
            {
                yield return TestTools.WaitForState(() => !dialogueController.IsBusy);

                Assert.AreEqual(i, penaltyManager.PenaltiesLeft);
                yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
                yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
                var subStory = Find("SubStory(Clone)");
                while (subStory != null && penaltyManager.PenaltiesLeft > 0)
                {
                    yield return _inputTestTools.ProgressStory(dialogueController);
                }

                Assert.AreEqual(i - 1, penaltyManager.PenaltiesLeft);
            }

            var dialogueControllers = Object.FindObjectsOfType<DialogueController>();
            Assert.IsTrue(dialogueControllers.Any(controller => controller.ActiveNarrativeScript.Name == "TMPH_GameOver"));
        }
    }
}