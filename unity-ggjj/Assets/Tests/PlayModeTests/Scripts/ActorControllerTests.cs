using System.Collections;
using System.Reflection;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

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
            var defenceAnimator = GameObject.Find("Defense_Actor").GetComponent<Actor>().GetComponent<Animator>();

            yield return ProgressToTestPoint();
            
            _actorController.SetActiveSpeaker("TutorialBoy", SpeakingType.Speaking);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsTalking(prosecutionAnimator);
            AssertIsNotTalking(defenceAnimator);
        }

        [Test]
        public void ActiveActorCanBeSet()
        {
            _actorController.SetActiveActor("TutorialBoy");
            var witnessActor = GameObject.Find("Witness_Actor").GetComponent<Actor>();
            var objectStorage = Object.FindObjectOfType<global::DialogueController>().ActiveNarrativeScript.ObjectStorage;
            Assert.IsTrue(witnessActor.MatchesActorData(objectStorage.GetObject<ActorData>("TutorialBoy")));
        }

        [UnityTest]
        public IEnumerator SpeakerCanBeSetToThinking()
        {
            var defenceAnimator = GameObject.Find("Defense_Actor").GetComponent<Actor>().GetComponent<Animator>();
            yield return ProgressToTestPoint();
            
            _actorController.SetActiveSpeaker("Arin", SpeakingType.Thinking);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsNotTalking(defenceAnimator);
            AssertNameBoxCorrect();
        }

        [UnityTest]
        public IEnumerator SpeakerCanBeSetToNarrator()
        {
            yield return ProgressToTestPoint();
            var nameBox = Object.FindObjectOfType<NameBox>().gameObject;
            var defenceAnimator = GameObject.Find("Defense_Actor").GetComponent<Actor>().GetComponent<Animator>();

            _actorController.SetActiveSpeakerToNarrator();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsNotTalking(defenceAnimator);
            Assert.IsFalse(nameBox.activeInHierarchy);
        }

        private void AssertIsTalking(Animator animator)
        {
            var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            Assert.IsTrue(animationName.Contains("Talking"));
            Assert.IsTrue(animator.GetBool(TALKING_PARAMETER_NAME));
            AssertNameBoxCorrect();
        }

        private void AssertIsNotTalking(Animator animator)
        {
            var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            Assert.IsFalse(animationName.Contains("Talking"));
            Assert.IsFalse(animator.GetBool(TALKING_PARAMETER_NAME));
        }

        private void AssertNameBoxCorrect()
        {
            var actorData = _actorController.CurrentSpeakingActorData;
            var nameBoxImage = Object.FindObjectOfType<NameBox>().GetComponent<Image>();
            var nameBoxText = nameBoxImage.GetComponentInChildren<TextMeshProUGUI>();
            Assert.AreEqual(actorData.DisplayColor, nameBoxImage.color);
            Assert.AreEqual(actorData.DisplayName, nameBoxText.text);
        }

        private IEnumerator ProgressToTestPoint()
        {
            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();
            yield return _storyProgresser.ProgressStory();
            var dialogueController = Object.FindObjectOfType<global::DialogueController>();
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);
        }
    }
}
