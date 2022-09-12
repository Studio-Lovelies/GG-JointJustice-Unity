using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.AppearingDialogueController
{
    /// <summary>
    /// Tests for <see cref="GetDelay"/>
    /// </summary>
    public class GetDelay
    {
        private static readonly char[] RegularCharacters = "The quick brown fox jumps over the lazy dog$^+|=".ToCharArray();
        private const float REGULAR_CHARACTER_SPEED = 5;

        private static readonly char[] PunctuationCharacters = "!@#%&*_}\"{:;[\\]".ToCharArray();
        private const float PUNCTUATION_CHARACTER_SPEED = 6;

        private static readonly char[] IgnoredCharacters = "'()-".ToCharArray(); // treated as regular characters
        private static readonly object[][] PunctuationDelays = { new object[] { ',', 0.1f } }; // edge-case

        private static readonly object[] CharactersAndSpeed = new IEnumerable<object>[]
        {
            RegularCharacters.Select(character => new object[]{character, REGULAR_CHARACTER_SPEED}),
            PunctuationCharacters.Select(character => new object[] { character, PUNCTUATION_CHARACTER_SPEED }),
            IgnoredCharacters.Select(character => new object[] { character, REGULAR_CHARACTER_SPEED }),
            PunctuationDelays
        }.SelectMany(a=>a).ToArray();

        private global::AppearingDialogueController _appearingDialogueController;

        // Unity offers no [OneTimeSetUp]-equivalent for Unity-attributes ( https://forum.unity.com/threads/add-coroutine-version-of-onetimesetup.890092/#post-7643152 )
        private bool _performedOneTimeSetup;

        private GameObject gameObjectInstance;

        [UnitySetUp]
        public IEnumerator BeforeEachRun()
        {
            if (_performedOneTimeSetup)
            {
                _appearingDialogueController.SkippingDisabled = default;
                _appearingDialogueController.SpeedupText = default;
                _appearingDialogueController.SpeedupDelay = 0;
                _appearingDialogueController.CharacterDelay = REGULAR_CHARACTER_SPEED;
                _appearingDialogueController.DefaultPunctuationDelay = PUNCTUATION_CHARACTER_SPEED;
                yield break;
            }

            yield return OneTimeSetup();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Object.DestroyImmediate(gameObjectInstance);
        }

        private IEnumerator OneTimeSetup()
        {
            _performedOneTimeSetup = true;

            gameObjectInstance = new GameObject();
            var textMesh = gameObjectInstance.AddComponent<TextMeshProUGUI>();
            yield return new WaitForEndOfFrame();

            // create controller component
            LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
            _appearingDialogueController = gameObjectInstance.AddComponent<global::AppearingDialogueController>();
            _appearingDialogueController.CharacterDelay = REGULAR_CHARACTER_SPEED;
            _appearingDialogueController.DefaultPunctuationDelay = PUNCTUATION_CHARACTER_SPEED;

            // set private properties needed for these tests to run
            var so = new SerializedObject(_appearingDialogueController);
            so.FindProperty("_textBox").objectReferenceValue = textMesh;
            var textInfoField = _appearingDialogueController.GetType().GetField("_textInfo", BindingFlags.NonPublic | BindingFlags.Instance);
            textInfoField.SetValue(_appearingDialogueController, textMesh.textInfo);
            var ignoredItemsReference = so.FindProperty("_ignoredCharacters");
            for (var i = 0; i < IgnoredCharacters.Length; i++)
            {
                ignoredItemsReference.InsertArrayElementAtIndex(i);
                ignoredItemsReference.GetArrayElementAtIndex(i).intValue = IgnoredCharacters[i];
            }

            var punctuationDelaysReference = so.FindProperty("_specialDelays");
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
            Assert.AreEqual(REGULAR_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.CharacterDelay = REGULAR_CHARACTER_SPEED * 2;
            Assert.AreEqual(REGULAR_CHARACTER_SPEED * 2, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(PunctuationCharacters))]
        [Test]
        public void VerifyPunctuationCharacterSpeed(char character)
        {
            Assert.AreEqual(PUNCTUATION_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.DefaultPunctuationDelay = PUNCTUATION_CHARACTER_SPEED * 2;
            Assert.AreEqual(PUNCTUATION_CHARACTER_SPEED * 2, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(IgnoredCharacters))]
        [Test]
        public void VerifyIgnoredCharacterSpeed(char character)
        {
            Assert.AreEqual(REGULAR_CHARACTER_SPEED, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.CharacterDelay = REGULAR_CHARACTER_SPEED * 2;
            Assert.AreEqual(REGULAR_CHARACTER_SPEED * 2, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(PunctuationDelays))]
        [Test]
        public void VerifySpecialDelays(char character, float expectedDelay)
        {
            Assert.AreEqual(expectedDelay, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
            _appearingDialogueController.CharacterDelay = REGULAR_CHARACTER_SPEED * 2;
            _appearingDialogueController.DefaultPunctuationDelay = PUNCTUATION_CHARACTER_SPEED * 2;
            Assert.AreEqual(expectedDelay, _appearingDialogueController.GetDelay(new TMP_CharacterInfo{character=character}));
        }

        [TestCaseSource(nameof(CharactersAndSpeed))]
        [Test]
        public void VerifySpeedupWorksWithEveryCharacter(char character, float delayInSeconds)
        {
            const int SPEEDUP_IN_SECONDS = 100;
            Assert.AreEqual(delayInSeconds, _appearingDialogueController.GetDelay(new TMP_CharacterInfo { character = character }));
            _appearingDialogueController.SpeedupDelay = SPEEDUP_IN_SECONDS;
            Assert.AreEqual(delayInSeconds, _appearingDialogueController.GetDelay(new TMP_CharacterInfo { character = character }));
            _appearingDialogueController.SpeedupText = true; // speedup ignores all character types
            Assert.AreEqual(SPEEDUP_IN_SECONDS, _appearingDialogueController.GetDelay(new TMP_CharacterInfo { character = character }));
        }

        [TestCaseSource(nameof(CharactersAndSpeed))]
        [Test]
        public void VerifySpeedupOnlyWorksWhenSkippingIsNotDisabled(char character, float delayInSeconds)
        {
            const int SPEEDUP_IN_SECONDS = 100;
            Assert.AreEqual(delayInSeconds, _appearingDialogueController.GetDelay(new TMP_CharacterInfo { character = character }));
            _appearingDialogueController.SpeedupDelay = SPEEDUP_IN_SECONDS;
            Assert.AreEqual(delayInSeconds, _appearingDialogueController.GetDelay(new TMP_CharacterInfo { character = character }));
            _appearingDialogueController.SpeedupText = true;
            _appearingDialogueController.SkippingDisabled = true;
            Assert.AreEqual(delayInSeconds, _appearingDialogueController.GetDelay(new TMP_CharacterInfo { character = character }));
        }
    }
}