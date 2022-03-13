using System.Collections;
using UnityEngine;

namespace Tests.PlayModeTests.Tools
{
    public class StoryProgresser
    {
        private readonly AppearingDialogueController _appearingDialogueController;
        private readonly InputTest _inputTest;
        
        public StoryProgresser(InputTest inputTest)
        {
            _inputTest = inputTest;
            _appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
        }
        
        /// <summary>
        /// Holds the X Key until an AppearingDialogueController is not printing text
        /// </summary>
        public IEnumerator ProgressStory()
        {
            _inputTest.Press(_inputTest.Keyboard.xKey);
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            _inputTest.Release(_inputTest.Keyboard.xKey);
        }
    }
}