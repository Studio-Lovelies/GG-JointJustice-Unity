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
    private const string PLAYER_PREFS_KEY = "SaveData";
    private SaveData _previouslyStoredSaveData;

    [SetUp]
    public void EnsureDefaultSaveFileExists()
    {
        // Force creation of a new default SaveData object
        PlayerPrefsProxy.DeleteSaveData();
        PlayerPrefsProxy.UpdateCurrentSaveData((ref SaveData saveData) => saveData = new SaveData(SaveData.LatestVersion));
    }

    [OneTimeTearDown]
    public void RestorePreviouslyExistingSaveFile()
    {
        PlayerPrefsProxy.DeleteSaveData();
        if (_previouslyStoredSaveData != null)
        {
            PlayerPrefsProxy.UpdateCurrentSaveData((ref SaveData data) => data = _previouslyStoredSaveData);
        }
    }

    [OneTimeSetUp]
    public void BackupPreviouslyExistingSaveFile()
    {
        if (!PlayerPrefsProxy.HasExistingSaveData())
        {
            return;
        }
        _previouslyStoredSaveData = PlayerPrefsProxy.Load();
    }

    [Test]
    public void LoadReturnsUpdatedSaveData()
    {
        var initialSaveData = PlayerPrefsProxy.Load();
        // unlock a chapter
        PlayerPrefsProxy.UpdateCurrentSaveData((ref SaveData data) => {
            data.GameProgression.UnlockedChapters.AddChapter(SaveData.Progression.Chapters.Chapter2);
        });
        var firstSaveDataUpdate = PlayerPrefsProxy.Load();

        Assert.IsFalse(initialSaveData.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.Chapter2));
        Assert.IsTrue(firstSaveDataUpdate.GameProgression.UnlockedChapters.HasFlag(SaveData.Progression.Chapters.Chapter2));

        // unlock another chapter
        PlayerPrefsProxy.UpdateCurrentSaveData((ref SaveData data) => {
            data.GameProgression.UnlockedChapters.AddChapter(SaveData.Progression.Chapters.BonusChapter1);
        });

        var secondSaveDataUpdate = PlayerPrefsProxy.Load();
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
        var currentInternalSaveData = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
        var saveDataAtVersionZero = new Regex("\"Version\":\\d+").Replace(currentInternalSaveData, "version:0");
        StringAssert.AreNotEqualIgnoringCase(currentInternalSaveData, saveDataAtVersionZero);
        PlayerPrefs.SetString(PLAYER_PREFS_KEY, saveDataAtVersionZero);
        PlayerPrefs.Save();
        StringAssert.AreEqualIgnoringCase(saveDataAtVersionZero, PlayerPrefs.GetString(PLAYER_PREFS_KEY, saveDataAtVersionZero));

        // loading is successful and SaveData is set to the current version
        var saveData = PlayerPrefsProxy.Load();
        Assert.AreEqual(saveData.Version, SaveData.LatestVersion);
    }

    [Test]
    public void ThrowsIfSaveGameIsTooNew()
    {
        const int absurdlyHighVersionNumber = MaxValue - 1;
        // deliberately overwrite the current SaveData version to be close to Int32 maximum
        var currentInternalSaveData = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
        var saveDataAtMaxBounds = new Regex("\"Version\":\\d+").Replace(currentInternalSaveData, $"version:{absurdlyHighVersionNumber}");
        StringAssert.AreNotEqualIgnoringCase(currentInternalSaveData, saveDataAtMaxBounds);
        PlayerPrefs.SetString(PLAYER_PREFS_KEY, saveDataAtMaxBounds);
        PlayerPrefs.Save();
        StringAssert.AreEqualIgnoringCase(saveDataAtMaxBounds, PlayerPrefs.GetString(PLAYER_PREFS_KEY, saveDataAtMaxBounds));

        var exception = Assert.Throws<NotSupportedException>(() => {
            var _ = PlayerPrefsProxy.Load();
        });
        StringAssert.Contains($"'{absurdlyHighVersionNumber}'", exception.Message);
        StringAssert.Contains($"'{SaveData.LatestVersion}'", exception.Message);
    }

    [Test]
    public void ThrowsIfAttemptingToLoadWithoutSaveDataPresent()
    {
        PlayerPrefsProxy.DeleteSaveData();

        Assert.Throws<KeyNotFoundException>(() => {
            var _ = PlayerPrefsProxy.Load();
        });
    }

    [Test]
    public void EnsureSaveFileGetsCreatedOnUpdate()
    {
        PlayerPrefsProxy.DeleteSaveData();

        Assert.IsFalse(PlayerPrefsProxy.HasExistingSaveData());
        
        PlayerPrefsProxy.UpdateCurrentSaveData((ref SaveData saveData) => saveData = new SaveData(SaveData.LatestVersion));
        
        Assert.IsTrue(PlayerPrefsProxy.HasExistingSaveData());
    }
}
