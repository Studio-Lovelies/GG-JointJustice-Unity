using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts
{
    public class PenaltyManagerTests
    {
        private PenaltyManager _penaltyManager;
        private readonly StoryProgresser _storyProgresser = new StoryProgresser();

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
            TestTools.StartGame("PenaltyTest");
            _penaltyManager = Object.FindObjectOfType<PenaltyManager>();
        }

        [UnityTest]
        public IEnumerator PenaltiesAreEnabledOnCrossExaminationStart()
        {
            Assert.IsTrue(!_penaltyManager.isActiveAndEnabled || _penaltyManager.PenaltiesLeft == 0);
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
            Assert.IsTrue(_penaltyManager.isActiveAndEnabled);
        }
    
        [UnityTest]
        public IEnumerator PenaltiesAreDisabledOnCrossExaminationEnd()
        {
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
            Assert.IsTrue(_penaltyManager.isActiveAndEnabled);
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
            Assert.IsFalse(_penaltyManager.isActiveAndEnabled);
        }
    
        [UnityTest]
        public IEnumerator NumberOfPenaltiesCanBeReset()
        {
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            for (int i = 0; i < 3; i++)
            {
                yield return _storyProgresser.ProgressStory();
            }
            
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.zKey);
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.enterKey);

            yield return TestTools.DoUntilStateIsReached(() => _storyProgresser.ProgressStory(), () => !narrativeScriptPlayer.NarrativeScriptPlayer.HasSubStory);
            
            Assert.AreEqual(4, _penaltyManager.PenaltiesLeft);
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
            Assert.AreEqual(5, _penaltyManager.PenaltiesLeft);
        }
    }
}
