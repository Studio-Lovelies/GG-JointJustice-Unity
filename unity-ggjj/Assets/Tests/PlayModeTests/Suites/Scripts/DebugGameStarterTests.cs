using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts
{
    public class DebugGameStarterTests
    {
        private const string BLANK_SCENE = "Assets/Tests/PlayModeTests/TestScenes/BlankScene.unity";
        private const string ROSS_COOL_X = "Assets/Tests/PlayModeTests/TestScripts/RossCoolX.json";
        
        [UnityTest]
        public IEnumerator CorrectlySetsUpNarrativePlayer()
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
            var debugGameStarter = gameStateGameObject.AddComponent<DebugGameStarter>();
            debugGameStarter.narrativeScript = AssetDatabase.LoadAssetAtPath<TextAsset>(ROSS_COOL_X);

            // We except this to be 'null' and throw an exception at this point, as DebugGameStarter.Start() hasn't run
            AssignNewNarrativeScriptPlayerComponent();
            Assert.IsNull(narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript);
            LogAssert.Expect(LogType.Exception, new Regex($"{nameof(NullReferenceException)}: "));
            
            // Complete the Unity Update Loop; this implicitly calls DebugGameStarter.Start()
            yield return new WaitForSeconds(0.0f);

            // We except this to be properly set now, as DebugGameStarter.Start() has run
            AssignNewNarrativeScriptPlayerComponent();
            Assert.AreEqual(narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.ActiveNarrativeScript!.Script.name, System.IO.Path.GetFileNameWithoutExtension(ROSS_COOL_X));
        }
    }
}