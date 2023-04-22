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
        var actualImage = new Texture2D(2, 2);
        actualImage.LoadImage(File.ReadAllBytes(actualFilePath));
        var expectedImage = new Texture2D(2, 2);
        expectedImage.LoadImage(File.ReadAllBytes(expectedFilePath));
        
        if (actualImage.width != expectedImage.width || actualImage.height != expectedImage.height)
        {
            ExportToArtifactDirectory(actualFilePath, expectedFilePath);
            Assert.Fail($"Expected image {expectedFilePath}(w={expectedImage.width}, h={expectedImage.height}) is different from actual {actualFilePath}(w={actualImage.width}, h={actualImage.height})");
            return false;
        }
        
        for (var x = 0; x < actualImage.width; x++)
        {
            for (var y = 0; y < actualImage.height; y++)
            {
                var actualPixel = actualImage.GetPixel(x, y);
                var expectedPixel = expectedImage.GetPixel(x, y);
                if (RoughlyEquals(actualPixel, expectedPixel))
                {
                    continue;
                }
                ExportToArtifactDirectory(actualFilePath, expectedFilePath);
                Assert.Fail($"Expected image pixel {expectedFilePath}(x={x}, y={y}, v={expectedPixel}) is different from actual {actualFilePath}(x={x}, y={y}, v={actualPixel})");
                return false;
            }
        }

        return true;
    }

    private static bool RoughlyEquals(Color actualPixel, Color expectedPixel)
    {
        const float THRESHOLD = 10f / 255f;
        return (
                Mathf.Abs(actualPixel.r - expectedPixel.r) + 
                Mathf.Abs(actualPixel.g - expectedPixel.g) + 
                Mathf.Abs(actualPixel.b - expectedPixel.b) + 
                Mathf.Abs(actualPixel.a - expectedPixel.a)
            ) / 4f < THRESHOLD;
    }

    private static void ExportToArtifactDirectory(string actualFilePath, string expectedFilePath)
    {
        var fileNameOfFilePath = Path.GetFileNameWithoutExtension(expectedFilePath);
        var runDirectory = TestContext.CurrentTestExecutionContext.StartTime.ToString("s").Replace(":", "-");
        var targetDirectory = Path.GetFullPath(Path.Join(Application.dataPath, $"../TestRunArtifacts/{runDirectory}/{Path.GetFileName(Path.GetDirectoryName(expectedFilePath))}"));
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }
        
        File.Copy(actualFilePath, Path.GetFullPath(Path.Join(targetDirectory, $"{fileNameOfFilePath}.actual.png")));
        File.Copy(expectedFilePath, Path.GetFullPath(Path.Join(targetDirectory, $"{fileNameOfFilePath}.expected.png")));
    }
}
