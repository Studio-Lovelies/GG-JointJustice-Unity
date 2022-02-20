using System.Collections;
using UnityEngine;

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
    }
}