using System.Collections;
using System.Linq;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuTest
    {
        private const string SCENE_PATH = "Assets/Scenes/TestScenes/EvidenceMenu - Test Scene.unity";
        
        protected InputTestTools InputTestTools { get; } = new InputTestTools();
        protected EvidenceController EvidenceController { get; private set; }
        protected global::DialogueController DialogueController { get; private set; }
        protected global::EvidenceMenu EvidenceMenu { get; private set; }
        protected Transform CanvasTransform { get; private set; }
        protected Menu Menu { get; private set; }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(SCENE_PATH, new LoadSceneParameters(LoadSceneMode.Additive));
            
            EvidenceController = Object.FindObjectOfType<EvidenceController>();
            DialogueController = Object.FindObjectOfType<global::DialogueController>();
            EvidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            Menu = EvidenceMenu.GetComponent<Menu>();
            CanvasTransform = Object.FindObjectOfType<Canvas>().transform;
            var dialogueController = Object.FindObjectOfType<global::DialogueController>();
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return SceneManager.UnloadSceneAsync("Assets/Scenes/TestScenes/EvidenceMenu - Test Scene.unity");
        }
        
        protected ActorData[] AddProfiles()
        {
            ActorData[] actors = Resources.LoadAll<ActorData>("Actors");

            foreach (var actorData in actors)
            {
                EvidenceController.AddToCourtRecord(actorData);
            }

            return actors;
        }
        
        protected Evidence[] AddEvidence()
        {
            var evidenceOfCurrentScript = DialogueController.ActiveNarrativeScript.ObjectStorage.GetObjectsOfType<Evidence>().ToArray();
            foreach (var evidence in evidenceOfCurrentScript)
            {
                EvidenceController.AddEvidence(evidence);
            }

            return evidenceOfCurrentScript;
        }
        
        protected IEnumerator PressZ()
        {
            yield return InputTestTools.PressForFrame(InputTestTools.Keyboard.zKey);
        }
    }
}

