using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts
{
    public class FullScreenAnimation : InputTest
    {
        [UnityTest]
        public IEnumerator FullScreenAnimationsCannotBeSkipped()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/FullScreenAnimation - Test Scene.unity", new LoadSceneParameters());
            var speechPanel = TestTools.FindInactiveInSceneByName<GameObject>("SpeechPanel");
            yield return null; 
           
            Assert.IsFalse(speechPanel.activeInHierarchy);
            yield return PressForFrame(Keyboard.xKey);
            Assert.IsFalse(speechPanel.activeInHierarchy);
        }
    }
}

