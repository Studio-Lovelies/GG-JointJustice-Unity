using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static UnityEngine.GameObject;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.CrossExamination
{
    public class ViaKeyboard
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        private NarrativeScriptPlayer _narrativeScriptPlayer;

        private Keyboard Keyboard => _inputTestTools.Keyboard;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/CrossExamination - TestScene.unity", new LoadSceneParameters());
            _narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayer>();
        }
        
        [UnityTest]
        public IEnumerator CanPresentEvidenceDuringExamination()
        {
            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _inputTestTools.PressForFrame(Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
            yield return TestTools.WaitForState(() => _narrativeScriptPlayer.HasSubStory);
            Assert.IsTrue(_narrativeScriptPlayer.HasSubStory);
        }

        [UnityTest]
        public IEnumerator CantPresentEvidenceDuringExaminationDialogue()
        {
            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
            Assert.IsTrue(_narrativeScriptPlayer.HasSubStory);

            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
        }

        [UnityTest]
        public IEnumerator CantPresentEvidenceDuringPressingDialogue()
        {
            int existingSubstories = TestTools.FindInactiveInScene<NarrativeScriptPlayer>().Count(controller => {
                GameObject gameObject = controller.gameObject;
                return gameObject.name.Contains("SubStory") && gameObject.activeInHierarchy;
            });

            yield return _inputTestTools.PressForFrame(Keyboard.xKey);
            AppearingDialogueController appearingDialogueController = TestTools.FindInactiveInScene<AppearingDialogueController>()[0];

            while (appearingDialogueController.PrintingText)
            {
                yield return _inputTestTools.WaitForRepaint();
            }

            yield return _inputTestTools.PressForFrame(Keyboard.cKey);

            while (appearingDialogueController.PrintingText)
            {
                yield return _inputTestTools.WaitForRepaint();
            }

            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);

            Assert.AreEqual(existingSubstories,  TestTools.FindInactiveInScene<NarrativeScriptPlayer>().Count(controller => {
                GameObject gameObject = controller.gameObject;
                return gameObject.name.Contains("SubStory") && gameObject.activeInHierarchy;
            }));
            
            Object.Destroy(Find("SubStory(Clone)"));
        }

        [UnityTest]
        public IEnumerator GameOverPlaysOnNoLivesLeft()
        {
            var penaltyManager = Object.FindObjectOfType<PenaltyManager>();
            var appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
            
            for (int i = penaltyManager.PenaltiesLeft; i > 0; i--)
            {
                yield return TestTools.WaitForState(() => _narrativeScriptPlayer.CanPressWitness);

                Assert.AreEqual(i, penaltyManager.PenaltiesLeft);
                yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
                yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
                while (_narrativeScriptPlayer.HasSubStory && penaltyManager.PenaltiesLeft > 0)
                {
                    yield return _inputTestTools.ProgressStory(appearingDialogueController);
                }

                Assert.AreEqual(i - 1, penaltyManager.PenaltiesLeft);
            }
            
            yield return new WaitForSeconds(5);
            
            Assert.IsTrue(new AssetName(_narrativeScriptPlayer.ActiveNarrativeScript.Script.name).ToString() == new AssetName("TMPHGameOver").ToString());
        }
    }
}