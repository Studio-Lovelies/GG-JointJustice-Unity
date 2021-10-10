using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts.PauseMenu
{
    public class PauseMenuKeyboardTests
    {
        private Menu _pauseMenu;
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        private Keyboard Keyboard => _inputTestTools.Keyboard;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            SceneManager.LoadScene("Inky-TestScene");
            yield return null;
            _pauseMenu = InputTestTools.FindInactiveInSceneByName<Menu>("PauseMenu");
            yield return null;
        }
    
        [UnityTest]
        public IEnumerator PauseMenuCanBeOpenedAndClosed()
        {
            Assert.IsFalse(_pauseMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsTrue(_pauseMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsFalse(_pauseMenu.isActiveAndEnabled);
        }

        [UnityTest]
        public IEnumerator PauseMenuCanBeNavigated()
        {
            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Button[] buttons = Object.FindObjectsOfType<Button>();
            Button resumeButton = buttons.SingleOrDefault(button => button.name == "ResumeButton");
            Button settingsButton = buttons.SingleOrDefault(button => button.name == "SettingsButton");
            Button mainMenuButton = buttons.SingleOrDefault(button => button.name == "MainMenuButton");
            
            // Menu can be navigated down and wrap from bottom to top
            for (int i = 0; i < 2; i++)
            { 
                VerifySelectedGameObject(resumeButton);
                yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
                VerifySelectedGameObject(settingsButton);
                yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
                VerifySelectedGameObject(mainMenuButton);
                yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
            }
            
            // Menu can be navigated up and wrap from top to bottom
            for (int i = 0; i < 2; i++)
            {
                VerifySelectedGameObject(resumeButton);
                yield return _inputTestTools.PressForFrame(Keyboard.upArrowKey);
                VerifySelectedGameObject(mainMenuButton);
                yield return _inputTestTools.PressForFrame(Keyboard.upArrowKey);
                VerifySelectedGameObject(settingsButton);
                yield return _inputTestTools.PressForFrame(Keyboard.upArrowKey);
            }

            // Navigating left and right should not select different buttons
            yield return _inputTestTools.PressForFrame(Keyboard.leftArrowKey);
            VerifySelectedGameObject(resumeButton);
            yield return _inputTestTools.PressForFrame(Keyboard.rightArrowKey);
            VerifySelectedGameObject(resumeButton);
        }

        [UnityTest]
        public IEnumerator ResumeButtonResumesGame()
        {
            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsTrue(_pauseMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.IsFalse(_pauseMenu.isActiveAndEnabled);
        }
        
        [UnityTest]
        public IEnumerator SettingsMenuCanBeOpenedAndClosed()
        {
            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsTrue(_pauseMenu.isActiveAndEnabled);
            Assert.IsNull(GameObject.Find("SettingsMenu"));
            yield return _inputTestTools.PressForFrame(Keyboard.downArrowKey);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            GameObject settingsMenu = GameObject.Find("SettingsMenu");
            Assert.IsTrue(settingsMenu.activeInHierarchy);
            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsFalse(settingsMenu.activeInHierarchy);
        }
        
        [UnityTest]
        public IEnumerator MainMenuButtonReturnsToMainMenu()
        {
            yield return _inputTestTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsTrue(_pauseMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.upArrowKey);

            SceneManagerAPIStub sceneManagerAPIStub = new SceneManagerAPIStub();
            var currentAPI = SceneManagerAPI.overrideAPI;
            SceneManagerAPI.overrideAPI = sceneManagerAPIStub;
            
            Assert.False(sceneManagerAPIStub.loadedScenes.Contains("MainMenu"));
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);
            Assert.True(sceneManagerAPIStub.loadedScenes.Contains("MainMenu"));
            SceneManagerAPI.overrideAPI = currentAPI;
        }
        
        private void VerifySelectedGameObject(Button button)
        {
            Assert.AreEqual(button.gameObject, EventSystem.current.currentSelectedGameObject);
        }
    }
}
