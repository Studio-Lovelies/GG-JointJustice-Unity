using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuTest
    {
        protected EvidenceController EvidenceController { get; private set; }
        protected NarrativeScriptPlayerComponent NarrativeScriptPlayerComponent { get; private set; }
        protected global::EvidenceMenu EvidenceMenu { get; private set; }
        protected Transform CanvasTransform { get; private set; }
        protected Menu Menu { get; private set; }
        protected StoryProgresser _storyProgresser = new StoryProgresser();

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
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            TestTools.StartGame("AddRecordTest");

            EvidenceController = Object.FindObjectOfType<EvidenceController>();
            NarrativeScriptPlayerComponent = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            EvidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            Menu = EvidenceMenu.GetComponent<Menu>();
            CanvasTransform = Object.FindObjectOfType<Canvas>().transform;
            var dialogueController = Object.FindObjectOfType<global::AppearingDialogueController>();
            yield return TestTools.WaitForState(() => !dialogueController.IsPrintingText);
        }

        protected ActorData[] AddProfiles()
        {
            ActorData[] actors = Resources.LoadAll<ActorData>("Actors");

            foreach (var actorData in actors)
            {
                EvidenceController.AddRecord(actorData);
            }

            return actors;
        }
        
        protected Evidence[] AddEvidence()
        {
            var evidenceOfCurrentScript = NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript.ObjectStorage.GetObjectsOfType<Evidence>().ToArray();
            foreach (var evidence in evidenceOfCurrentScript)
            {
                EvidenceController.AddEvidence(evidence);
            }

            return evidenceOfCurrentScript;
        }
        
        protected IEnumerator PressZ()
        {
            yield return _storyProgresser.PressForFrame(_storyProgresser.Keyboard.zKey);
        }
    }
}

