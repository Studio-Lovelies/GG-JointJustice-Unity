using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class ScreenshotDiff
{
    private static readonly Dictionary<string, int> CurrentCount = new Dictionary<string, int>();

    static IEnumerator captureScreenshot(string path)
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        var imageBytes = screenImage.EncodeToPNG();

        File.WriteAllBytes(path, imageBytes);
    }
    
    public static IEnumerator TakeScreenshotOrCompare()
    {
        var currentTestName = TestContext.CurrentContext.Test.FullName;
        var currentCount = CurrentCount.ContainsKey(currentTestName) ? CurrentCount[currentTestName] + 1 : 1;
        CurrentCount[currentTestName] = currentCount;

        var expectedFilePath = Path.Join( Application.dataPath, $"Tests/PlayModeTests/Tools/Screenshots/{currentTestName}/{currentCount}.png");
        expectedFilePath = Path.GetFullPath(expectedFilePath);
        
        Directory.CreateDirectory(Path.GetDirectoryName(expectedFilePath)!);
        
        if (File.Exists(expectedFilePath))
        {
            var actualFilePath = Path.GetTempFileName();
            yield return captureScreenshot(actualFilePath);
            Assert.IsTrue(PNGIsIdentical(actualFilePath, expectedFilePath), "Expected screenshot {0} is different from actual {1}", expectedFilePath, actualFilePath);
            yield break;
        }
        
        if (System.Environment.GetCommandLineArgs().Contains("-batchmode"))
        {
            Assert.Fail($"Screenshot {expectedFilePath} doesn't exist; have you committed the file after it got created locally?");
        }
        
        yield return captureScreenshot(expectedFilePath);
        Debug.LogWarning($"Screenshot {expectedFilePath} created; make sure to commit it");
    }

    private static bool PNGIsIdentical(string actualFilePath, string expectedFilePath)
    {
        var file = File.ReadAllBytes(actualFilePath);
        var temp = File.ReadAllBytes(expectedFilePath);
        if (file.Length != temp.Length)
        {
            Debug.LogWarning($"File {actualFilePath} ({file.Length}) and {expectedFilePath}({temp.Length}) are not the same length");
            ExportToArtifactDirectory(actualFilePath, expectedFilePath);
            return false;
        }
        for (var i = 0; i < file.Length; i++)
        {
            if (file[i] == temp[i])
            {
                continue;
            }

            Debug.LogWarning($"File {actualFilePath} and {expectedFilePath} are not the same at index {i}");
            ExportToArtifactDirectory(actualFilePath, expectedFilePath);
            return false;
        }

        return true;
    }

    private static void ExportToArtifactDirectory(string actualFilePath, string expectedFilePath)
    {
        var fileNameOfFilePath = $"{Path.GetFileName(Path.GetDirectoryName(actualFilePath))}.{Path.GetFileNameWithoutExtension(actualFilePath)}";
        var runDirectory = TestContext.CurrentTestExecutionContext.StartTime.ToString("s");
        var runDirectoryWithOnlyValidCharacters = string.Join("-", runDirectory.Split(Path.GetInvalidFileNameChars()));
        var targetDirectory = Path.GetFullPath(Path.Join(Application.dataPath, $"../TestRunArtifacts/{runDirectoryWithOnlyValidCharacters}/"));
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }
        
        File.Copy(actualFilePath, Path.GetFullPath(Path.Join(targetDirectory, $"{fileNameOfFilePath}.actual.png")));
        File.Copy(expectedFilePath, Path.GetFullPath(Path.Join(targetDirectory, $"{fileNameOfFilePath}.expected.png")));
    }
}
