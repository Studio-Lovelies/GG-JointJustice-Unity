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
    public class ViaMouse : InputTestFixture
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

            Mouse mouse = InputSystem.AddDevice<Mouse>();

            RectTransform openFirstSubMenuButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton");
            RectTransform openSecondSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");
            RectTransform closeSecondSubMenuButton = secondSubMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (4)");
            RectTransform closeFirstSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (4)");

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            Set(mouse.position, openFirstSubMenuButton.position + openFirstSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            Set(mouse.position, openSecondSubMenuButton.position + openSecondSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(mouse.leftButton);

            Assert.True(secondSubMenu.Active);
            Assert.False(subMenu.Active);

            Set(mouse.position, closeSecondSubMenuButton.position + closeSecondSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            Set(mouse.position, closeFirstSubMenuButton.position + closeFirstSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(mouse.leftButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);
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

            Mouse mouse = InputSystem.AddDevice<Mouse>();

            RectTransform openFirstSubMenuButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton");
            RectTransform openSecondSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");
            RectTransform closeAllSubMenusButton = secondSubMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            Set(mouse.position, openFirstSubMenuButton.position + openFirstSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            Set(mouse.position, openSecondSubMenuButton.position + openSecondSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(mouse.leftButton);

            Assert.True(secondSubMenu.Active);
            Assert.False(subMenu.Active);

            Set(mouse.position, closeAllSubMenusButton.position + closeAllSubMenusButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(mouse.leftButton);

            Assert.True(mainMenu.Active);
            Assert.False(secondSubMenu.Active);
        }

        [UnityTest]
        [ReloadScene("Assets/Scenes/MainMenu.unity")]
        public IEnumerator CanStartGame()
        {
            SceneManagerAPIStub sceneManagerAPIStub = new SceneManagerAPIStub();
            SceneManagerAPI.overrideAPI = sceneManagerAPIStub;

            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            Menu[] menus = InputTestTools.FindInactiveInScene<Menu>();
            Menu mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");

            Mouse mouse = InputSystem.AddDevice<Mouse>();

            RectTransform startGameButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "NewGameButton");

            Set(mouse.position, startGameButton.position + startGameButton.localScale * 0.5f);
            Assert.False(sceneManagerAPIStub.loadedScenes.Contains("Transition - Test Scene"));
            yield return _inputTestTools.PressForFrame(mouse.leftButton);
            Assert.True(sceneManagerAPIStub.loadedScenes.Contains("Transition - Test Scene"));

            SceneManagerAPI.overrideAPI = null;
        }
    }
}