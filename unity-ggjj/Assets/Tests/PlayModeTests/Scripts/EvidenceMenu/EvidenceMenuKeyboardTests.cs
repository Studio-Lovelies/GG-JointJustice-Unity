using System.Collections;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuKeyboardTests : EvidenceMenuTest
    {
        /// <summary>
        /// Attempts to open and close the menu and checks if the menu is active after each attempt.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceMenuOpensAndCloses()
        {
            yield return PressZ();
            Assert.True(EvidenceMenu.isActiveAndEnabled);
            yield return PressC();
            yield return PressZ();
            Assert.False(EvidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Attempts to close the menu when evidence is being
        /// presented then asserts that the menu is still open.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceMenuCannotBeClosedWhenPresentingEvidence()
        {
            EvidenceController.RequirePresentEvidence();
            yield return InputTestTools.WaitForBehaviourActiveAndEnabled(EvidenceMenu, InputTestTools.Keyboard.xKey);
            Assert.True(EvidenceMenu.isActiveAndEnabled);
            yield return PressZ();
            Assert.True(EvidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Selects evidence and asserts that the menu has closed.
        /// </summary>
        [UnityTest]
        public IEnumerator EvidenceCanBeSelected()
        {
            EvidenceController.AddEvidence(Resources.Load<Evidence>("Evidence/AttorneysBadge"));
            yield return PressZ();
            Assert.True(EvidenceMenu.isActiveAndEnabled);
            yield return InputTestTools.PressForFrame(InputTestTools.Keyboard.enterKey);
            Assert.True(EvidenceMenu.isActiveAndEnabled);
            yield return PressZ();
            
            EvidenceController.RequirePresentEvidence();
            yield return PressZ();
            Assert.True(EvidenceMenu.isActiveAndEnabled);
            yield return InputTestTools.PressForFrame(InputTestTools.Keyboard.enterKey);
            Assert.False(EvidenceMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Attempts to navigate left and right then asserts that the correct item is selected.
        /// </summary>
        [UnityTest]
        public IEnumerator CanNavigateWithLeftAndRightArrows()
        {
            var evidence = AddEvidence();
            
            yield return PressZ();
            foreach (var item in evidence)
            {
                Assert.AreEqual(item.DisplayName, Menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
                yield return PressRight();
            }

            for (int i = evidence.Length - 1; i > -1; i--)
            {
                Assert.AreEqual(evidence[i].DisplayName, Menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
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
            yield return InputTestTools.PressForFrame(InputTestTools.Keyboard.enterKey, 50);

            yield return PressRight();
            yield return CheckItems(Object.FindObjectsOfType<EvidenceMenuItem>().Length);
            yield return PressRight();
            
            // Spam navigation button
            yield return PressRight();
            yield return InputTestTools.PressForFrame(InputTestTools.Keyboard.enterKey, 101);
            yield return PressLeft();

            // After all this "Stolen Dinos" should be selected
            Assert.AreEqual("Stolen Dinos",Menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
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

            for (var i = 0; i < evidence.Length / 2; i++)
            {
                EvidenceController.SubstituteEvidence(EvidenceController.CurrentEvidence[i].name, EvidenceController.CurrentEvidence[evidence.Length - 1 - i].name);
                Assert.AreEqual(evidence[evidence.Length - 1 - i], EvidenceController.CurrentEvidence[i]); 
            }

            yield return PressZ();
            yield return CheckItems(evidence.Length);
        }

        private IEnumerator CheckItems(int count)
        {
            TextMeshProUGUI[] evidenceTextBoxes = EvidenceMenu.GetComponentsInChildren<TextMeshProUGUI>();
            TextMeshProUGUI evidenceName =
                evidenceTextBoxes.First(evidenceTextBox => evidenceTextBox.gameObject.name == "EvidenceName");
            TextMeshProUGUI evidenceDescription = evidenceTextBoxes.First(evidenceTextBox =>
                evidenceTextBox.gameObject.name == "EvidenceDescription");
            Image[] images = EvidenceMenu.GetComponentsInChildren<Image>();
            Image evidenceIcon = images.First(image => image.gameObject.name == "EvidenceIcon");

            for (int i = 0; i < count; i++)
            {
                ICourtRecordObject item = Menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
                Assert.AreEqual(evidenceName.text, item.CourtRecordName);
                Assert.AreEqual(evidenceDescription.text, item.Description);
                Assert.AreEqual(evidenceIcon.sprite, item.Icon);
                yield return PressRight();
            }
        }

        private IEnumerator PressC()
        {
            yield return InputTestTools.PressForFrame(InputTestTools.Keyboard.cKey);
        }

        private IEnumerator PressLeft()
        {
            yield return InputTestTools.PressForFrame(InputTestTools.Keyboard.leftArrowKey);
        }

        private IEnumerator PressRight()
        {
            yield return InputTestTools.PressForFrame(InputTestTools.Keyboard.rightArrowKey);
        }
    }
}
