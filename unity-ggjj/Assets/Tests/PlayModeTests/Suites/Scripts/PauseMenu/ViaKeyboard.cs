using System.Collections;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Suites.Scripts.PauseMenu
{
    public class ViaKeyboard : PauseMenuTests
    {
        /// <summary>
        /// Tests that the pause menu opens and closes and the escape key is pressed.
        /// </summary>
        [UnityTest]
        public IEnumerator PauseMenuCanBeOpenedAndClosed()
        {
            AssertMenuInactive();
            yield return inputTestTools.PressForFrame(Keyboard.escapeKey);
            AssertMenuActive();
            yield return inputTestTools.PressForFrame(Keyboard.escapeKey);
            AssertMenuInactive();
        }

        /// <summary>
        /// Tests that the pause menu can be navigated using the up and down arrow keys.
        /// Makes sure the menu cannot be navigated using the left and right arrow keys.
        /// Tests the wrapping of the menu buttons (pressing up on the top button
        /// selects the button at the bottom.
        /// </summary>
        [UnityTest]
        public IEnumerator PauseMenuCanBeNavigated()
        {
            yield return inputTestTools.PressForFrame(Keyboard.escapeKey);
            var resumeButton = GetButton(RESUME_BUTTON_NAME);
            var settingsButton = GetButton(SETTINGS_BUTTON_NAME);
            var mainMenuButton = GetButton(MAIN_MENU_BUTTON_NAME);

            // Menu can be navigated down and wrap from bottom to top
            for (var i = 0; i < 2; i++)
            { 
                AssertButtonSelected(resumeButton);
                yield return inputTestTools.PressForFrame(Keyboard.downArrowKey);
                AssertButtonSelected(settingsButton);
                yield return inputTestTools.PressForFrame(Keyboard.downArrowKey);
                AssertButtonSelected(mainMenuButton);
                yield return inputTestTools.PressForFrame(Keyboard.downArrowKey);
            }
            
            // Menu can be navigated up and wrap from top to bottom
            for (var i = 0; i < 2; i++)
            {
                AssertButtonSelected(resumeButton);
                yield return inputTestTools.PressForFrame(Keyboard.upArrowKey);
                AssertButtonSelected(mainMenuButton);
                yield return inputTestTools.PressForFrame(Keyboard.upArrowKey);
                AssertButtonSelected(settingsButton);
                yield return inputTestTools.PressForFrame(Keyboard.upArrowKey);
            }

            // Navigating left and right should not select different buttons
            yield return inputTestTools.PressForFrame(Keyboard.leftArrowKey);
            AssertButtonSelected(resumeButton);
            yield return inputTestTools.PressForFrame(Keyboard.rightArrowKey);
            AssertButtonSelected(resumeButton);
        }

        /// <summary>
        /// When the resume button is clicked it should resume the game.
        /// </summary>
        [UnityTest]
        public IEnumerator ResumeButtonResumesGame()
        {
            yield return TestResumeButton(() => inputTestTools.PressForFrame(Keyboard.enterKey));
        }
        
        /// <summary>
        /// When the settings button is clicked it should open the settings menu.
        /// </summary>
        [UnityTest]
        public IEnumerator SettingsMenuCanBeOpenedAndClosed()
        {
            yield return TestSettingsButton(NavigateToAndClickSettingsButton);
        }
        
        /// <summary>
        /// When the main menu button is clicked the game should return to the main menu.
        /// </summary>
        [UnityTest]
        public IEnumerator MainMenuButtonReturnsToMainMenu()
        {
            yield return TestMainMenuButton(NavigateToAndClickMainMenuButton);
        }

        /// <summary>
        /// Coroutine used by SettingsMenuCanBeOpenedAndClosed to navigate to the settings button.
        /// </summary>
        private IEnumerator NavigateToAndClickSettingsButton()
        {
            yield return inputTestTools.PressForFrame(Keyboard.downArrowKey);
            yield return inputTestTools.PressForFrame(Keyboard.enterKey);
        }

        /// <summary>
        /// Coroutine used by MainMenuButtonReturnsToMainMenu to navigate to the main menu button.
        /// </summary>
        /// <returns></returns>
        private IEnumerator NavigateToAndClickMainMenuButton()
        {
            yield return inputTestTools.PressForFrame(Keyboard.upArrowKey);
            yield return inputTestTools.PressForFrame(Keyboard.enterKey);
        }
    }
}
