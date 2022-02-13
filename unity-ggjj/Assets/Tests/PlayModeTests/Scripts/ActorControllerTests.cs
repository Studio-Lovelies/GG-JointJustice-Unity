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
            var prosecutionAnimator =  GameObject.Find("Prosecution_Actor").GetComponent<Actor>().GetComponent<Animator>();
            
            yield return ActorsAreSetToTalkingOnDialogueStart();
            _actorController.SetActiveSpeaker("TutorialBoy", SpeakingType.Speaking);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            yield return new WaitForSeconds(50);
            AssertIsTalking(prosecutionAnimator);
            AssertIsNotTalking(_witnessAnimator);
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
