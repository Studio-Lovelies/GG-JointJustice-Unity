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
    public class ViaMouse : InputTestFixture
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

            var mouse = InputSystem.AddDevice<Mouse>();

            var openFirstSubMenuButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton");
            var openSecondSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");
            var closeSecondSubMenuButton = secondSubMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (4)");
            var closeFirstSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (4)");

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            Set(mouse.position, openFirstSubMenuButton.position + openFirstSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(mouse.leftButton);
            
            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            Set(mouse.position, openSecondSubMenuButton.position + openSecondSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(mouse.leftButton);

            Assert.True(secondSubMenu.Active);
            Assert.False(subMenu.Active);

            Set(mouse.position, closeSecondSubMenuButton.position + closeSecondSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            Set(mouse.position, closeFirstSubMenuButton.position + closeFirstSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(mouse.leftButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);
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

            var mouse = InputSystem.AddDevice<Mouse>();

            var openFirstSubMenuButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton");
            var openSecondSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");
            var closeAllSubMenusButton = secondSubMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            Set(mouse.position, openFirstSubMenuButton.position + openFirstSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            Set(mouse.position, openSecondSubMenuButton.position + openSecondSubMenuButton.localScale * 0.5f);
            yield return PressForFrame(mouse.leftButton);

            Assert.True(secondSubMenu.Active);
            Assert.False(subMenu.Active);

            Set(mouse.position, closeAllSubMenusButton.position + closeAllSubMenusButton.localScale * 0.5f);
            yield return PressForFrame(mouse.leftButton);

            Assert.True(mainMenu.Active);
            Assert.False(secondSubMenu.Active);
        }

        [UnityTest]
        [ReloadScene]
        public IEnumerator CanStartGame()
        { 
            var sceneManagerAPIStub = new SceneManagerAPIStub();
            SceneManagerAPI.overrideAPI = sceneManagerAPIStub;

            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            var menus = Resources.FindObjectsOfTypeAll<Menu>();
            var mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");

            var mouse = InputSystem.AddDevice<Mouse>();

            var startGameButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "NewGameButton");

            Set(mouse.position, startGameButton.position + startGameButton.localScale * 0.5f);
            Assert.False(sceneManagerAPIStub.loadedScenes.Contains("SampleScene"));
            yield return PressForFrame(mouse.leftButton);
            Assert.True(sceneManagerAPIStub.loadedScenes.Contains("SampleScene"));

            SceneManagerAPI.overrideAPI = null;
        }
    }
}