using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuKeyboardTests
    {
        private const string SCENE_PATH = "Assets/Scenes/EvidenceMenu - Test Scene.unity";
        
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        private EvidenceController _evidenceController;
        private global::DialogueController _dialogueController;
        private global::EvidenceMenu _evidenceMenu;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(SCENE_PATH, new LoadSceneParameters(LoadSceneMode.Additive));
            _evidenceController = Object.FindObjectOfType<EvidenceController>();
            _dialogueController = Object.FindObjectOfType<global::DialogueController>();
            _evidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            yield return TestTools.WaitForState(() => !_dialogueController.IsBusy);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return SceneManager.UnloadSceneAsync("Assets/Scenes/EvidenceMenu - Test Scene.unity");
        }
        
        /// <summary>
        /// Attempts to open and close the menu and checks if the menu is active after each attempt.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceMenuOpensAndCloses()
        {
            yield return TestTools.WaitForState(() => !_dialogueController.IsBusy);
            yield return PressZ();
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return PressC();
            yield return PressZ();
            Assert.False(_evidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Attempts to close the menu when evidence is being
        /// presented then asserts that the menu is still open.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceMenuCannotBeClosedWhenPresentingEvidence()
        {
            _evidenceController.RequirePresentEvidence();
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, _inputTestTools.Keyboard.xKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return PressZ();
            Assert.True(_evidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Selects evidence and asserts that the menu has closed.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceCanBeSelected()
        {
            _evidenceController.AddEvidence(Resources.Load<Evidence>("Evidence/Attorneys_Badge"));
            yield return PressZ();
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return PressZ();
            
            _evidenceController.RequirePresentEvidence();
            yield return PressZ();
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            Assert.False(_evidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Attempts to navigate left and right then asserts that the correct item is selected.
        /// </summary>
        [UnityTest]
        public IEnumerator CanNavigateWithLeftAndRightArrows()
        {
            var evidence = AddEvidence();

            var menu = _evidenceMenu.GetComponent<Menu>();
            yield return PressZ();
            foreach (var item in evidence)
            {
                Assert.AreEqual(item.DisplayName, menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
                yield return PressRight();
            }

            for (int i = evidence.Length - 1; i > -1; i--)
            {
                Assert.AreEqual(evidence[i].DisplayName, menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
                yield return PressLeft();
            }
        }

        /// <summary>
        /// Presses navigation buttons a lot of times then asserts
        /// that the selected item is selected.
        /// </summary>
        [UnityTest]
        public IEnumerator CanNavigateToMultiplePages()
        {
            AddEvidence();
            AddEvidence();

            yield return PressZ();
            
            // Spam navigation button
            yield return PressLeft();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey, 50);

            yield return PressRight();
            yield return CheckItems(Object.FindObjectsOfType<EvidenceMenuItem>().Length);
            yield return PressRight();
            
            // Spam navigation button
            yield return PressRight();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey, 101);
            yield return PressLeft();
            
            // After all this Jory Sr's Letter should be selected
            Assert.AreEqual("Switch",_evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
        }

        /// <summary>
        /// Tries to open the profile menu and checks if the evidence menu items display profiles.
        /// Closes profile menu at the end and checks something is selected.
        /// </summary>
        [UnityTest]
        public IEnumerator ProfileMenuCanBeAccessed()
        {
            var profiles = AddProfiles();
            yield return PressZ();
            yield return PressC();
            yield return CheckItems(profiles.Length);
        }

        /// <summary>
        /// Checks evidence is correctly substituted with its designated alternate evidence.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceCanBeSubstitutedWithAltEvidence()
        {
            var evidence = AddEvidence();

            for (int i = 0; i < evidence.Length / 2; i++)
            {
                _evidenceController.CurrentEvidence[i].AltEvidence =
                    evidence[evidence.Length - 1 - i];
            }

            for (int i = 0; i < evidence.Length / 2; i++)
            {
                _evidenceController.SubstituteEvidenceWithAlt(_evidenceController.CurrentEvidence[i]);
                Assert.AreEqual(evidence[evidence.Length - 1 - i], _evidenceController.CurrentEvidence[i]); 
            }

            yield return PressZ();
            yield return CheckItems(evidence.Length);
        }

        private IEnumerator CheckItems(int count)
        {
            Menu menu = _evidenceMenu.GetComponent<Menu>();
            TextMeshProUGUI[] evidenceTextBoxes = _evidenceMenu.GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI evidenceName =
                evidenceTextBoxes.First(evidenceTextBox => evidenceTextBox.gameObject.name == "EvidenceName");
            TextMeshProUGUI evidenceDescription = evidenceTextBoxes.First(evidenceTextBox =>
                evidenceTextBox.gameObject.name == "EvidenceDescription");
            Image[] images = _evidenceMenu.GetComponentsInChildren<Image>();
            Image evidenceIcon = images.First(image => image.gameObject.name == "EvidenceIcon");

            for (int i = 0; i < count; i++)
            {
                ICourtRecordObject item = menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
                Assert.AreEqual(evidenceName.text, item.CourtRecordName);
                Assert.AreEqual(evidenceDescription.text, item.Description);
                Assert.AreEqual(evidenceIcon.sprite, item.Icon);
                yield return PressRight();
            }
        }

        private Evidence[] AddEvidence()
        {
            Evidence[] evidence = Resources.LoadAll<Evidence>("Evidence");

            foreach (var item in evidence)
            {
                _evidenceController.AddEvidence(item);
            }

            return evidence;
        }
        
        private IEnumerator PressZ()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
        }

        private IEnumerator PressC()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
        }

        private IEnumerator PressLeft()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
        }

        private IEnumerator PressRight()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
        }
        
        private ActorData[] AddProfiles()
        {
            ActorData[] actors = Resources.LoadAll<ActorData>("Actors");

            foreach (var actorData in actors)
            {
                _evidenceController.AddToCourtRecord(actorData);
            }

            return actors;
        }
    }
}
