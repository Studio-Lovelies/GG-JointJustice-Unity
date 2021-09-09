using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Tests.PlayModeTests.Tools
{
    public class InputTestTools : InputTestFixture
    {
        public Keyboard Keyboard { get; } = InputSystem.AddDevice<Keyboard>();
        
        /// <summary>
        /// Start this coroutine to press a specified key for one frame.
        /// </summary>
        /// <param name="control">The key to press.</param>
        public IEnumerator PressForFrame(ButtonControl control)
        {
            yield return null;
            Press(control);
            yield return null;
            Release(control);
            yield return null;
        }

        /// <summary>
        /// Start this coroutine to press a specified key for a specified number of seconds.
        /// </summary>
        /// <param name="control">The key to press.</param>
        /// <param name="seconds">The number of seconds to press the key for.</param>
        public IEnumerator PressForSeconds(ButtonControl control, float seconds)
        {
            yield return null;
            Press(control);
            yield return new WaitForSeconds(seconds);
            Release(control);
            yield return null;
        }

        /// <summary>
        /// Start this coroutine to press a specified key every frame for a specified number of frames.
        /// </summary>
        /// <param name="control">The key to press.</param>
        /// <param name="repeats">The number of frames to press the key for.</param>
        public IEnumerator RepeatPressForFrames(ButtonControl control, int repeats)
        {
            for (int i = 0; i < repeats; i++)
            {
                Press(control);
                yield return null;
                Release(control);
                yield return null;
            }
        }

        /// <summary>
        /// Start this coroutine to press a specified key every repeatedly,
        /// holding for a specified number of seconds each time the key is pressed.
        /// </summary>
        /// <param name="control">The key to press.</param>
        /// <param name="seconds">The number of seconds to hold the key for each press.</param>
        /// <param name="repeats">The number of times to press the key.</param>
        /// <returns></returns>
        public IEnumerator RepeatPressForSeconds(ButtonControl control, float seconds, int repeats)
        {
            for (int i = 0; i < repeats; i++)
            {
                Press(control);
                yield return new WaitForSeconds(seconds);
                Release(control);
                yield return null;
            }
        }
    }
}
