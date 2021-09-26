using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuKeyboardTests
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        /// <summary>
        /// Attempts to open and close the menu and checks if the menu is active after each attempt.
        /// </summary>
        [UnityTest, Order(0)]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator EvidenceMenuOpensAndCloses()
        {
            yield return null;
            global::EvidenceMenu evidenceMenu = InputTestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Attempts to close the menu when evidence is being
        /// presented then asserts that the menu is still open.
        /// </summary>
        [UnityTest, Order(1)]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator EvidenceMenuCannotBeClosedWhenPresentingEvidence()
        {
            yield return EvidenceMenuOpensAndCloses();

            global::EvidenceMenu evidenceMenu = InputTestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, _inputTestTools.Keyboard.xKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.True(evidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Selects evidence and asserts that the menu has closed.
        /// </summary>
        [UnityTest, Order(2)]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator EvidenceCanBeSelected()
        {
            yield return EvidenceMenuCannotBeClosedWhenPresentingEvidence();

            global::EvidenceMenu evidenceMenu = InputTestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            Assert.False(evidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Attempts to navigate left and right then asserts that the correct item is selected.
        /// </summary>
        [UnityTest, Order(3)]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator CanNavigateWithLeftAndRightArrows()
        {
            yield return EvidenceCanBeSelected();

            global::EvidenceMenu evidenceMenu = InputTestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, _inputTestTools.Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);

            Assert.AreEqual("Bent Coins", evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
        }

        /// <summary>
        /// Presses navigation buttons a lot of times then asserts
        /// that the selected item is selected.
        /// </summary>
        [UnityTest, Order(4)]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator CanNavigateToMultiplePages()
        {
            yield return CanNavigateWithLeftAndRightArrows();

            global::EvidenceMenu evidenceMenu = InputTestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            // Get to the correct point in the scene
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, _inputTestTools.Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, _inputTestTools.Keyboard.xKey);

            // Spam navigation button
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey, 50);

            // Get the evidence menu text boxes to check if they are updated correctly
            Menu menu = evidenceMenu.GetComponent<Menu>();
            TextMeshProUGUI[] evidenceTextBoxes = evidenceMenu.GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI evidenceName =
                evidenceTextBoxes.First(evidenceTextBox => evidenceTextBox.gameObject.name == "EvidenceName");
            TextMeshProUGUI evidenceDescription = evidenceTextBoxes.First(evidenceTextBox =>
                evidenceTextBox.gameObject.name == "EvidenceDescription");
            Image[] images = evidenceMenu.GetComponentsInChildren<Image>();
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
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey, 4);

            // After all this Jory Sr's Letter should be selected
            Assert.AreEqual("Jory Sr's Letter",evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
        }

        /// <summary>
        /// Tries to open the profile menu and checks if the evidence menu items display profiles.
        /// Closes profile menu at the end and checks something is selected.
        /// </summary>
        [UnityTest, Order(5)]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator ProfileMenuCanBeAccessed()
        {
            yield return CanNavigateToMultiplePages();

            global::EvidenceMenu evidenceMenu = InputTestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
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
        [UnityTest, Order(6)]
        [ReloadScene("Assets/Scenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator EvidenceCanBeSubstitutedWithAltEvidence()
        {
            yield return ProfileMenuCanBeAccessed();

            global::EvidenceMenu evidenceMenu = InputTestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            
            while (!evidenceMenu.isActiveAndEnabled)
            {
                yield return _inputTestTools.PressForSeconds(_inputTestTools.Keyboard.xKey, 0.5f);
            }

            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            Assert.AreEqual("Attorney's Badge", evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
        }
    }
}
