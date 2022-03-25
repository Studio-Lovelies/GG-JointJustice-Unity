using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        public IEnumerator SelectChoice(int choiceIndex)
        {
            var choice = Object.FindObjectOfType<ChoiceMenu>().transform.GetChild(choiceIndex).GetComponent<Selectable>();
            choice.Select();
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
        }
    }
}