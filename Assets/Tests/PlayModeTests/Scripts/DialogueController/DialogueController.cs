using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.DialogueController
{
    public class DialogueController
    {
        // This constant contains exactly one action and one spoken line
        private const string ActionFollowedByScriptSerializedInk = "{\"inkVersion\":20,\"root\":[[\"^&SPEAK:Ross\",\"\\n\",\"^After all the work I put into those levels...\",\"\\n\",\"end\",[\"done\",{\"#f\":5,\"#n\":\"g-0\"}],null],\"done\",{\"#f\":1}],\"listDefs\":{}}";

        [Test]
        public void DialogueController_OnNextLine_CallsCorrectEvents()
        {
            // Setup component
            var dialogueControllerGameObject = new GameObject("DialogueController");
            var dialogueController = dialogueControllerGameObject.AddComponent<global::DialogueController>();

            // Setup event listener for action and spoken lines
            List<string> spokenLines = new List<string>();
            var spokenLinesEvent = new NewSpokenLineEvent();
            spokenLinesEvent.AddListener(spokenLine => {
                spokenLines.Add(spokenLine);
            });

            List<string> actionLines = new List<string>();
            var actionLinesEvent = new NewActionLineEvent();
            actionLinesEvent.AddListener(actionLine => {
                actionLines.Add(actionLine);
            });

            FieldInfo onNewSpokenLineFieldInfo = dialogueController.GetType().GetField("_onNewSpokenLine", BindingFlags.NonPublic | BindingFlags.Instance);
            onNewSpokenLineFieldInfo.SetValue(dialogueController, spokenLinesEvent);

            FieldInfo onNewActionLineFieldInfo = dialogueController.GetType().GetField("_onNewActionLine", BindingFlags.NonPublic | BindingFlags.Instance);
            onNewActionLineFieldInfo.SetValue(dialogueController, actionLinesEvent);

            FieldInfo onNewLineFieldInfo = dialogueController.GetType().GetField("_onNewLine", BindingFlags.NonPublic | BindingFlags.Instance);
            onNewLineFieldInfo.SetValue(dialogueController, new UnityEvent());

            dialogueController.SetNarrativeScript(new TextAsset(ActionFollowedByScriptSerializedInk));

            // Run both lines
            dialogueController.OnContinueStory();
            dialogueController.OnContinueStory();
            
            // Assert their existence due to the events being called
            Assert.Contains("&SPEAK:Ross\n", actionLines);
            Assert.Contains("After all the work I put into those levels...\n", spokenLines);
        }
    }
}
