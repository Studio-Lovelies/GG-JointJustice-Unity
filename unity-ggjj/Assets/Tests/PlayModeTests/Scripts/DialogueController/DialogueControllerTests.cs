using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

namespace Tests.PlayModeTests.Scripts.DialogueController
{
    public class DialogueControllerTests
    {
        // This constant contains exactly one action and one spoken line
        private const string ActionFollowedByScriptSerializedInk = "{\"inkVersion\":20,\"root\":[[\"^&MODE:Dialogue\",\"\\n\",\"^&SPEAK:Ross\",\"\\n\",\"^After all the work I put into those levels...\",\"\\n\",\"end\",[\"done\",{\"#f\":5,\"#n\":\"g-0\"}],null],\"done\",{\"#f\":1}],\"listDefs\":{}}";

        [Test]
        public void DialogueController_OnNextLine_CallsCorrectEvents()
        {
            // Setup component
            GameObject dialogueControllerGameObject = new GameObject("DialogueController");
            global::DialogueController dialogueController = dialogueControllerGameObject.AddComponent<global::DialogueController>();

            // Setup event listener for action and spoken lines
            List<string> spokenLines = new List<string>();
            UnityEvent<string> spokenLinesEvent = new UnityEvent<string>();
            spokenLinesEvent.AddListener(spokenLine => {
                spokenLines.Add(spokenLine);
            });

            List<string> actionLines = new List<string>();
            UnityEvent<string> actionLinesEvent = new UnityEvent<string>();
            actionLinesEvent.AddListener(actionLine => {
                actionLines.Add(actionLine);
            });

            FieldInfo onNewSpokenLineFieldInfo = dialogueController.GetType().GetField("_onNewSpokenLine", BindingFlags.NonPublic | BindingFlags.Instance);
            onNewSpokenLineFieldInfo?.SetValue(dialogueController, spokenLinesEvent);

            FieldInfo onNewActionLineFieldInfo = dialogueController.GetType().GetField("_onNewActionLine", BindingFlags.NonPublic | BindingFlags.Instance);
            onNewActionLineFieldInfo?.SetValue(dialogueController, actionLinesEvent);

            FieldInfo onNewLineFieldInfo = dialogueController.GetType().GetField("_onNewLine", BindingFlags.NonPublic | BindingFlags.Instance);
            onNewLineFieldInfo?.SetValue(dialogueController, new UnityEvent());

            NarrativeScript testNarrativeScript = new NarrativeScript(new TextAsset(ActionFollowedByScriptSerializedInk));
            dialogueController.SetNewDialogue(testNarrativeScript);

            // Run both lines
            dialogueController.OnContinueStory(); // == &MODE:Dialogue;
            dialogueController.OnContinueStory(); // == &SPEAK:Ross;

            // Assert their existence due to the events being called
            Assert.Contains("&SPEAK:Ross\n", actionLines);
            Assert.Contains("After all the work I put into those levels...\n", spokenLines);
        }
    }
}
