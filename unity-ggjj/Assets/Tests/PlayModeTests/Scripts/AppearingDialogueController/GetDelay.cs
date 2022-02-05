using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.AppearingDialogueController
{
    /// <summary>
    /// Tests for <see cref="global::AppearingDialogueController.GetDelay"/>
    /// </summary>
    public class GetDelay
    {
        private const float SPEED_MULTIPLIER = 2;

        private static readonly char[] RegularCharacters = "The quick brown fox jumps over the lazy dog$^+|=".ToCharArray();
        private const float REGULAR_CHARACTER_SPEED = 5;

        private static readonly char[] PunctuationCharacters = "!@#%&*_}\"{:;[\\]".ToCharArray();
        private const float PUNCTUATION_CHARACTER_SPEED = 6;
        private static readonly char[] IgnoredCharacters = "'()-".ToCharArray(); // treated as regular characters

        private static readonly object[][] PunctuationDelays = { new object[]{',', 0.1f}};

        private global::AppearingDialogueController _appearingDialogueController;

        // Unity offers no [OneTimeSetUp]-equivalent for Unity-attributes ( https://forum.unity.com/threads/add-coroutine-version-of-onetimesetup.890092/#post-7643152 )
        private bool _performedOneTimeSetup;
        [UnitySetUp]
        public IEnumerator BeforeEachRun()
        {
            if (_performedOneTimeSetup)
            {
                _appearingDialogueController.SkippingDisabled = default;
                _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER;
                yield break;
            }

            yield return OneTimeSetup();
        }

        private IEnumerator OneTimeSetup()
        {
            _performedOneTimeSetup = true;

            var textMesh = new GameObject().AddComponent<TextMeshProUGUI>();
            yield return new WaitForEndOfFrame();

            // create controller component
            _appearingDialogueController = new GameObject().AddComponent<global::AppearingDialogueController>();
            _appearingDialogueController.CharacterDelay = REGULAR_CHARACTER_SPEED;
            _appearingDialogueController.DefaultPunctuationDelay = PUNCTUATION_CHARACTER_SPEED;
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER;

            // set private properties needed for these tests to run
            var so = new SerializedObject(_appearingDialogueController);
            so.FindProperty("_textBox").objectReferenceValue = textMesh;
            so.FindProperty("_directorActionDecoder").objectReferenceValue = new GameObject().AddComponent<DirectorActionDecoder>();
            var ignoredItemsReference = so.FindProperty("_ignoredCharacters");
            for (var i = 0; i < IgnoredCharacters.Length; i++)
            {
                ignoredItemsReference.InsertArrayElementAtIndex(i);
                ignoredItemsReference.GetArrayElementAtIndex(i).intValue = IgnoredCharacters[i];
            }

            var punctuationDelaysReference = so.FindProperty("_punctuationDelay");
            for (var i = 0; i < PunctuationDelays.Length; i++)
            {
                punctuationDelaysReference.InsertArrayElementAtIndex(i);
                while (punctuationDelaysReference.NextVisible(true))
                {
                    if (punctuationDelaysReference.type == "char")
                    {
                        punctuationDelaysReference.intValue = (char)PunctuationDelays[i][0];
                    }
                    if (punctuationDelaysReference.type == "float")
                    {
                        punctuationDelaysReference.floatValue = (float)PunctuationDelays[i][1];
                        break;
                    }
                }
            }
            so.ApplyModifiedProperties();

            // wait a frame, so that `Start()` is called on the `AppearingDialogueController` we added
            yield return new WaitForEndOfFrame();
        }

        [TestCaseSource(nameof(RegularCharacters))]
        [Test]
        public void VerifyRegularCharacterSpeed(char character)
        {
            Assert.AreEqual(REGULAR_CHARACTER_SPEED / SPEED_MULTIPLIER, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER * 2;
            Assert.AreEqual(REGULAR_CHARACTER_SPEED / (SPEED_MULTIPLIER * 2), _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(PunctuationCharacters))]
        [Test]
        public void VerifyPunctuationCharacterSpeed(char character)
        {
            Assert.AreEqual(PUNCTUATION_CHARACTER_SPEED / SPEED_MULTIPLIER, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER * 2;
            Assert.AreEqual(PUNCTUATION_CHARACTER_SPEED / (SPEED_MULTIPLIER * 2), _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(IgnoredCharacters))]
        [Test]
        public void VerifyIgnoredCharacterSpeed(char character)
        {
            Assert.AreEqual(REGULAR_CHARACTER_SPEED / SPEED_MULTIPLIER, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER * 2;
            Assert.AreEqual(REGULAR_CHARACTER_SPEED / (SPEED_MULTIPLIER * 2), _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(PunctuationDelays))]
        [Test]
        public void VerifyPunctuationDelays(char character, float expectedDelay)
        {
            Assert.AreEqual(expectedDelay / SPEED_MULTIPLIER, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER * 2;
            Assert.AreEqual(expectedDelay / (SPEED_MULTIPLIER * 2), _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(RegularCharacters))]
        [Test]
        public void VerifyRegularCharacterSpeedWithDisabledSkipping(char character)
        {
            _appearingDialogueController.SkippingDisabled = true;
            Assert.AreEqual(REGULAR_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER * 2;
            Assert.AreEqual(REGULAR_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(PunctuationCharacters))]
        [Test]
        public void VerifyPunctuationCharacterSpeedWithDisabledSkipping(char character)
        {
            _appearingDialogueController.SkippingDisabled = true;
            Assert.AreEqual(PUNCTUATION_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER * 2;
            Assert.AreEqual(PUNCTUATION_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(IgnoredCharacters))]
        [Test]
        public void VerifyIgnoredCharacterSpeedWithDisabledSkipping(char character)
        {
            _appearingDialogueController.SkippingDisabled = true;
            Assert.AreEqual(REGULAR_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER * 2;
            Assert.AreEqual(REGULAR_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }


        [TestCaseSource(nameof(PunctuationDelays))]
        [Test]
        public void VerifyPunctuationDelaysWithDisabledSkipping(char character, float expectedDelay)
        {
            _appearingDialogueController.SkippingDisabled = true;
            Assert.AreEqual(expectedDelay, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.SpeedMultiplier = SPEED_MULTIPLIER * 2;
            Assert.AreEqual(expectedDelay, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }
    }
}