using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SceneLoading;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.EditModeTests.Suites
{
    public class GameStartSettingsTests
    {
        [Test]
        public void NarrativeScriptTextAssetCanOnlyBeAccessedOnce()
        {   
            const string GAME_START_SETTINGS_PATH = "Assets/Scripts/SceneLoading/GameStartSettings.asset";
            const string TEST_NARRATIVE_SCRIPT_TEXT_ASSET_PATH = "Assets/Tests/PlayModeTests/TestScripts/RossCoolX.json";
            
            var gameStartSettings = AssetDatabase.LoadAssetAtPath<GameStartSettings>(GAME_START_SETTINGS_PATH);
            var testNarrativeScriptTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(TEST_NARRATIVE_SCRIPT_TEXT_ASSET_PATH);
            
            Assert.NotNull(gameStartSettings);
            Assert.NotNull(testNarrativeScriptTextAsset);
            
            Assert.IsFalse(gameStartSettings.IsNarrativeScriptTextAssetAssigned);
            gameStartSettings.SetNarrativeScriptTextAsset(testNarrativeScriptTextAsset);
            Assert.IsTrue(gameStartSettings.IsNarrativeScriptTextAssetAssigned);
            
            var narrativeScriptTextAsset = gameStartSettings.GetAndClearNarrativeScriptTextAsset();
            Assert.AreEqual(testNarrativeScriptTextAsset, narrativeScriptTextAsset);
            Assert.IsFalse(gameStartSettings.IsNarrativeScriptTextAssetAssigned);

            Assert.Throws<InvalidOperationException>(() => gameStartSettings.GetAndClearNarrativeScriptTextAsset());
        }
    }
}