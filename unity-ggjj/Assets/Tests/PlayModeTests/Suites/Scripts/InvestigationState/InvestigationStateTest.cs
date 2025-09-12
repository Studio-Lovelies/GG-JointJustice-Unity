using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Suites.Scripts.InvestigationState
{
    public class InvestigationStateTest
    {
        protected NarrativeScriptPlayerComponent NarrativeScriptPlayerComponent { get; private set; }
        protected Menu InvestigationMainMenu { get; private set; }
        protected Menu InvestigationTalkMenu { get; private set; }
        protected Menu InvestigationMoveMenu { get; private set; }
        protected Menu EvidenceMenu { get; private set; }
        protected GameObject InvestigateMoveContainer { get; private set; }
        protected GameObject SpeechPanel { get; private set; }
        protected Transform CanvasTransform { get; private set; }
        protected readonly StoryProgresser StoryProgresser = new StoryProgresser();

        [TearDown]
        public void TearDown()
        {
            StoryProgresser.TearDown();
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            StoryProgresser.Setup();

            yield return SceneManager.LoadSceneAsync("Game");
            TestTools.StartGame("InvestigationUI");

            NarrativeScriptPlayerComponent = Object.FindAnyObjectByType<NarrativeScriptPlayerComponent>();
            InvestigationMainMenu = TestTools.FindInactiveInSceneByName<Menu>("InvestigateMainMenu");
            InvestigationTalkMenu = TestTools.FindInactiveInSceneByName<Menu>("InvestigateTalkMenu");
            InvestigationMoveMenu = TestTools.FindInactiveInSceneByName<Menu>("InvestigateMoveMenu");
            EvidenceMenu = TestTools.FindInactiveInSceneByName<Menu>("EvidenceMenu");
            InvestigateMoveContainer = TestTools.FindInactiveInSceneByName<GameObject>("InvestigateMoveContainer");
            SpeechPanel = GameObject.Find("SpeechPanel");
            CanvasTransform = Object.FindAnyObjectByType<Canvas>().transform;
            var dialogueController = Object.FindAnyObjectByType<global::AppearingDialogueController>();
            yield return TestTools.WaitForState(() => !dialogueController.IsPrintingText);
            
            Assert.False(InvestigationMainMenu.isActiveAndEnabled);
            yield return StoryProgresser.PressForFrame(StoryProgresser.keyboard.xKey);
        }
    }
}

