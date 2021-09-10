using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuMouseTest
    {
        private global::EvidenceMenu _evidenceMenu;
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        
        private Keyboard Keyboard => _inputTestTools.Keyboard;
        private Mouse Mouse => _inputTestTools.Mouse;
        
        [UnityTest, Order(0)]
        public IEnumerator MouseShouldHighlightEvidenceMenuItems()
        {
            SceneManager.LoadScene("EvidenceMenu - Test Scene", LoadSceneMode.Single);
            yield return null;
            _evidenceMenu = Resources.FindObjectsOfTypeAll<global::EvidenceMenu>()[0];
            
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, Keyboard.xKey);

            MenuItem[] menuItems = GameObject.Find("EvidenceContainer").GetComponentsInChildren<MenuItem>();
            MenuItem decrementButton = GameObject.Find("DecrementButton").GetComponent<MenuItem>();
            MenuItem incrementButton = GameObject.Find("IncrementButton").GetComponent<MenuItem>();
            EvidenceMenuItem firstMenuItem = menuItems.First(menuItem => menuItem.gameObject.name == "EvidenceMenuItem").GetComponent<EvidenceMenuItem>();
            yield return _inputTestTools.SetMousePosition(firstMenuItem.transform.position);
            Menu menu = _evidenceMenu.GetComponent<Menu>();
            Assert.AreEqual(firstMenuItem.Evidence.DisplayName, menu.SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);

            for (int i = 1; i < menuItems.Length; i++)
            {
                MenuItem menuItem = menuItems.First(menuItem => menuItem.gameObject.name == $"EvidenceMenuItem ({i})");
                yield return _inputTestTools.SetMousePosition(menuItem.transform.position);
                yield return null;
                Assert.AreEqual(menuItem.GetComponent<EvidenceMenuItem>().Evidence.DisplayName, menu.SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);
            }
        }
        
        [UnityTest, Order(1)]
        public IEnumerator NavigationButtonsCanBeClicked()
        {
            MenuItem decrementButton = GameObject.Find("DecrementButton").GetComponent<MenuItem>();
            MenuItem incrementButton = GameObject.Find("IncrementButton").GetComponent<MenuItem>();
            Menu menu = _evidenceMenu.GetComponent<Menu>();
            
            yield return _inputTestTools.SetMousePosition(decrementButton.transform.position);
            yield return _inputTestTools.RepeatPressForFrames(Mouse.leftButton, 100);
            yield return _inputTestTools.SetMousePosition(incrementButton.transform.position);
            yield return _inputTestTools.RepeatPressForFrames(Mouse.leftButton, 123);

            Transform menuItem = GameObject.Find("EvidenceMenuItem").transform;
            yield return _inputTestTools.SetMousePosition(menuItem.position);

            Assert.AreEqual(menuItem.GetComponent<EvidenceMenuItem>().Evidence.DisplayName,
                menu.SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);
        }

        [UnityTest, Order(2)]
        public IEnumerator MenuItemsCanBeClickedWithMouse()
        {
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Mouse.leftButton);
            Assert.False(_evidenceMenu.isActiveAndEnabled);
        }
    }
}
