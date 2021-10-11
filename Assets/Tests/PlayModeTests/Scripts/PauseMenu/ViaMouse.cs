using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayModeTests.Scripts.PauseMenu
{
    public class ViaMouse : PauseMenuTests
    {
        /// <summary>
        /// Check if all buttons in the menu are highlighted when the mouse is hovered over them.
        /// </summary>
        [UnityTest]
        public IEnumerator PauseMenuCanBeNavigated()
        {
            yield return ToggleMenu();
            foreach (var button in Buttons)
            {
                yield return InputTools.SetMousePositionWorldSpace(button.transform.position);
                AssertButtonSelected(button);
            }
        }

        /// <summary>
        /// When the resume button is clicked it should resume the game.
        /// </summary>
        [UnityTest]
        public IEnumerator ResumeButtonResumesGame()
        {
            yield return TestResumeButton(() => InputTools.ClickAtPosition(GetButton(RESUME_BUTTON_NAME).transform.position));
        }
        
        /// <summary>
        /// When the settings button is clicked it should open the settings menu.
        /// </summary>
        [UnityTest]
        public IEnumerator SettingsMenuCanBeOpenedAndClosed()
        {
            yield return TestSettingsButton(() => InputTools.ClickAtPosition(GetButton(SETTINGS_BUTTON_NAME).transform.position));
        }

        /// <summary>
        /// When the main menu button is clicked the game should return to the main menu.
        /// </summary>
        [UnityTest]
        public IEnumerator MainMenuButtonReturnsToMainMenu()
        {
            yield return TestMainMenuButton(() => InputTools.ClickAtPosition(GetButton(MAIN_MENU_BUTTON_NAME).transform.position));
        }

        /// <summary>
        /// When something that isn't a Selectable is clicked the
        /// currently selected Selectable should remain selected.
        /// </summary>
        [UnityTest]
        public IEnumerator ClickingOnBackgroundDoesNotDeselectButton()
        {
            yield return ToggleMenu();
            Button button = GetButton(RESUME_BUTTON_NAME);
            AssertButtonSelected(button);
            yield return InputTools.ClickAtPosition(Vector2.zero);
            AssertButtonSelected(button);
        }
    }
}
