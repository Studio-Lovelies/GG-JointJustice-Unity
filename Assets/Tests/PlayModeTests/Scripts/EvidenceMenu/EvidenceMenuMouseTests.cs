using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuMouseTests : EvidenceMenuTest
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        /// <summary>
        /// Places mouse over each menu item and asserts it is highlighted.
        /// </summary>
        [UnityTest]
        public IEnumerator MouseShouldHighlightEvidenceMenuItems()
        {
            AddEvidence();
            AddEvidence();
            
            Vector3 canvasScale = Vector3.right * CanvasTransform.localScale.x;

            yield return PressZ();
            
            // Get the menu items to test
            MenuItem[] menuItems = GetMenuItems();
            EvidenceMenuItem firstMenuItem = GetFirstMenuItem();
            yield return _inputTestTools.SetMousePositionWorldSpace(firstMenuItem.transform.position + firstMenuItem.GetComponent<RectTransform>().rect.size.x * canvasScale);
            Assert.AreEqual(firstMenuItem.CourtRecordObject.DisplayName, Menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
            
            // Loop through the menu items and check if they highlight correctly
            for (int i = 1; i < menuItems.Length; i++)
            {
                MenuItem menuItem = menuItems.First(item => item.gameObject.name == $"EvidenceMenuItem ({i})");
                yield return _inputTestTools.SetMousePositionWorldSpace(menuItem.transform.position + (menuItem.GetComponent<RectTransform>().rect.size.x / 2) * canvasScale);
                ICourtRecordObject evidence = Menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
                Assert.AreEqual(menuItem.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName, evidence.DisplayName);
            }
        }

        /// <summary>
        /// Presses the navigation buttons multiple times then checks if
        /// the first menu item is the same as the one that the menu thinks is selected.
        /// </summary>
        [UnityTest]
        public IEnumerator NavigationButtonsCanBeClicked()
        {
            AddEvidence();
            AddEvidence();
            
            yield return PressZ();
            
            Transform decrementButton = GameObject.Find("DecrementButton").GetComponent<MenuItem>().transform;
            Transform incrementButton = GameObject.Find("IncrementButton").GetComponent<MenuItem>().transform;
            
            EvidenceMenuItem firstMenuItem = GetFirstMenuItem();
            yield return SelectButton(firstMenuItem.transform);
            Assert.AreEqual("Attorney's Badge", firstMenuItem.CourtRecordObject.DisplayName);
            
            yield return _inputTestTools.SetMousePositionWorldSpace(decrementButton.transform.position);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Mouse.leftButton);
            yield return SelectButton(firstMenuItem.transform);
            Assert.AreEqual("Bent Coins", firstMenuItem.CourtRecordObject.DisplayName);

            yield return SelectButton(incrementButton);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Mouse.leftButton);
            yield return SelectButton(firstMenuItem.transform);
            Assert.AreEqual("Attorney's Badge", firstMenuItem.CourtRecordObject.DisplayName);
            
            yield return SelectButton(incrementButton);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Mouse.leftButton);
            yield return SelectButton(firstMenuItem.transform);
            Assert.AreEqual("Bent Coins", firstMenuItem.CourtRecordObject.DisplayName);
        }

        /// <summary>
        /// Attempts to click a menu item and asserts the menu is closed.
        /// </summary>
        [UnityTest]
        public IEnumerator MenuItemsCanBeClickedWithMouse()
        {
            AddEvidence();
            yield return PressZ();
            var menuItems = GetMenuItems();

            foreach (var menuItem in menuItems)
            {
                yield return SelectButton(menuItem.transform);
                yield return new WaitForSeconds(50);
                Assert.IsFalse(EvidenceMenu.isActiveAndEnabled);
                yield return PressZ();
                Assert.IsTrue(EvidenceMenu.isActiveAndEnabled);
            }
        }

        private MenuItem[] GetMenuItems()
        {
            return GameObject.Find("EvidenceContainer").GetComponentsInChildren<MenuItem>();
        }

        private EvidenceMenuItem GetFirstMenuItem()
        {
            return GetMenuItems().First(menuItem => menuItem.gameObject.name == "EvidenceMenuItem").GetComponent<EvidenceMenuItem>();
        }

        private IEnumerator SelectButton(Transform tf)
        {
            yield return _inputTestTools.SetMousePositionWorldSpace(tf.position + tf.GetComponent<RectTransform>().rect.size.x * Vector3.right * CanvasTransform.localScale.x);
        }
    }
}
