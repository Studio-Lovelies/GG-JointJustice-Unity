using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;

namespace Editor.Ink
{
    public class Validation : AssetPostprocessor
    {
        /// <summary>
        /// Called when an Ink file is compiled after editing it
        /// </summary>
        private static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            foreach (var importedAssetPath in importedAssets)
            {
                if (!InkEditorUtils.IsInkFile(importedAssetPath))
                {
                    continue;
                }
                var inkFile = InkLibrary.GetInkFileWithPath(importedAssetPath);
                ValidateNarrativeScript(inkFile);
            }
        }

        /// <summary>
        /// Menu item to allow validation of all Ink files in the project
        /// </summary>
        [UnityEditor.MenuItem("Assets/Validate Narrative Scripts", false, 202)]
        private static void ValidateAllNarrativeScripts()
        {
            var noErrors = true;
            foreach (var unused in InkLibrary.GetMasterInkFiles().ToList().Where(inkFile => !ValidateNarrativeScript(inkFile)))
            {
                noErrors = false;
            }

            if (noErrors)
            {
                Debug.Log("Narrative Scripts validated without errors");
            }
        }

        /// <summary>
        /// Reads an Ink file and attempts to parse every action.
        /// Logs any errors found to the console
        /// </summary>
        /// <param name="inkFile">The Ink file to read</param>
        private static bool ValidateNarrativeScript(InkFile inkFile)
        {
            var lines = new List<string>();
            var story = new Story(inkFile.jsonAsset.text);
            NarrativeScript.ReadContent(story.mainContentContainer.content, lines, story);

            var noErrors = true;

            foreach (var line in lines.Where(t => t[0] == ActionDecoderBase.ACTION_TOKEN))
            {
                try
                {
                    ActionDecoderBase.GenerateInvocationDetails(line, typeof(ActionDecoder));
                }
                catch (Exception exception)
                {
                    noErrors = false;
                    Debug.LogError($"Error in {inkFile} on line \"{line}\"\n{exception.Message}");
                }
            }

            return noErrors;
        }
    }
}
