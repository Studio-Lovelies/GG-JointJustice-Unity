using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.CrossExamination
{
    public class ViaKeyboard : StoryProgresser
    {
        private NarrativeScriptPlayerComponent _narrativeScriptPlayerComponent;

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            TestTools.StartGame("RossCoolX");
            _narrativeScriptPlayerComponent = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
        }

        [UnityTest]
        public IEnumerator CanPresentEvidenceDuringExamination()
        {
            yield return ProgressStory();
            EvidenceMenu evidenceMenu = Object.FindObjectOfType<EvidenceMenu>(true);
            yield return PressForFrame(Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return PressForFrame(Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
            Assert.IsTrue(_narrativeScriptPlayerComponent.NarrativeScriptPlayer.HasSubStory);
        }

        [UnityTest]
        public IEnumerator CantPresentEvidenceDuringExaminationDialogue()
        {
            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return ProgressStory();
            yield return WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return PressForFrame(Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
            Assert.IsTrue(_narrativeScriptPlayerComponent.NarrativeScriptPlayer.HasSubStory);

            yield return WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return PressForFrame(Keyboard.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
        }

        [UnityTest]
        public IEnumerator CantPresentEvidenceDuringPressingDialogue()
        { 
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            
            yield return ProgressStory();
            yield return PressForFrame(Keyboard.cKey);
            yield return TestTools.WaitForState(() => !narrativeScriptPlayer.NarrativeScriptPlayer.Waiting);

            EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return WaitForBehaviourActiveAndEnabled(evidenceMenu, Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return PressForFrame(Keyboard.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
        }

        [UnityTest]
        public IEnumerator GameOverPlaysOnNoLivesLeft()
        {
            var penaltyManager = Object.FindObjectOfType<PenaltyManager>();
            
            for (int i = penaltyManager.PenaltiesLeft; i > 0; i--)
            {
                yield return TestTools.WaitForState(() => _narrativeScriptPlayerComponent.NarrativeScriptPlayer.CanPressWitness);

                Assert.AreEqual(i, penaltyManager.PenaltiesLeft);
                yield return PressForFrame(Keyboard.zKey);
                yield return PressForFrame(Keyboard.enterKey);
                while (_narrativeScriptPlayerComponent.NarrativeScriptPlayer.HasSubStory && penaltyManager.PenaltiesLeft > 0)
                {
                    yield return ProgressStory();
                }

                Assert.AreEqual(i - 1, penaltyManager.PenaltiesLeft);
            }
            
            yield return new WaitForSeconds(5);
            
            Assert.IsTrue(new AssetName(_narrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript.Script.name).ToString() == new AssetName("TMPHGameOver").ToString());
        }
    }
}