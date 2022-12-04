using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Tests.PlayModeTests.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Suites.Scripts.AppearingDialogueController
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
            const string TEXT_TO_PRINT = "Lorem ips";

            _appearingDialogueController.CharacterDelay = 1f;
            var startedAt = DateTime.Now;
            _appearingDialogueController.PrintText(TEXT_TO_PRINT);
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            var firstDuration = DateTime.Now - startedAt;

            _appearingDialogueController.CharacterDelay = 0f;
            startedAt = DateTime.Now;
            _appearingDialogueController.PrintText(TEXT_TO_PRINT);
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            var secondDuration = DateTime.Now - startedAt;

            Assert.Greater(firstDuration, secondDuration);
        }

        [UnityTest]
        public IEnumerator AppearInstantlyImpactsTimeUntilOnLineEndEventIsFired()
        {
            TimeSpan firstDuration;
            TimeSpan secondDuration;

            _appearingDialogueController.AppearInstantly = false;
            var startedAt = DateTime.Now;
            _appearingDialogueController.PrintText(TEST_TEXT);
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            firstDuration = DateTime.Now - startedAt;

            _appearingDialogueController.AppearInstantly = true;
            startedAt = DateTime.Now;
            _appearingDialogueController.PrintText(TEST_TEXT);
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            secondDuration = DateTime.Now - startedAt;

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

        [UnityTest]
        public IEnumerator TextCanBeAutoSkipped()
        {
            var storyProgresser = new StoryProgresser();
            storyProgresser.Setup();
            TestTools.StartGame("AutoSkipTest");
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeGameState>().NarrativeScriptPlayerComponent.NarrativeScriptPlayer;
            
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            var dialogueText = GameObject.Find("Dialogue").GetComponent<TextMeshProUGUI>();
            Assert.AreEqual("Start of test", dialogueText.text);
            narrativeScriptPlayer.Continue();
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            Assert.AreEqual("End of test", dialogueText.text);
        }

        [UnityTest]
        public IEnumerator ContinueArrowAppearsWhenTextIsPrinting()
        {
            var storyProgresser = new StoryProgresser();
            storyProgresser.Setup();
            TestTools.StartGame("AutoSkipTest");
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeGameState>().NarrativeScriptPlayerComponent.NarrativeScriptPlayer;
            var continueArrow = GameObject.Find("ContinueArrow");

            Assert.IsFalse(_appearingDialogueController.IsPrintingText);
            Assert.IsTrue(continueArrow.activeInHierarchy);

            narrativeScriptPlayer.Continue();
            Assert.IsFalse(continueArrow.activeInHierarchy);
            
            yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
            Assert.IsTrue(continueArrow.activeInHierarchy);
        }
    }
}