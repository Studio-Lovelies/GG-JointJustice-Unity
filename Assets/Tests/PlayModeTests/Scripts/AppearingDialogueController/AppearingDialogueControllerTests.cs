using System;
using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts.AppearingDialogueController
{
    public class AppearingDialogueControllerTests
    {
        private const string TEST_TEXT =
            "Lorem ipsum dolor sit amet consectetur adipiscing elit Integer luctus leo nec mi pulvinar eget molestie purus gravida Aliquam imperdiet pharetra massa vel aliquam Pellentesq";

        private global::AppearingDialogueController _appearingDialogueController;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/AppearingDialogueController - Test Scene.unity",  new LoadSceneParameters(LoadSceneMode.Additive));
            _appearingDialogueController = Object.FindObjectOfType<global::AppearingDialogueController>();
            _appearingDialogueController.AutoSkip = false;
        }

        [UnityTest]
        public IEnumerator DialogueAppearsAtCorrectSpeed()
        {
            yield return TestCharacterDelay(0.7f, 0.1f, 0.5f, 5, TEST_TEXT,
                i => _appearingDialogueController.CharacterDelay = i);
        }

        [UnityTest]
        public IEnumerator PunctuationAppearsAtCorrectSpeed()
        {
            yield return TestCharacterDelay(0.7f, 0.1f, 0.5f, 5, ".?!;:/.?!;:/",
                i => _appearingDialogueController.DefaultPunctuationDelay = i);
        }

        [UnityTest]
        public IEnumerator SkippingCanBeDisabled()
        {
            _appearingDialogueController.SkippingDisabled = true;
            InputTestTools inputTestTools = new InputTestTools();
            inputTestTools.Press(inputTestTools.Keyboard.xKey);
            yield return TestCharacterDelay(0.7f, 0.1f, 0.5f, 5, TEST_TEXT,
                i => _appearingDialogueController.CharacterDelay = i);
        }

        [UnityTest]
        public IEnumerator DialogueCanBeContinuedWithoutClearingSpeechPanel()
        {
            const string TEST_TEXT_1 = "Test text 1";
            const string TEST_TEXT_2 = "Test text 2";

            _appearingDialogueController.PrintText(TEST_TEXT_1);
            yield return TestTools.WaitForState(() => _appearingDialogueController.Text == TEST_TEXT_1);
            Assert.AreEqual(TEST_TEXT_1, _appearingDialogueController.Text);
            _appearingDialogueController.ContinueDialogue = true;
            _appearingDialogueController.PrintText(TEST_TEXT_2);
            yield return TestTools.WaitForState(() =>
                _appearingDialogueController.Text == $"{TEST_TEXT_1} {TEST_TEXT_2}");
            Assert.AreEqual($"{TEST_TEXT_1} {TEST_TEXT_2}", _appearingDialogueController.Text);
        }

        [UnityTest]
        public IEnumerator TextCanAppearInstantly()
        {
            _appearingDialogueController.AppearInstantly = true;
            _appearingDialogueController.PrintText(TEST_TEXT);
            Assert.AreEqual(TEST_TEXT, _appearingDialogueController.Text);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TextBoxCanBeHidden()
        {
            var nameBox = GameObject.Find("SpeechPanel");

            Assert.IsTrue(nameBox.activeSelf);
            _appearingDialogueController.TextBoxHidden = true;
            Assert.IsFalse(nameBox.activeSelf);
            yield return null;
        }

        /// <summary>
        /// Method to test speed at which characters print.
        /// Waits a certain amount of time, then checks to see
        /// if the expected number of letters have been printed.
        /// Iterates over a range of speeds down to a target speed.
        /// </summary>
        /// <param name="initialCharacterDelay">The initial delay between characters</param>
        /// <param name="delayDecrement">The amount the delay decreases between each iteration</param>
        /// <param name="minCharacterDelay">The target character delay</param>
        /// <param name="waitTime">The time to test each iteration</param>
        /// <param name="testString">The string to print</param>
        /// <param name="testAction">The action to perform to update the character delay on each test</param>
        /// <returns></returns>
        private IEnumerator TestCharacterDelay(float initialCharacterDelay, float delayDecrement,
            float minCharacterDelay, float waitTime, string testString, Action<float> testAction)
        {
            for (float i = initialCharacterDelay; i > minCharacterDelay; i -= delayDecrement)
            {
                testAction(i);
                _appearingDialogueController.PrintText(testString);

                yield return new WaitForSeconds(waitTime);
                Assert.AreEqual(Mathf.Ceil(waitTime / i), _appearingDialogueController.MaxVisibleCharacters);
            }
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return SceneManager.UnloadSceneAsync("AppearingDialogueController - Test Scene");
        }
    }
}
