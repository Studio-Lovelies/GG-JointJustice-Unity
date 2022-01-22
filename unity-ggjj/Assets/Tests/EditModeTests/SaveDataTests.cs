using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SaveFiles;
using UnityEngine;
using static System.Int32;

/// <remarks>
/// A new save file is created for each individual test. See: <see cref="EnsureDefaultSaveFileExists"/>
/// </remarks>
public class SaveDataTests
{
    private readonly Proxy _proxy = new Proxy();

    [SetUp]
    [OneTimeTearDown]
    public void EnsureDefaultSaveFileExists()
    {
        // Force creation of a new default SaveData object
        Proxy.DeleteSaveData();
        _proxy.UpdateCurrentSaveData((ref SaveData saveData) => saveData = new SaveData(SaveData.LatestVersion));
    }


    [Test]
    public void LoadReturnsUpdatedSaveData()
    {
        var initialSaveData = _proxy.Load();
        // unlock a chapter
        _proxy.UpdateCurrentSaveData((ref SaveData data) => {
            data.GameProgression.UnlockedChapters.AddChapter(SaveData.Progression.Chapters.Chapter2);
        });
        var firstSaveDataUpdate = _proxy.Load();

        Assert.IsFalse(initialSaveData.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.Chapter2));
        Assert.IsTrue(firstSaveDataUpdate.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.Chapter2));

        // unlock another chapter
        _proxy.UpdateCurrentSaveData((ref SaveData data) => {
            data.GameProgression.UnlockedChapters.AddChapter(SaveData.Progression.Chapters.BonusChapter1);
        });

        var secondSaveDataUpdate = _proxy.Load();
        Assert.IsFalse(initialSaveData.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.Chapter2));
        Assert.IsFalse(initialSaveData.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.BonusChapter1));
        Assert.IsTrue(firstSaveDataUpdate.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.Chapter2));
        Assert.IsFalse(firstSaveDataUpdate.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.BonusChapter1));
        Assert.IsTrue(secondSaveDataUpdate.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.Chapter2));
        Assert.IsTrue(secondSaveDataUpdate.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.BonusChapter1));
    }

    [Test]
    public void LoadingOutdatedSaveDataGetsUpgraded()
    {
        // deliberately overwrite the current SaveData version to 0
        var currentInternalSaveData = PlayerPrefs.GetString("SaveData");
        var saveDataAtVersionZero = new Regex("\"Version\":\\d+").Replace(currentInternalSaveData, "version:0");
        StringAssert.AreNotEqualIgnoringCase(currentInternalSaveData, saveDataAtVersionZero);
        PlayerPrefs.SetString("SaveData", saveDataAtVersionZero);
        PlayerPrefs.Save();
        StringAssert.AreEqualIgnoringCase(saveDataAtVersionZero, PlayerPrefs.GetString("SaveData", saveDataAtVersionZero));

        // loading is successful and SaveData is set to the current version
        var saveData = _proxy.Load();
        Assert.AreEqual(saveData.Version, SaveData.LatestVersion);
    }

    [Test]
    public void ThrowsIfSaveGameIsTooNew()
    {
        const int absurdlyHighVersionNumber = MaxValue - 1;
        // deliberately overwrite the current SaveData version to be close to Int32 maximum
        var currentInternalSaveData = PlayerPrefs.GetString("SaveData");
        var saveDataAtMaxBounds = new Regex("\"Version\":\\d+").Replace(currentInternalSaveData, $"version:{absurdlyHighVersionNumber}");
        StringAssert.AreNotEqualIgnoringCase(currentInternalSaveData, saveDataAtMaxBounds);
        PlayerPrefs.SetString("SaveData", saveDataAtMaxBounds);
        PlayerPrefs.Save();
        StringAssert.AreEqualIgnoringCase(saveDataAtMaxBounds, PlayerPrefs.GetString("SaveData", saveDataAtMaxBounds));

        var exception = Assert.Throws<NotSupportedException>(() => {
            var _ = _proxy.Load();
        });
        StringAssert.Contains($"'{absurdlyHighVersionNumber}'", exception.Message);
        StringAssert.Contains($"'{SaveData.LatestVersion}'", exception.Message);
    }

    [Test]
    public void ThrowsIfAttemptingToLoadWithoutSaveDataPresent()
    {
        Proxy.DeleteSaveData();

        Assert.Throws<KeyNotFoundException>(() => {
            var _ = _proxy.Load();
        });
    }

    [Test]
    public void EnsureSaveFileGetsCreatedOnUpdate()
    {
        Proxy.DeleteSaveData();

        Assert.IsFalse(Proxy.HasExistingSaveData());
        
        _proxy.UpdateCurrentSaveData((ref SaveData saveData) => saveData = new SaveData(SaveData.LatestVersion));
        
        Assert.IsTrue(Proxy.HasExistingSaveData());
    }
}
