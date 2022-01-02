using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

namespace Tests.PlayModeTests.Scripts.TextDecoder
{
    public class DialogueTests
    {
        private const string SERIALIZED_INK_STARTING_WITH_MODE_PLACEHOLDER = "{\"inkVersion\":20,\"root\":[[\"^&MODE:__MODE__\",\"\\n\",\"^&SPEAK:Ross\",\"\\n\",\"^After all the work I put into those levels...\",\"\\n\",\"end\",[\"done\",{\"#f\":5,\"#n\":\"g-0\"}],null],\"done\",{\"#f\":1}],\"listDefs\":{}}";

        private static IEnumerable<DialogueControllerMode> AllDialogueControllerModes => (DialogueControllerMode[])Enum.GetValues(typeof(DialogueControllerMode));

        [Test]
        [TestCaseSource(nameof(AllDialogueControllerModes))]
        public void CorrectlyParsesModeFromContent(DialogueControllerMode dialogueControllerMode)
        {
            global::Dialogue testDialogue = new global::Dialogue(new TextAsset(SERIALIZED_INK_STARTING_WITH_MODE_PLACEHOLDER.Replace("__MODE__", dialogueControllerMode.ToString())));
            Assert.AreEqual(dialogueControllerMode, testDialogue.ScriptMode);
        }

        [Test]
        public void ThrowsOnIncorrectMode()
        {
            global::Dialogue testDialogue = new global::Dialogue(new TextAsset(SERIALIZED_INK_STARTING_WITH_MODE_PLACEHOLDER));
            var exception = Assert.Throws<NotSupportedException>(() => {
                // ReSharper disable once UnusedVariable "Attempting to get this value is expected to throw before the value is assigned"
                var empty = testDialogue.ScriptMode;
            });
            StringAssert.Contains("is not supported", exception.Message);
            foreach (DialogueControllerMode mode in AllDialogueControllerModes)
            {
                StringAssert.Contains(mode.ToString(), exception.Message);
            }
        }

        private const string SERIALIZED_INK_STARTING_WITHOUT_MODE_PLACEHOLDER = "{\"inkVersion\":20,\"root\":[[\"^&SPEAK:Ross\",\"\\n\",\"^After all the work I put into those levels...\",\"\\n\",\"end\",[\"done\",{\"#f\":5,\"#n\":\"g-0\"}],null],\"done\",{\"#f\":1}],\"listDefs\":{}}";
        [Test]
        public void ThrowsOnMissingMode()
        {
            global::Dialogue testDialogue = new global::Dialogue(new TextAsset(SERIALIZED_INK_STARTING_WITHOUT_MODE_PLACEHOLDER));
            var exception = Assert.Throws<NotSupportedException>(() => {
                // ReSharper disable once UnusedVariable "Attempting to get this value is expected to throw before the value is assigned"
                var empty = testDialogue.ScriptMode;
            });
            StringAssert.Contains("The first line of each .ink script needs to begin with either", exception.Message);
            foreach (DialogueControllerMode mode in AllDialogueControllerModes)
            {
                StringAssert.Contains(mode.ToString(), exception.Message);
            }
        }
    }
}
