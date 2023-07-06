using UnityEditor;
using UnityEngine;

public class Hotfix : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {
        System.IO.File.Delete(Application.dataPath + "/../Library/PackageCache/com.inklestudios.ink-unity-integration@1.1.1/InkLibs/InkCompiler/ink_compiler.csproj.meta");
    }
}