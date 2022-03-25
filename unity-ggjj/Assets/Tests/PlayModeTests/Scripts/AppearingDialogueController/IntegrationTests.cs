using System;
using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts.AppearingDialogueController
{
    /// <summary>
    /// Integration tests for <see cref="AppearingDialogueController"/>
    /// </summary>
    public class IntegrationTests
    {
        private global::AppearingDialogueController _appearingDialogueController;
        private const string TEST_TEXT = "Lorem ipsum dolor sit amet consectetur adipiscing elit Integer luctus leo nec mi pulvinar eget molestie purus gravida Aliquam imperdiet pharetra massa vel aliquam Pellentesq";

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            SceneManager.LoadScene("Game");
            yield return null;
            _appearingDialogueController = Object.FindObjectOfType<global::AppearingDialogueController>();
            _appearingDialogueController.AutoSkip = false;
            yield return new WaitForEndOfFrame();
        }

        [UnityTest]
        public IEnumerator GetDelayResultImpactsTimeUntilOnLineEndEventIsFired()
        {
            const string textToPrint = "Lorem ips";

            DateTime startedAt = default;
            DateTime completedAt = default;

            void OnLineEndUpdateCompletionTime()
            {
                completedAt = DateTime.Now;
            }

            TimeSpan firstDuration = default;
            TimeSpan secondDuration = default;

            _appearingDialogueController.OnLineEnd.AddListener(OnLineEndUpdateCompletionTime);
            _appearingDialogueController.CharacterDelay = 1f;
            startedAt = DateTime.Now;
            _appearingDialogueController.PrintText(textToPrint);
            while (completedAt == default)
            {
                yield return new WaitForSeconds(1.0f);
            }
            firstDuration = completedAt - startedAt;
            completedAt = default;

            _appearingDialogueController.CharacterDelay = 0f;
            startedAt = DateTime.Now;
            _appearingDialogueController.PrintText(textToPrint);
            while (completedAt == default)
            {
                yield return new WaitForSeconds(1.0f);
            }
            secondDuration = completedAt - startedAt;

            Assert.Greater(firstDuration, secondDuration);
        }

        [UnityTest]
        public IEnumerator AppearInstantlyImpactsTimeUntilOnLineEndEventIsFired()
        {
            DateTime startedAt = default;
            DateTime completedAt = default;

            void OnLineEndUpdateCompletionTime()
            {
                completedAt = DateTime.Now;
            }

            TimeSpan firstDuration = default;
            TimeSpan secondDuration = default;

            _appearingDialogueController.OnLineEnd.AddListener(OnLineEndUpdateCompletionTime);
            _appearingDialogueController.AppearInstantly = false;
            startedAt = DateTime.Now;
            _appearingDialogueController.PrintText(TEST_TEXT);
            while (completedAt == default)
            {
                yield return new WaitForSeconds(1.0f);
            }
            firstDuration = completedAt - startedAt;
            completedAt = default;

            _appearingDialogueController.AppearInstantly = true;
            startedAt = DateTime.Now;
            _appearingDialogueController.PrintText(TEST_TEXT);
            while (completedAt == default)
            {
                yield return new WaitForSeconds(1.0f);
            }
            secondDuration = completedAt - startedAt;

            Assert.Greater(firstDuration, secondDuration);
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
    }
}