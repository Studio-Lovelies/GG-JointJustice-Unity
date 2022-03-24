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
            yield return TestTools.WaitForState(() => !Object.FindObjectOfType<AppearingDialogueController>().IsPrintingText);
            Release(Keyboard.xKey);
        }
    }
}