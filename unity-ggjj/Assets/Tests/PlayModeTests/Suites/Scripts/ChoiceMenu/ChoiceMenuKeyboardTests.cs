using System.Collections;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Suites.Scripts.ChoiceMenu
{
    public class ChoiceMenuKeyboardTests
    {
        private readonly StoryProgresser _storyProgresser = new();
        private NarrativeGameState _narrativeGameState;
        private Menu ChoiceMenu { get; set; }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _storyProgresser.Setup();

            yield return SceneManager.LoadSceneAsync("Game");
            TestTools.StartGame("ChoiceMenu");
            
            ChoiceMenu = TestTools.FindInactiveInSceneByName<Menu>("ChoiceMenu");
            _narrativeGameState = Object.FindAnyObjectByType<NarrativeGameState>();
        }

        [UnityTest]
        public IEnumerator ChoiceMenuOpensViaKeyboard()
        {
            yield return TestTools.WaitForState(() => !_narrativeGameState.AppearingDialogueController.IsPrintingText);
            yield return PressX();
            yield return TestTools.WaitForState(() => ChoiceMenu.gameObject.activeInHierarchy);
            yield return PressX();
            yield return TestTools.WaitForState(() => !ChoiceMenu.gameObject.activeInHierarchy);
        }
                    
        private IEnumerator PressX()
        {
            yield return _storyProgresser.PressForFrame(_storyProgresser.keyboard.xKey);
        }
    }
}