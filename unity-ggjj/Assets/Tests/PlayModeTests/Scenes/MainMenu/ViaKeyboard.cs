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
    public class ViaKeyboard
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        private Keyboard Keyboard => _inputTestTools.keyboard;

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
        public IEnumerator UnitySetUp()
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
            var subMenu = menus.First(menu => menu.gameObject.name == "SettingsMenu");
            var secondSubMenu = menus.First(menu => menu.gameObject.name == "ControlsMenu");

            var selectedButton = mainMenu.SelectedButton.name;
            yield return _inputTestTools.PressForFrame(Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.rightArrowKey);
            var newSelectedButton = mainMenu.SelectedButton.name;
            Assert.AreNotEqual(selectedButton, newSelectedButton);

            Assert.True(mainMenu.Active);
            Assert.False(subMenu.Active);

            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.False(mainMenu.Active);
            Assert.True(subMenu.Active);

            for (var i = 0; i < 4; i++)
            {
                yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
            }
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.False(subMenu.Active);
            Assert.True(secondSubMenu.Active);

            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Assert.False(secondSubMenu.Active);
            Assert.True(subMenu.Active);

            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Assert.False(subMenu.Active);
            Assert.True(mainMenu.Active);
        }

        [UnityTest]
        public IEnumerator CanStartGame()
        {
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "Game");
            
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            yield return TestTools.WaitForState(() => narrativeScriptPlayer.NarrativeScriptPlayer.ActiveNarrativeScript.Script.name != "Baby");
        }

        [UnityTest]
        public IEnumerator CanHandleKonamiCode()
        {
            yield return _inputTestTools.PressForFrame(Keyboard.upArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.upArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.bKey);
            yield return _inputTestTools.PressForFrame(Keyboard.aKey);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "Game");
            
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            yield return TestTools.WaitForState(() => narrativeScriptPlayer.NarrativeScriptPlayer.ActiveNarrativeScript.Script.name == "Baby");
        }

        [UnityTest]
        public IEnumerator CanHandleIncorrectKonamiCode()
        {
            yield return _inputTestTools.PressForFrame(Keyboard.upArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.upArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.bKey);
            yield return _inputTestTools.PressForFrame(Keyboard.aKey);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "Game");
            
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            yield return TestTools.WaitForState(() => narrativeScriptPlayer.NarrativeScriptPlayer.ActiveNarrativeScript.Script.name != "Baby");
        }
    }
}