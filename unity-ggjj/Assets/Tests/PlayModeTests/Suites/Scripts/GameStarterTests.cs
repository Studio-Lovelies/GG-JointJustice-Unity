using System;
using System.Collections;
using System.Text.RegularExpressions;
using GameState;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Suites.Scripts
{
    public class GameStarterTests
    {
        private const string BLANK_SCENE = "Assets/Tests/PlayModeTests/TestScenes/BlankScene.unity";
        private const string ROSS_COOL_X = "Assets/Tests/PlayModeTests/TestScripts/RossCoolX.json";
        
        [UnityTest]
        public IEnumerator CorrectlySetsUpNarrativeScriptPlayer()
        { 
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(BLANK_SCENE, new LoadSceneParameters());
            var gameStateGameObject = new GameObject();
            var narrativeGameState = gameStateGameObject.AddComponent<NarrativeGameState>();
        
            // The NarrativeScriptPlayerComponent fetches the currently active script only the first time "ActiveNarrativeScript" is accessed.
            // Hence, we need a new instance every time we want to check if the currently "ActiveNarrativeScript" has changed
            void AssignNewNarrativeScriptPlayerComponent()
            {
                var narrativeScriptPlayerComponent = gameStateGameObject.AddComponent<NarrativeScriptPlayerComponent>();
                var serializedObjectNarrativeScriptPlayerComponent = new SerializedObject(narrativeScriptPlayerComponent);
                serializedObjectNarrativeScriptPlayerComponent.FindProperty("_narrativeGameState").objectReferenceValue = narrativeGameState;
                serializedObjectNarrativeScriptPlayerComponent.ApplyModifiedProperties();
        
                var narrativeGameStateSerializedObject = new SerializedObject(narrativeGameState);
                narrativeGameStateSerializedObject.FindProperty("_narrativeScriptPlayerComponent").objectReferenceValue = narrativeScriptPlayerComponent;
                narrativeGameStateSerializedObject.ApplyModifiedProperties();
            }
            
            // Create the `DebugGameStarter` (`Awake()` has been called, however `Start()` hasn't yet)
            var debugGameStarter = gameStateGameObject.AddComponent<GameStarter>();
            var testDebugScript = AssetDatabase.LoadAssetAtPath<TextAsset>(ROSS_COOL_X);
            Assert.NotNull(testDebugScript, "Failed to load specified test debug script.");
            TestTools.SetField(debugGameStarter, "_debugNarrativeScriptTextAsset", testDebugScript);
        
            // We expect this to be 'null' and throw an exception at this point, as DebugGameStarter.Start() hasn't run
            AssignNewNarrativeScriptPlayerComponent();
            Assert.IsNull(narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript);
            LogAssert.Expect(LogType.Exception, new Regex($"{nameof(NullReferenceException)}: "));
            
            // Complete the Unity Update Loop; this implicitly calls DebugGameStarter.Start()
            yield return new WaitForSeconds(0.0f);
        
            // We expect this to be properly set now, as GameStarter.Start() has run
            AssignNewNarrativeScriptPlayerComponent();
            Assert.AreEqual(narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript!.Script.name, System.IO.Path.GetFileNameWithoutExtension(ROSS_COOL_X));
        }
    }
}