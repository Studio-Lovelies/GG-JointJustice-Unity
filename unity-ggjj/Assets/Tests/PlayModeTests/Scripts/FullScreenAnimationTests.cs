using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts
{
    public class FullScreenAnimation
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        
        [UnityTest]
        public IEnumerator FullScreenAnimationsCannotBeSkipped()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/FullScreenAnimation - Test Scene.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            var speechPanel = TestTools.FindInactiveInSceneByName<GameObject>("SpeechPanel");
            var dialogueController = Object.FindObjectOfType<global::DialogueController>();
            yield return null; 
           
            Assert.IsFalse(speechPanel.activeInHierarchy);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            Assert.IsFalse(speechPanel.activeInHierarchy);
            yield return TestTools.WaitForState(() => !dialogueController.IsBusy);
            Assert.IsTrue(speechPanel.activeInHierarchy);
            yield return SceneManager.UnloadSceneAsync("Assets/Scenes/TestScenes/FullScreenAnimation - Test Scene.unity");
        }
    }
}

