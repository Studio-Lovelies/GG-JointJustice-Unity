using System.Collections;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayModeTests.Scripts.PauseMenu
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
            yield return InputTools.PressForFrame(Keyboard.escapeKey);
            AssertMenuActive();
            yield return InputTools.PressForFrame(Keyboard.escapeKey);
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
            yield return InputTools.PressForFrame(Keyboard.escapeKey);
            Button resumeButton = GetButton(RESUME_BUTTON_NAME);
            Button settingsButton = GetButton(SETTINGS_BUTTON_NAME);
            Button mainMenuButton = GetButton(MAIN_MENU_BUTTON_NAME);
            
            // Menu can be navigated down and wrap from bottom to top
            for (int i = 0; i < 2; i++)
            { 
                AssertButtonSelected(resumeButton);
                yield return InputTools.PressForFrame(Keyboard.downArrowKey);
                AssertButtonSelected(settingsButton);
                yield return InputTools.PressForFrame(Keyboard.downArrowKey);
                AssertButtonSelected(mainMenuButton);
                yield return InputTools.PressForFrame(Keyboard.downArrowKey);
            }
            
            // Menu can be navigated up and wrap from top to bottom
            for (int i = 0; i < 2; i++)
            {
                AssertButtonSelected(resumeButton);
                yield return InputTools.PressForFrame(Keyboard.upArrowKey);
                AssertButtonSelected(mainMenuButton);
                yield return InputTools.PressForFrame(Keyboard.upArrowKey);
                AssertButtonSelected(settingsButton);
                yield return InputTools.PressForFrame(Keyboard.upArrowKey);
            }

            // Navigating left and right should not select different buttons
            yield return InputTools.PressForFrame(Keyboard.leftArrowKey);
            AssertButtonSelected(resumeButton);
            yield return InputTools.PressForFrame(Keyboard.rightArrowKey);
            AssertButtonSelected(resumeButton);
        }

        /// <summary>
        /// When the resume button is clicked it should resume the game.
        /// </summary>
        [UnityTest]
        public IEnumerator ResumeButtonResumesGame()
        {
            yield return TestResumeButton(() => InputTools.PressForFrame(Keyboard.enterKey));
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
            yield return NavigateToAndClickMainMenuButton();
        }

        /// <summary>
        /// Coroutine used by SettingsMenuCanBeOpenedAndClosed to navigate to the settings button.
        /// </summary>
        private IEnumerator NavigateToAndClickSettingsButton()
        {
            yield return InputTools.PressForFrame(Keyboard.downArrowKey);
            yield return InputTools.PressForFrame(Keyboard.enterKey);
        }

        /// <summary>
        /// Coroutine used by MainMenuButtonReturnsToMainMenu to navigate to the main menu button.
        /// </summary>
        /// <returns></returns>
        private IEnumerator NavigateToAndClickMainMenuButton()
        {
            yield return InputTools.PressForFrame(Keyboard.upArrowKey);
            yield return InputTools.PressForFrame(Keyboard.enterKey);
        }
    }
}
