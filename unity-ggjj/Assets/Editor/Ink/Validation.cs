using System.Collections.Generic;
using Ink.UnityIntegration;
using UnityEditor;
using UnityEngine;

public class Validation : AssetPostprocessor
{
    private static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        Debug.Log("a");
        foreach (var importedAssetPath in importedAssets) {
            if (InkEditorUtils.IsInkFile(importedAssetPath))
            {
                Debug.Log(importedAssetPath);
                ValidateNarrativeScript(importedAssetPath);
            }
        }
    }

    private static void ValidateNarrativeScript(string importedAssetPath)
    {
        
    }
}
