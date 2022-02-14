using System;
using System.Linq;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;

public class Validation : AssetPostprocessor
{
    private static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        foreach (var importedAssetPath in importedAssets) {
            if (InkEditorUtils.IsInkFile(importedAssetPath))
            {
                ValidateNarrativeScript(importedAssetPath);
            }
        }
    }

    private static void ValidateNarrativeScript(string importedAssetPath)
    {
        InkFile inkFile = InkLibrary.GetInkFileWithPath(importedAssetPath);
        var lines = inkFile.GetFileContents().Split('\n');
        var decoder = new ActionDecoder();
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != string.Empty && lines[i][0] == DialogueController.ACTION_TOKEN)
            {
                try
                {
                    decoder.GetMethod(lines[i]);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Error on line {i + 1}: {exception.Message}");
                }
            }
        }
    }
}
