using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuMouseTests
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        /// <summary>
        /// Places mouse over each menu item and asserts it is highlighted.
        /// </summary>
        [UnityTest, Order(0)]
        [ReloadScene("Assets/Scenes/TestScenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator MouseShouldHighlightEvidenceMenuItems()
        {
            yield return null;
            var dialogueController = Object.FindObjectOfType<global::DialogueController>();
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);
            Transform canvasTransform = Object.FindObjectOfType<Canvas>().transform;
            Vector3 canvasScale = Vector3.right * canvasTransform.localScale.x;
            global::EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
        
            // Get to required point in scene
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, _inputTestTools.Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, _inputTestTools.Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, _inputTestTools.Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(evidenceMenu, _inputTestTools.Keyboard.xKey);
            
            // Get the menu items to test
            MenuItem[] menuItems = GameObject.Find("EvidenceContainer").GetComponentsInChildren<MenuItem>();
            EvidenceMenuItem firstMenuItem = menuItems.First(menuItem => menuItem.gameObject.name == "EvidenceMenuItem").GetComponent<EvidenceMenuItem>();
            yield return _inputTestTools.SetMousePositionWorldSpace(firstMenuItem.transform.position + firstMenuItem.GetComponent<RectTransform>().rect.size.x * canvasScale);
            Menu menu = evidenceMenu.GetComponent<Menu>();
            Assert.AreEqual(firstMenuItem.CourtRecordObject.DisplayName, menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
            
            // Loop through the menu items and check if they highlight correctly
            for (int i = 1; i < menuItems.Length; i++)
            {
                MenuItem menuItem = menuItems.First(item => item.gameObject.name == $"EvidenceMenuItem ({i})");
                yield return _inputTestTools.SetMousePositionWorldSpace(menuItem.transform.position + (menuItem.GetComponent<RectTransform>().rect.size.x / 2) * canvasScale);
                ICourtRecordObject evidence = menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject;
                Assert.AreEqual(menuItem.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName, evidence.DisplayName);
            }
        }

        /// <summary>
        /// Presses the navigation buttons multiple times then checks if
        /// the first menu item is the same as the one that the menu thinks is selected.
        /// </summary>
        [UnityTest, Order(1)]
        [ReloadScene("Assets/Scenes/TestScenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator NavigationButtonsCanBeClicked()
        {
            yield return MouseShouldHighlightEvidenceMenuItems();

            global::EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            Transform canvasTransform = Object.FindObjectOfType<Canvas>().transform;
            Vector3 canvasScale = Vector3.right * canvasTransform.localScale.x;

            Menu menu = evidenceMenu.GetComponent<Menu>();

            MenuItem decrementButton = GameObject.Find("DecrementButton").GetComponent<MenuItem>();
            MenuItem incrementButton = GameObject.Find("IncrementButton").GetComponent<MenuItem>();
            
            yield return _inputTestTools.SetMousePositionWorldSpace(decrementButton.transform.position + (decrementButton.GetComponent<RectTransform>().rect.size.x / 2) * canvasScale);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Mouse.leftButton, 100);
            yield return _inputTestTools.SetMousePositionWorldSpace(incrementButton.transform.position + (incrementButton.GetComponent<RectTransform>().rect.size.x / 2) * canvasScale);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Mouse.leftButton, 123);
        
            Transform outerMenuItem = GameObject.Find("EvidenceMenuItem").transform;
            yield return _inputTestTools.SetMousePositionWorldSpace(outerMenuItem.position + (outerMenuItem.GetComponent<RectTransform>().rect.size.x / 2) * canvasScale);
        
            Assert.AreEqual(outerMenuItem.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName,
                menu.SelectedButton.GetComponent<EvidenceMenuItem>().CourtRecordObject.DisplayName);
        }

        /// <summary>
        /// Attempts to click a menu item and asserts the menu is closed.
        /// </summary>
        [UnityTest, Order(2)]
        [ReloadScene("Assets/Scenes/TestScenes/EvidenceMenu - Test Scene.unity")]
        public IEnumerator MenuItemsCanBeClickedWithMouse()
        {
            yield return NavigationButtonsCanBeClicked();

            global::EvidenceMenu evidenceMenu = TestTools.FindInactiveInScene<global::EvidenceMenu>()[0];
            Assert.True(evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Mouse.leftButton);
            Assert.False(evidenceMenu.isActiveAndEnabled);
        }
    }
}
