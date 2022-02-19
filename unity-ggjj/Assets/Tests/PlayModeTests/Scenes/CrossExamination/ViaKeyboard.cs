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
        private NarrativeScriptPlayerComponent _narrativeScriptPlayerComponent;
        private StoryProgresser _storyProgresser;

        private Keyboard Keyboard => _inputTestTools.Keyboard;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/CrossExamination - TestScene.unity", new LoadSceneParameters());
            _narrativeScriptPlayerComponent = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            _storyProgresser = new StoryProgresser();
        }
        
        [UnityTest]
        public IEnumerator CanPresentEvidenceDuringExamination()
        {
            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _storyProgresser.ProgressStory();
            yield return _inputTestTools.PressForFrame(Keyboard.zKey);
            yield return TestTools.WaitForState(() => evidenceMenu.isActiveAndEnabled);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
            yield return TestTools.WaitForState(() => _narrativeScriptPlayerComponent.HasSubStory);
            Assert.IsTrue(_narrativeScriptPlayerComponent.HasSubStory);
        }

        [UnityTest]
        public IEnumerator CantPresentEvidenceDuringExaminationDialogue()
        {
            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _storyProgresser.ProgressStory();
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
            Assert.IsTrue(_narrativeScriptPlayerComponent.HasSubStory);

            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
        }

        [UnityTest]
        public IEnumerator CantPresentEvidenceDuringPressingDialogue()
        {
            int existingSubstories = TestTools.FindInactiveInScene<NarrativeScriptPlayerComponent>().Count(controller => {
                GameObject gameObject = controller.gameObject;
                return gameObject.name.Contains("SubStory") && gameObject.activeInHierarchy;
            });

            yield return _inputTestTools.PressForFrame(Keyboard.xKey);
            AppearingDialogueController appearingDialogueController = TestTools.FindInactiveInScene<AppearingDialogueController>()[0];

            while (appearingDialogueController.IsPrintingText)
            {
                yield return _inputTestTools.WaitForRepaint();
            }

            yield return _inputTestTools.PressForFrame(Keyboard.cKey);

            while (appearingDialogueController.IsPrintingText)
            {
                yield return _inputTestTools.WaitForRepaint();
            }

            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);

            Assert.AreEqual(existingSubstories,  TestTools.FindInactiveInScene<NarrativeScriptPlayerComponent>().Count(controller => {
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
            var storyProgresser = new StoryProgresser();
            
            for (int i = penaltyManager.PenaltiesLeft; i > 0; i--)
            {
                yield return TestTools.WaitForState(() => _narrativeScriptPlayerComponent.CanPressWitness);

                Assert.AreEqual(i, penaltyManager.PenaltiesLeft);
                yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
                yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
                while (_narrativeScriptPlayerComponent.HasSubStory && penaltyManager.PenaltiesLeft > 0)
                {
                    yield return storyProgresser.ProgressStory();
                }

                Assert.AreEqual(i - 1, penaltyManager.PenaltiesLeft);
            }
            
            yield return new WaitForSeconds(5);
            
            Assert.IsTrue(new AssetName(_narrativeScriptPlayerComponent.ActiveNarrativeScript.Script.name).ToString() == new AssetName("TMPHGameOver").ToString());
        }
    }
}