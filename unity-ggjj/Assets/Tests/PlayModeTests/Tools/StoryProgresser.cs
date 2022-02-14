using System.Collections;
using UnityEngine;

namespace Tests.PlayModeTests.Tools
{
    public class StoryProgresser
    {
        private InputTestTools _inputTestTools = new InputTestTools();
        private DialogueController _dialogueController;
        
        public StoryProgresser()
        {
            _dialogueController = Object.FindObjectOfType<DialogueController>();
        }
        
        /// <summary>
        /// Holds the X Key until a DialogueController is not busy
        /// </summary>
        public IEnumerator ProgressStory()
        {
            _inputTestTools.Press(_inputTestTools.Keyboard.xKey);
            yield return TestTools.WaitForState(() => !_dialogueController.IsBusy);
            _inputTestTools.Release(_inputTestTools.Keyboard.xKey);
        }
    }
}