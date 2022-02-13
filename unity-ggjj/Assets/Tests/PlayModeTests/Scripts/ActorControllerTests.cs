using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.ActorController
{
    public class ActorControllerTests
    {
        const string TALKING_PARAMETER_NAME = "Talking";
        
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        private StoryProgresser _storyProgresser;
        private global::ActorController _actorController;
        private Animator _witnessAnimator;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/CrossExamination - TestScene.unity", new LoadSceneParameters());
            yield return null;

            _storyProgresser = new StoryProgresser();
            _actorController = Object.FindObjectOfType<global::ActorController>();
            
            yield return _storyProgresser.ProgressStory();
            _witnessAnimator = GameObject.Find("Witness_Actor").GetComponent<Actor>().GetComponent<Animator>();
            AssertIsNotTalking(_witnessAnimator);
        }

        [UnityTest]
        public IEnumerator ActorsCanBeSetToTalkingInCode()
        {
            _actorController.StartTalking();
            yield return null;
            AssertIsTalking(_witnessAnimator);
            _actorController.StopTalking();
            yield return null;
            AssertIsNotTalking(_witnessAnimator);
        }

        [UnityTest]
        public IEnumerator ActorsAreSetToTalkingOnDialogueStart()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsTalking(_witnessAnimator);
        }

        [UnityTest]
        public IEnumerator ActorsAreSetToNotTalkingOnDialogueEnd()
        {
            yield return ActorsAreSetToTalkingOnDialogueStart();
            yield return _storyProgresser.ProgressStory();
            AssertIsNotTalking(_witnessAnimator);
        }

        [UnityTest]
        public IEnumerator ActiveSpeakerCanBeSet()
        {
            var dialogueController = Object.FindObjectOfType<global::DialogueController>();
            var prosecutionAnimator =  GameObject.Find("Prosecution_Actor").GetComponent<Actor>().GetComponent<Animator>();
            var defenceAnimator = GameObject.Find("Defense_Actor").GetComponent<Actor>().GetComponent<Animator>();

            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);
            
            _actorController.SetActiveSpeaker("TutorialBoy", SpeakingType.Speaking);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsTalking(prosecutionAnimator);
            AssertIsNotTalking(defenceAnimator);
        }

        private void AssertIsTalking(Animator animator)
        {
            var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            Assert.IsTrue(animationName.Contains("Talking"));
            Assert.IsTrue(animator.GetBool(TALKING_PARAMETER_NAME));
        }

        private void AssertIsNotTalking(Animator animator)
        {
            var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            Assert.IsFalse(animationName.Contains("Talking"));
            Assert.IsFalse(animator.GetBool(TALKING_PARAMETER_NAME));
        }
    }
}
