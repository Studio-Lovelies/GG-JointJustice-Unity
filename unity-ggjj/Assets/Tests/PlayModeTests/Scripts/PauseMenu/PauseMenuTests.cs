using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayModeTests.Scripts.PauseMenu
{
    /// <summary>
    /// Contains methods and properties used in both the keyboard tests and the mouse tests.
    /// </summary>
    public abstract class PauseMenuTests
    {
        protected const string RESUME_BUTTON_NAME = "ResumeButton";
        protected const string SETTINGS_BUTTON_NAME = "SettingsButton";
        protected const string MAIN_MENU_BUTTON_NAME = "MainMenuButton";
        
        protected Menu PauseMenu { get; private set; }
        protected Button[] Buttons { get; private set; }
        protected InputTestTools InputTools { get; }= new InputTestTools();
        protected Keyboard Keyboard => InputTools.Keyboard;
        protected float CanvasScale { get; private set; }

        /// <summary>
        /// Loads the correct scene and gets the required objects before every test.
        /// </summary>
        [UnitySetUp]
        protected IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/AppearingDialogueController - Test Scene.unity",  new LoadSceneParameters());
            PauseMenu = TestTools.FindInactiveInSceneByName<Menu>("PauseMenu");
            Buttons = PauseMenu.GetComponentsInChildren<Button>();
            CanvasScale = GameObject.Find("BaseCanvas").transform.localScale.x;
        }

        /// <summary>
        /// Asserts that the pause menu object is active.
        /// </summary>
        protected void AssertMenuActive()
        {
            Assert.IsTrue(PauseMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Asserts that the pause menu object is inactive.
        /// </summary>
        protected void AssertMenuInactive()
        {
            Assert.IsFalse(PauseMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Opens and closes the menu.
        /// </summary>
        protected IEnumerator ToggleMenu()
        {
            yield return InputTools.PressForFrame(Keyboard.escapeKey);
        }

        /// <summary>
        /// Asserts that a button is selected by the event system.
        /// </summary>
        /// <param name="button">The button to check.</param>
        protected void AssertButtonSelected(Button button)
        {
            Assert.AreEqual(button.gameObject, EventSystem.current.currentSelectedGameObject);
        }
        
        /// <summary>
        /// Gets a button from the menu by name.
        /// </summary>
        /// <param name="buttonName">The name of the button.</param>
        /// <returns>The button found (or null if no button was found).</returns>
        protected Button GetButton(string buttonName)
        {
            return Buttons.SingleOrDefault(button => button.name == buttonName);
        }

        /// <summary>
        /// Tests that the the resume button resumes the game.
        /// Is called by the keyboard and mouse tests which provide a
        /// method of navigating to the correct button.
        /// </summary>
        /// <param name="selectionMethod">A coroutine that navigates to and clicks the resume button.</param>
        protected IEnumerator TestResumeButton(Func<IEnumerator> selectionMethod)
        {
            yield return InputTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsTrue(PauseMenu.isActiveAndEnabled);
            yield return selectionMethod();
            Assert.IsFalse(PauseMenu.isActiveAndEnabled);
        }

        /// <summary>
        /// Tests that the the settings button opens the settings menu, and escape closes it.
        /// Is called by the keyboard and mouse tests which provide a
        /// method of navigating to the correct button.
        /// </summary>
        /// <param name="selectionMethod">A coroutine that navigates to and clicks the settings button.</param>
        protected IEnumerator TestSettingsButton(Func<IEnumerator> selectionMethod)
        {
            yield return InputTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsTrue(PauseMenu.isActiveAndEnabled);
            Assert.IsNull(GameObject.Find("SettingsMenu"));
            yield return selectionMethod();
            GameObject settingsMenu = GameObject.Find("SettingsMenu");
            Assert.IsTrue(settingsMenu.activeInHierarchy);
            yield return InputTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsFalse(settingsMenu.activeInHierarchy);
        }

        /// <summary>
        /// Tests that the the main menu button opens the main menu scene.
        /// Is called by the keyboard and mouse tests which provide a
        /// method of navigating to the correct button.
        /// </summary>
        /// <param name="selectionMethod">A coroutine that navigates to and clicks the main menu button.</param>
        protected IEnumerator TestMainMenuButton(Func<IEnumerator> selectionMethod)
        {
            SceneManagerAPIStub sceneManagerAPIStub = new SceneManagerAPIStub();
            var currentAPI = SceneManagerAPI.overrideAPI;
            SceneManagerAPI.overrideAPI = sceneManagerAPIStub;
            Assert.False(sceneManagerAPIStub.loadedScenes.Contains("MainMenu"));
            yield return InputTools.PressForFrame(Keyboard.escapeKey);
            Assert.IsTrue(PauseMenu.isActiveAndEnabled);
            yield return selectionMethod();
            Assert.True(sceneManagerAPIStub.loadedScenes.Contains("MainMenu"));
            SceneManagerAPI.overrideAPI = currentAPI;
        }
    }
}