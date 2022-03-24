using System;
using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
            SceneManager.LoadScene("Game");
            yield return null;
            TestTools.StartGame("ActorControllerTestScript");

            _storyProgresser = new StoryProgresser();
            _actorController = Object.FindObjectOfType<global::ActorController>();
            
            yield return _storyProgresser.ProgressStory();
            _witnessAnimator = GameObject.Find("Witness_Actor").GetComponent<Animator>();
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
            var prosecutionAnimator =  GameObject.Find("Prosecution_Actor").GetComponent<Animator>();
            _actorController.SetActiveSpeaker("TutorialBoy", SpeakingType.Speaking);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsTalking(prosecutionAnimator);
            AssertIsNotTalking(_witnessAnimator);
        }

        [Test]
        public void ActiveActorCanBeSet()
        {
            _actorController.SetActiveActor("TutorialBoy");
            var witnessActor = GameObject.Find("Witness_Actor").GetComponent<Actor>();
            var objectStorage = Object.FindObjectOfType<NarrativeScriptPlayerComponent>().NarrativeScriptPlayer.ActiveNarrativeScript.ObjectStorage;
            Assert.IsTrue(witnessActor.MatchesActorData(objectStorage.GetObject<ActorData>("TutorialBoy")));
        }

        [UnityTest]
        public IEnumerator SpeakerCanBeSetToThinking()
        {
            _actorController.SetActiveSpeaker("Arin", SpeakingType.Thinking);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsNotTalking(_witnessAnimator);
            AssertNameBoxCorrect();
        }

        [UnityTest]
        public IEnumerator SpeakerCanBeSetToNarrator()
        {
            var nameBox = Object.FindObjectOfType<NameBox>().gameObject;

            _actorController.SetActiveSpeakerToNarrator();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsNotTalking(_witnessAnimator);
            Assert.IsFalse(nameBox.activeInHierarchy);
        }

        [UnityTest]
        public IEnumerator ActorPoseCanBeSet()
        {
            const string POSE_NAME = "Sweaty"; 
            Assert.AreNotEqual(POSE_NAME, _witnessAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            _actorController.SetPose(POSE_NAME);
            yield return null;
            Assert.AreEqual(POSE_NAME, _witnessAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        [UnityTest]
        public IEnumerator ActorEmotionsCanBePlayed()
        {
            const string EMOTION_NAME = "HelmetThrow";
            _actorController.PlayEmotion(EMOTION_NAME);
            yield return null;
            Assert.AreEqual(EMOTION_NAME, _witnessAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        [Test]
        public void ActorsCanBeAssignedToSlots()
        {
            const string ACTOR_NAME = "Ross";
            _actorController.AssignActorToSlot(ACTOR_NAME, 1);
            _actorController.AssignActorToSlot(ACTOR_NAME, 3);
            var actors = Object.FindObjectOfType<BGSceneList>().GetComponentsInChildren<Actor>();
            foreach (var actor in actors)
            {
                Assert.AreEqual(ACTOR_NAME, actor.ActorData.InstanceName);
            }
        }

        [UnityTest]
        public IEnumerator ActorNotInTheSceneCanSpeak()
        {
            _actorController.SetActiveSpeaker("Dan", SpeakingType.Speaking);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            AssertIsNotTalking(_witnessAnimator);
            AssertNameBoxCorrect();
        }
        
        [UnityTest]
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool ActorVisiblityCanBeSetForActiveActor(bool shouldShow)
        {
            return AssertVisibility(shouldShow, null, _witnessAnimator.GetComponent<Renderer>());
        }

        [UnityTest]
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public bool ActorVisibilityCanBeSetForNonActiveActor(bool shouldShow)
        {
            return AssertVisibility(shouldShow, "TutorialBoy", GameObject.Find("Prosecution_Actor").GetComponent<Renderer>());
        }

        private bool AssertVisibility(bool shouldShow, string actorName, Renderer renderer)
        {
            renderer.enabled = !shouldShow;
            Assert.IsTrue(renderer.enabled == !shouldShow);
            _actorController.SetVisibility(shouldShow, actorName == null ? null : new ActorAssetName(actorName));
            return renderer.enabled;
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
    }
}
