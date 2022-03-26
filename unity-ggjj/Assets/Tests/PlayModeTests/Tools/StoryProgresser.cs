using System.Collections;
using UnityEngine;

namespace Tests.PlayModeTests.Tools
{
    public class StoryProgresser : InputTestTools
    {
        /// <summary>
        /// Holds the X Key until an AppearingDialogueController is not printing text
        /// </summary>
        public IEnumerator ProgressStory()
        {
            Press(Keyboard.xKey);
            var appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
            yield return TestTools.WaitForState(() => !appearingDialogueController.IsPrintingText);
            Release(Keyboard.xKey);
        }
    }
}