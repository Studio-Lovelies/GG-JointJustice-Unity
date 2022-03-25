using System;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Tools
{
    public class StoryProgresser
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        private readonly AppearingDialogueController _appearingDialogueController;
        
        public StoryProgresser()
        {
            _appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
        }
        
        /// <summary>
        /// Holds the X Key until an AppearingDialogueController is not printing text
        /// </summary>
        public IEnumerator ProgressStory()
        {
            _inputTestTools.Press(_inputTestTools.Keyboard.xKey);
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            _inputTestTools.Release(_inputTestTools.Keyboard.xKey);
        }

        /// <summary>
        /// Selects a choice and presses the x key to progress the story
        /// </summary>
        /// <param name="choiceIndex">The index of the choice to select</param>
        /// <param name="gameMode">The GameMode the game is currently using</param>
        public IEnumerator SelectChoice(int choiceIndex, GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Dialogue:
                    var choice = Object.FindObjectOfType<ChoiceMenu>().transform.GetChild(choiceIndex).GetComponent<Selectable>();
                    choice.Select();
                    yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey); 
                    break;
                case GameMode.CrossExamination:
                    yield return choiceIndex switch
                    {
                        0 => _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey),
                        1 => _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey),
                        2 => _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey),
                        _ => throw new ArgumentException($"Choice index can only be 0, 1, or 2 in GameMode {gameMode}")
                    };
                    break;
                default:
                    throw new NotSupportedException($"GameMode '{gameMode}' is not supported");
            }
        }
    }
}