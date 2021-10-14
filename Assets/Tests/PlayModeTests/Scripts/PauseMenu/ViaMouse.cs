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
                yield return InputTools.SetMousePositionWorldSpace(GetButtonCenter(button));
                AssertButtonSelected(button);
            }
        }

        /// <summary>
        /// When the resume button is clicked it should resume the game.
        /// </summary>
        [UnityTest]
        public IEnumerator ResumeButtonResumesGame()
        {
            yield return TestResumeButton(() => InputTools.ClickAtPositionWorldSpace(GetButtonCenter(GetButton(RESUME_BUTTON_NAME))));
        }
        
        /// <summary>
        /// When the settings button is clicked it should open the settings menu.
        /// </summary>
        [UnityTest]
        public IEnumerator SettingsMenuCanBeOpenedAndClosed()
        {
            yield return TestSettingsButton(() => InputTools.ClickAtPositionWorldSpace(GetButtonCenter(GetButton(SETTINGS_BUTTON_NAME))));
        }

        /// <summary>
        /// When the main menu button is clicked the game should return to the main menu.
        /// </summary>
        [UnityTest]
        public IEnumerator MainMenuButtonReturnsToMainMenu()
        {
            yield return TestMainMenuButton(() => InputTools.ClickAtPositionWorldSpace(GetButtonCenter(GetButton(MAIN_MENU_BUTTON_NAME))));
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
            yield return InputTools.ClickAtPositionWorldSpace(new Vector2(10, 10));
            AssertButtonSelected(button);
        }

        /// <summary>
        /// Get the center position of a button with its pivot on the middle left.
        /// </summary>
        /// <param name="button">The button to get the center of.</param>
        /// <returns>The position of the center of the button.</returns>
        private Vector2 GetButtonCenter(Button button)
        {
            return button.transform.position + Vector3.right * (button.GetComponent<RectTransform>().rect.size.x / 2) * CanvasScale;
        }
    }
}
