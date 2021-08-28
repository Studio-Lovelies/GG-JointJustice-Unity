using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scenes.MainMenu
{
    public class ViaKeyboard : InputTestFixture
    {
        private IEnumerator PressForFrame(ButtonControl control)
        {
            yield return new WaitForEndOfFrame();
            Press(control);
            yield return new WaitForEndOfFrame();
            Release(control);
            yield return new WaitForEndOfFrame();
        }

        [UnityTest]
        [ReloadScene]
        public IEnumerator CanEnterAndCloseTwoSubMenusIndividually()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            var menus = Resources.FindObjectsOfTypeAll<Menu>();
            var mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            var subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            var secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");

            var keyboard = InputSystem.AddDevice<Keyboard>();

            var selectedButton = mainMenu.SelectedButton.name;
            yield return PressForFrame(keyboard.rightArrowKey);
            var newSelectedButton = mainMenu.SelectedButton.name;
            Assert.AreNotEqual(selectedButton, newSelectedButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return PressForFrame(keyboard.enterKey);
            Assert.False(mainMenu.Active);
            Assert.True(subMenu.Active);

            yield return PressForFrame(keyboard.enterKey);
            Assert.False(subMenu.Active);
            Assert.True(secondSubMenu.Active);

            yield return PressForFrame(keyboard.downArrowKey);
            yield return PressForFrame(keyboard.downArrowKey);
            yield return PressForFrame(keyboard.downArrowKey);

            yield return PressForFrame(keyboard.enterKey);
            Assert.False(secondSubMenu.Active);
            Assert.True(subMenu.Active);

            yield return PressForFrame(keyboard.downArrowKey);
            yield return PressForFrame(keyboard.downArrowKey);
            yield return PressForFrame(keyboard.downArrowKey);

            yield return PressForFrame(keyboard.enterKey);
            Assert.False(subMenu.Active);
            Assert.True(mainMenu.Active);
        }

        [UnityTest]
        [ReloadScene]
        public IEnumerator CanEnterAndCloseTwoSubMenusWithCloseAllButton()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            var menus = Resources.FindObjectsOfTypeAll<Menu>();
            var mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            var subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            var secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");

            var keyboard = InputSystem.AddDevice<Keyboard>();

            var selectedButton = mainMenu.SelectedButton.name;
            yield return PressForFrame(keyboard.rightArrowKey);
            var newSelectedButton = mainMenu.SelectedButton.name;
            Assert.AreNotEqual(selectedButton, newSelectedButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return PressForFrame(keyboard.enterKey);
            Assert.False(mainMenu.Active);
            Assert.True(subMenu.Active);

            yield return PressForFrame(keyboard.enterKey);
            Assert.False(subMenu.Active);
            Assert.True(secondSubMenu.Active);

            yield return PressForFrame(keyboard.enterKey);
            Assert.False(secondSubMenu.Active);
            Assert.False(subMenu.Active);
            Assert.True(mainMenu.Active);
        }


        [UnityTest]
        [ReloadScene]
        public IEnumerator CanStartGame()
        { 
            var sceneManagerAPIStub = new SceneManagerAPIStub();
            SceneManagerAPI.overrideAPI = sceneManagerAPIStub;

            var keyboard = InputSystem.AddDevice<Keyboard>();

            Assert.False(sceneManagerAPIStub.loadedScenes.Contains("SampleScene"));
            yield return PressForFrame(keyboard.enterKey);
            Assert.True(sceneManagerAPIStub.loadedScenes.Contains("SampleScene"));
            
            SceneManagerAPI.overrideAPI = null;
        }
    }
}