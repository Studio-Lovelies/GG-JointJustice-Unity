using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts
{
    public class ActorControllerTests
    {
        private readonly StoryProgresser _storyProgresser = new StoryProgresser();
        private ActorController _actorController;
        private Animator _witnessAnimator;

        private static readonly int TalkingState = Animator.StringToHash("Talking");

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            _storyProgresser.Setup();
            SceneManager.LoadScene("Game");
            yield return null;
            TestTools.StartGame("ActorControllerTestScript");

            _actorController = Object.FindObjectOfType<ActorController>();
            
            yield return _storyProgresser.ProgressStory();
            _witnessAnimator = GameObject.Find("Witness_Actor").GetComponent<Animator>();
            AssertIsNotTalking(_witnessAnimator);
        }

        [UnityTearDown]
        public void UnityTearDown()
        {
            _storyProgresser.TearDown();
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
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
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
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
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
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
            AssertIsNotTalking(_witnessAnimator);
            AssertNameBoxCorrect();
        }

        [UnityTest]
        public IEnumerator SpeakerCanBeSetToNarrator()
        {
            var nameBox = Object.FindObjectOfType<NameBox>().gameObject;

            _actorController.SetActiveSpeakerToNarrator();
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
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
            _actorController.AssignActorToSlot("Defense", ACTOR_NAME);
            _actorController.AssignActorToSlot("Prosecution", ACTOR_NAME);
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
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
            AssertIsNotTalking(_witnessAnimator);
            AssertNameBoxCorrect();
        }
        
        [UnityTest]
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public IEnumerator ActorVisibilityCanBeSetForActiveActor(bool expectedVisibility)
        {
            return AssertVisibility(expectedVisibility, null, _witnessAnimator.GetComponent<Renderer>());
        }

        [UnityTest]
        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public IEnumerator ActorVisibilityCanBeSetForNonActiveActor(bool expectedVisibility)
        {
            return AssertVisibility(expectedVisibility, "TutorialBoy", GameObject.Find("Prosecution_Actor").GetComponent<Renderer>());
        }

        [UnityTest]
        public IEnumerator SetPoseOnActorThatIsInactive()
        {
            const string ACTOR_NAME = "Arin";
            const string ANIMATION_NAME = "Point";
            
            var sceneController = Object.FindObjectOfType<SceneController>();
            sceneController.SetScene("TMPHCourt");
            _actorController.AssignActorToSlot("Defense", ACTOR_NAME);
            sceneController.SetScene("Anime");
            _actorController.SetPose(ANIMATION_NAME, ACTOR_NAME); // explicitly select ACTOR_NAME
            sceneController.SetScene("TMPHCourt");
            
            var defenseAnimator = GameObject.Find("Defense_Actor").GetComponent<Animator>();
            
            yield return _storyProgresser.ProgressStory();
            Assert.AreEqual(ANIMATION_NAME, defenseAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            LogAssert.NoUnexpectedReceived();
        }

        // This is a regression test for #351:
        // #351 fixed #233 by enabling GameObjects that contained the Animator and disabling them again, if they were inactive before
        // However, after #351, containing GameObjects would accidentally be deactivated EVEN if they were active before
        [UnityTest]
        public IEnumerator SetPoseOnActorThatIsAlreadyActive()
        {
            const string ACTOR_NAME = "Arin";
            const string ANIMATION_NAME = "Point";
            
            var bgSceneList = GameObject.Find("BGSceneList");

            var sceneController = Object.FindObjectOfType<SceneController>();
            sceneController.SetScene("TMPHCourt");
            _actorController.AssignActorToSlot("Defense", ACTOR_NAME);
            _actorController.SetPose(ANIMATION_NAME, ACTOR_NAME);
            
            Assert.IsTrue(bgSceneList.activeSelf);
            
            var defenseAnimator = GameObject.Find("Defense_Actor").GetComponent<Animator>();
            
            yield return _storyProgresser.ProgressStory();
            Assert.AreEqual(ANIMATION_NAME, defenseAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            LogAssert.NoUnexpectedReceived();
        }

        private IEnumerator AssertVisibility(bool expectedVisibility, string actorName, Renderer renderer)
        {
            renderer.enabled = !expectedVisibility;
            Assert.IsTrue(renderer.enabled == !expectedVisibility, "Unity sanity check");
            _actorController.SetVisibility(expectedVisibility, actorName == null ? null : new ActorAssetName(actorName));
            Assert.IsTrue(renderer.enabled == expectedVisibility);
            yield break;
        }

        private void AssertIsTalking(Animator animator)
        {
            var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            Assert.IsTrue(animationName.Contains("Talking"));
            Assert.IsTrue(animator.GetBool(TalkingState));
            AssertNameBoxCorrect();
        }

        private static void AssertIsNotTalking(Animator animator)
        {
            var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            Assert.IsFalse(animationName.Contains("Talking"));
            Assert.IsFalse(animator.GetBool(TalkingState));
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
