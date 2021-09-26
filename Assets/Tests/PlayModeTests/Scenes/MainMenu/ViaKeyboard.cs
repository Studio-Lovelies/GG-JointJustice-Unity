using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scenes.MainMenu
{
    public class ViaKeyboard : InputTestFixture
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        [UnityTest]
        [ReloadScene("Assets/Scenes/MainMenu.unity")]
        public IEnumerator CanEnterAndCloseTwoSubMenusIndividually()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            Menu[] menus = InputTestTools.FindInactiveInScene<Menu>();
            Menu mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            Menu subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            Menu secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");

            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

            string selectedButton = mainMenu.SelectedButton.name;
            yield return _inputTestTools.PressForFrame(keyboard.rightArrowKey);
            string newSelectedButton = mainMenu.SelectedButton.name;
            Assert.AreNotEqual(selectedButton, newSelectedButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return _inputTestTools.PressForFrame(keyboard.enterKey);
            Assert.False(mainMenu.Active);
            Assert.True(subMenu.Active);

            yield return _inputTestTools.PressForFrame(keyboard.enterKey);
            Assert.False(subMenu.Active);
            Assert.True(secondSubMenu.Active);

            yield return _inputTestTools.PressForFrame(keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(keyboard.downArrowKey);

            yield return _inputTestTools.PressForFrame(keyboard.enterKey);
            Assert.False(secondSubMenu.Active);
            Assert.True(subMenu.Active);

            yield return _inputTestTools.PressForFrame(keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(keyboard.downArrowKey);

            yield return _inputTestTools.PressForFrame(keyboard.enterKey);
            Assert.False(subMenu.Active);
            Assert.True(mainMenu.Active);
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/MainMenu.unity")]
        public IEnumerator CanEnterAndCloseTwoSubMenusWithCloseAllButton()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            Menu[] menus = InputTestTools.FindInactiveInScene<Menu>();
            Menu mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            Menu subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            Menu secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");

            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            string selectedButton = mainMenu.SelectedButton.name;
            yield return _inputTestTools.PressForFrame(keyboard.rightArrowKey);
            string newSelectedButton = mainMenu.SelectedButton.name;
            Assert.AreNotEqual(selectedButton, newSelectedButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return _inputTestTools.PressForFrame(keyboard.enterKey);
            Assert.False(mainMenu.Active);
            Assert.True(subMenu.Active);

            yield return _inputTestTools.PressForFrame(keyboard.enterKey);
            Assert.False(subMenu.Active);
            Assert.True(secondSubMenu.Active);

            yield return _inputTestTools.PressForFrame(keyboard.enterKey);
            Assert.False(secondSubMenu.Active);
            Assert.False(subMenu.Active);
            Assert.True(mainMenu.Active);
        }


        [UnityTest]
        [ReloadScene("Assets/Scenes/MainMenu.unity")]
        public IEnumerator CanStartGame()
        {
            SceneManagerAPIStub sceneManagerAPIStub = new SceneManagerAPIStub();
            SceneManagerAPI.overrideAPI = sceneManagerAPIStub;

            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();

            Assert.False(sceneManagerAPIStub.loadedScenes.Contains("Transition - Test Scene"));
            yield return _inputTestTools.PressForFrame(keyboard.enterKey);
            Assert.True(sceneManagerAPIStub.loadedScenes.Contains("Transition - Test Scene"));

            SceneManagerAPI.overrideAPI = null;
        }
    }
}