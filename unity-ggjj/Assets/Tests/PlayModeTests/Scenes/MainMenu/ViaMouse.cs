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
    public class ViaMouse
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        private Mouse Mouse => _inputTestTools.mouse;

        [SetUp]
        public void Setup()
        {
            _inputTestTools.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            _inputTestTools.TearDown();
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("MainMenu");
        }

        [UnityTest]
        public IEnumerator CanEnterAndCloseTwoSubMenusIndividually()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            var menus = TestTools.FindInactiveInScene<Menu>();
            var mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            var subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            var secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");
            
            var openFirstSubMenuButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton");
            var openSecondSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");
            var closeSecondSubMenuButton = secondSubMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (4)");
            var closeFirstSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (4)");

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return _inputTestTools.SetMouseScreenSpacePosition(openFirstSubMenuButton.position + openFirstSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(Mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            yield return _inputTestTools.SetMouseScreenSpacePosition(openSecondSubMenuButton.position + openSecondSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(Mouse.leftButton);

            Assert.True(secondSubMenu.Active);
            Assert.False(subMenu.Active);

            yield return _inputTestTools.SetMouseScreenSpacePosition(closeSecondSubMenuButton.position + closeSecondSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(Mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            yield return _inputTestTools.SetMouseScreenSpacePosition(closeFirstSubMenuButton.position + closeFirstSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(Mouse.leftButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);
        }

        [UnityTest]
        public IEnumerator CanEnterAndCloseTwoSubMenusWithCloseAllButton()
        {
            // as the containing GameObjects are enabled, `GameObject.Find()` will not find them
            // and we query all existing menus instead
            var menus = TestTools.FindInactiveInScene<Menu>();
            var mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            var subMenu = menus.First(menu => menu.gameObject.name == "TestSubMenu");
            var secondSubMenu = menus.First(menu => menu.gameObject.name == "TestDoubleSubMenu");
            
            var openFirstSubMenuButton = mainMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton");
            var openSecondSubMenuButton = subMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");
            var closeAllSubMenusButton = secondSubMenu.gameObject.GetComponentsInChildren<RectTransform>().First(menuItem => menuItem.gameObject.name == "LoadButton (1)");

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return _inputTestTools.SetMouseScreenSpacePosition(openFirstSubMenuButton.position + openFirstSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(Mouse.leftButton);

            Assert.True(subMenu.Active);
            Assert.False(mainMenu.Active);

            yield return _inputTestTools.SetMouseScreenSpacePosition(openSecondSubMenuButton.position + openSecondSubMenuButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(Mouse.leftButton);

            Assert.True(secondSubMenu.Active);
            Assert.False(subMenu.Active);

            yield return _inputTestTools.SetMouseScreenSpacePosition(closeAllSubMenusButton.position + closeAllSubMenusButton.localScale * 0.5f);
            yield return _inputTestTools.PressForFrame(Mouse.leftButton);

            Assert.True(mainMenu.Active);
            Assert.False(secondSubMenu.Active);
        }

        [UnityTest]
        public IEnumerator CanStartGame()
        {
            var menus = TestTools.FindInactiveInScene<Menu>();
            var mainMenu = menus.First(menu => menu.gameObject.name == "MenuButtons");
            var startGameButton = mainMenu.gameObject.GetComponentsInChildren<Transform>().First(menuItem => menuItem.gameObject.name == "NewGameButton");
            yield return _inputTestTools.ClickAtScreenSpacePosition(startGameButton.position + startGameButton.localScale * 0.5f);
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "Game");
        }
    }
}
