using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.CrossExamination
{
    public class ViaKeyboard
    {
        private readonly StoryProgresser _storyProgresser = new StoryProgresser();
        
        private NarrativeScriptPlayerComponent _narrativeScriptPlayerComponent;

        [SetUp]
        public void Setup()
        {
            _storyProgresser.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            _storyProgresser.TearDown();
        }

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
            yield return _storyProgresser.ProgressStory();
            var evidenceMenu = Object.FindObjectOfType<EvidenceMenu>(true);
            yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
            Assert.IsTrue(_narrativeScriptPlayerComponent.NarrativeScriptPlayer.HasSubStory);
        }

        [UnityTest]
        public IEnumerator CantPresentEvidenceDuringExaminationDialogue()
        {
            var evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.WaitForBehaviourActiveAndEnabled(evidenceMenu, _storyProgresser.Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
            Assert.IsTrue(_narrativeScriptPlayerComponent.NarrativeScriptPlayer.HasSubStory);

            yield return _storyProgresser.WaitForBehaviourActiveAndEnabled(evidenceMenu, _storyProgresser.Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
        }

        [UnityTest]
        public IEnumerator CantPresentEvidenceDuringPressingDialogue()
        { 
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            
            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.cKey);
            yield return TestTools.WaitForState(() => !narrativeScriptPlayer.NarrativeScriptPlayer.Waiting);

            var evidenceMenu = TestTools.FindInactiveInScene<EvidenceMenu>()[0];
            yield return _storyProgresser.WaitForBehaviourActiveAndEnabled(evidenceMenu, _storyProgresser.Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.enterKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
        }

        [UnityTest]
        public IEnumerator GameOverPlaysOnNoLivesLeft()
        {
            var penaltyManager = Object.FindObjectOfType<PenaltyManager>();
            
            for (var i = penaltyManager.PenaltiesLeft; i > 0; i--)
            {
                yield return TestTools.WaitForState(() => _narrativeScriptPlayerComponent.NarrativeScriptPlayer.CanPressWitness);

                Assert.AreEqual(i, penaltyManager.PenaltiesLeft);
                yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.zKey);
                yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.enterKey);
                while (_narrativeScriptPlayerComponent.NarrativeScriptPlayer.HasSubStory && penaltyManager.PenaltiesLeft > 0)
                {
                    yield return _storyProgresser.ProgressStory();
                }

                Assert.AreEqual(i - 1, penaltyManager.PenaltiesLeft);
            }
            
            yield return new WaitForSeconds(5);
            
            Assert.IsTrue(new AssetName(_narrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript.Script.name).ToString() == new AssetName("TMPHGameOver").ToString());
        }
    }
}