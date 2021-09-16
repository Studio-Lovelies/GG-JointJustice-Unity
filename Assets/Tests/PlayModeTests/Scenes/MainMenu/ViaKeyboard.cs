using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
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
            yield return null;
            Press(control);
            yield return null;
            Release(control);
            yield return null;
        }

        [UnityTest]
        [ReloadScene]
        public IEnumerator CanEnterAndCloseTwoSubMenusIndividually()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            Menu[] menus = Resources.FindObjectsOfTypeAll<Menu>();
            Menu mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            Menu subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            Menu secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");

            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

            string selectedButton = mainMenu.SelectedButton.name;
            yield return PressForFrame(keyboard.rightArrowKey);
            string newSelectedButton = mainMenu.SelectedButton.name;
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
            Menu[] menus = Resources.FindObjectsOfTypeAll<Menu>();
            Menu mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            Menu subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            Menu secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");

            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            string selectedButton = mainMenu.SelectedButton.name;
            yield return PressForFrame(keyboard.rightArrowKey);
            string newSelectedButton = mainMenu.SelectedButton.name;
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
            SceneManagerAPIStub sceneManagerAPIStub = new SceneManagerAPIStub();
            SceneManagerAPI.overrideAPI = sceneManagerAPIStub;

            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

            Assert.False(sceneManagerAPIStub.loadedScenes.Contains("Transition - Test Scene"));
            yield return PressForFrame(keyboard.enterKey);
            Assert.True(sceneManagerAPIStub.loadedScenes.Contains("Transition - Test Scene"));

            SceneManagerAPI.overrideAPI = null;
        }
    }
}