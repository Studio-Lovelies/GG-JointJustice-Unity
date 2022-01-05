using System.Collections;
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
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        private global::AppearingDialogueController _appearingDialogueController;
        private EvidenceController _evidenceController;
        private global::DialogueController _dialogueController;
        private global::EvidenceMenu _evidenceMenu;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/EvidenceMenu - Test Scene.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            _appearingDialogueController = Object.FindObjectOfType<global::AppearingDialogueController>();
            _evidenceController = Object.FindObjectOfType<EvidenceController>();
            _dialogueController = Object.FindObjectOfType<global::DialogueController>();
            _evidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            yield return TestTools.WaitForState(() => !_dialogueController.IsBusy);
        }
        
        /// <summary>
        /// Attempts to open and close the menu and checks if the menu is active after each attempt.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceMenuOpensAndCloses()
        {
            yield return TestTools.WaitForState(() => !_dialogueController.IsBusy);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
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
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Selects evidence and asserts that the menu has closed.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceCanBeSelected()
        {
            _evidenceController.AddEvidence("Attorneys_Badge");
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            
            _evidenceController.RequirePresentEvidence();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
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
            _evidenceController.AddEvidence("Attorneys_Badge");
            _evidenceController.AddEvidence("Bent_Coins");
            _evidenceController.AddEvidence("Jorys_Backpack");
            _evidenceController.AddEvidence("Jory_Srs_Letter");
            _evidenceController.AddEvidence("Livestream_Recording");
            _evidenceController.AddEvidence("Plumber_Invoice");
            _evidenceController.AddEvidence("Stolen_Dinos");

            var menu = _evidenceMenu.GetComponent<Menu>();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            Assert.AreEqual("Attorney's Badge", menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            Assert.AreEqual("Attorney's Badge", menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            Assert.AreEqual("Bent Coins", menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            Assert.AreEqual("Jory's Backpack", menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            Assert.AreEqual("Jory Sr's Letter", menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            Assert.AreEqual("Jory's Backpack", menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
        }

        /// <summary>
        /// Presses navigation buttons a lot of times then asserts
        /// that the selected item is selected.
        /// </summary>
        [UnityTest]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator CanNavigateToMultiplePages()
        {
            for (int i = 0; i < 5; i++)
            {
                _evidenceController.AddEvidence("Attorneys_Badge");
            }
            for (int i = 0; i < 5; i++)
            {
                _evidenceController.AddEvidence("Bent_Coins");
            }

            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            
            // Spam navigation button
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey, 50);

            // Get the evidence menu text boxes to check if they are updated correctly
            Menu menu = _evidenceMenu.GetComponent<Menu>();
            TextMeshProUGUI[] evidenceTextBoxes = _evidenceMenu.GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI evidenceName =
                evidenceTextBoxes.First(evidenceTextBox => evidenceTextBox.gameObject.name == "EvidenceName");
            TextMeshProUGUI evidenceDescription = evidenceTextBoxes.First(evidenceTextBox =>
                evidenceTextBox.gameObject.name == "EvidenceDescription");
            Image[] images = _evidenceMenu.GetComponentsInChildren<Image>();
            Image evidenceIcon = images.First(image => image.gameObject.name == "EvidenceIcon");
            
            // Move to right navigation button and check displayed menu information is updated correctly.
            for (int i = 0; i < 9; i++)
            {
                yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
                ICourtRecordObject evidence = menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
                Assert.AreEqual(evidenceName.text, evidence.DisplayName);
                Assert.AreEqual(evidenceDescription.text, evidence.Description);
                Assert.AreEqual(evidenceIcon.sprite, evidence.Icon);
            }

            // Spam navigation button
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey, 101);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            
            // After all this Jory Sr's Letter should be selected
            Assert.AreEqual("Bent Coins",_evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
        }

        /// <summary>
        /// Tries to open the profile menu and checks if the evidence menu items display profiles.
        /// Closes profile menu at the end and checks something is selected.
        /// </summary>
        [UnityTest]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator ProfileMenuCanBeAccessed()
        {
            yield return CanNavigateToMultiplePages();

            global::EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            Menu menu = evidenceMenu.GetComponent<Menu>();
            TextMeshProUGUI[] evidenceTextBoxes = evidenceMenu.GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI evidenceName =
                evidenceTextBoxes.First(evidenceTextBox => evidenceTextBox.gameObject.name == "EvidenceName");
            TextMeshProUGUI evidenceDescription = evidenceTextBoxes.First(evidenceTextBox =>
                evidenceTextBox.gameObject.name == "EvidenceDescription");
            Image[] images = evidenceMenu.GetComponentsInChildren<Image>();
            Image evidenceIcon = images.First(image => image.gameObject.name == "EvidenceIcon");

            // Check displayed menu information updates properly.
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            ICourtRecordObject actor = menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
            Assert.AreEqual(evidenceName.text, actor.CourtRecordName);
            Assert.AreEqual(evidenceDescription.text, actor.Description);
            Assert.AreEqual(evidenceIcon.sprite, actor.Icon);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey, 5);
            actor = menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
            Assert.AreEqual(evidenceName.text, actor.CourtRecordName);
            Assert.AreEqual(evidenceDescription.text, actor.Description);
            Assert.AreEqual(evidenceIcon.sprite, actor.Icon);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            actor = menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
            Assert.AreEqual(evidenceName.text, actor.CourtRecordName);
            Assert.AreEqual(evidenceDescription.text, actor.Description);
            Assert.AreEqual(evidenceIcon.sprite, actor.Icon);
        }

        /// <summary>
        /// Checks evidence is correctly substituted with its designated alternate evidence.
        /// </summary>
        [UnityTest]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator EvidenceCanBeSubstitutedWithAltEvidence()
        {
            yield return ProfileMenuCanBeAccessed();
            global::EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            
            while (!evidenceMenu.isActiveAndEnabled)
            {
                yield return _inputTestTools.PressForSeconds(_inputTestTools.Keyboard.xKey, 0.5f);
            }

            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            Assert.AreEqual("Switch", evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
        }
    }
}
