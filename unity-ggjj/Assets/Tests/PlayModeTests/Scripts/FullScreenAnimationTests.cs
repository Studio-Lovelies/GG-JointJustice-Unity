using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
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
            yield return SceneManager.LoadSceneAsync("Game");
            TestTools.StartGame("TripleGavelHitTest");
            
            var speechPanel = TestTools.FindInactiveInSceneByName<GameObject>("SpeechPanel");
            yield return null; 
           
            Assert.IsFalse(speechPanel.activeInHierarchy);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.xKey);
            Assert.IsFalse(speechPanel.activeInHierarchy);
        }
    }
}

