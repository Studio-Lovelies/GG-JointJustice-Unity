using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuMouseTests : EvidenceMenuTest
    {
        /// <summary>
        /// Places mouse over each menu item and asserts it is highlighted.
        /// </summary>
        [UnityTest]
        public IEnumerator MouseShouldHighlightEvidenceMenuItems()
        {
            AddEvidence();
            AddEvidence();
            yield return PressZ();
            
            // Get the menu items to test
            var menuItems = GetMenuItems();
            var firstMenuItem = GetFirstMenuItem();
            yield return HoverOverButton(firstMenuItem.transform);
            Assert.AreEqual(firstMenuItem.CourtRecordObject.DisplayName, Menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
            
            // Loop through the menu items and check if they highlight correctly
            for (var i = 1; i < menuItems.Length; i++)
            {
                var menuItem = menuItems.First(item => item.gameObject.name == $"EvidenceMenuItem ({i})");
                yield return HoverOverButton(menuItem.transform);
                var evidence = Menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
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
            
            var decrementButton = GameObject.Find("DecrementButton").GetComponent<MenuItem>().transform;
            var incrementButton = GameObject.Find("IncrementButton").GetComponent<MenuItem>().transform;
            
            var firstMenuItem = GetFirstMenuItem();
            yield return HoverOverButton(firstMenuItem.transform);
            Assert.AreEqual("Bent Coins", firstMenuItem.CourtRecordObject.DisplayName);

            yield return HoverOverButton(decrementButton);
            yield return LeftClick();
            yield return HoverOverButton(firstMenuItem.transform);
            Assert.AreEqual("Livestream Recording", firstMenuItem.CourtRecordObject.DisplayName);

            yield return HoverOverButton(incrementButton);
            yield return LeftClick();
            yield return HoverOverButton(firstMenuItem.transform);
            Assert.AreEqual("Bent Coins", firstMenuItem.CourtRecordObject.DisplayName);
            
            yield return HoverOverButton(incrementButton);
            yield return LeftClick();
            yield return HoverOverButton(firstMenuItem.transform);
            Assert.AreEqual("Livestream Recording", firstMenuItem.CourtRecordObject.DisplayName);
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
            Assert.IsTrue(EvidenceMenu.isActiveAndEnabled);

            foreach (var menuItem in menuItems)
            {
                yield return HoverOverButton(menuItem.transform);
                var clicked = false;
                menuItem.GetComponent<Button>().onClick.AddListener(() => clicked = true);
                yield return _storyProgresser.PressForFrame(_storyProgresser.Mouse.leftButton);
                Assert.IsTrue(clicked);
            }
        }

        private static MenuItem[] GetMenuItems()
        {
            return GameObject.Find("EvidenceContainer").GetComponentsInChildren<MenuItem>();
        }

        private static EvidenceMenuItem GetFirstMenuItem()
        {
            return GetMenuItems().First(menuItem => menuItem.gameObject.name == "EvidenceMenuItem").GetComponent<EvidenceMenuItem>();
        }

        private IEnumerator HoverOverButton(Transform transform)
        {
            yield return _storyProgresser.SetMouseWorldSpacePosition(transform.TransformPoint(transform.GetComponent<RectTransform>().rect.center));
        }

        private IEnumerator LeftClick()
        {
            yield return _storyProgresser.PressForFrame(_storyProgresser.Mouse.leftButton);
        }
    }
}
