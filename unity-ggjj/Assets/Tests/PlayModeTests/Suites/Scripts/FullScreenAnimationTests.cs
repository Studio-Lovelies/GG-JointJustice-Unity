using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Suites.Scripts
{
    public class FullScreenAnimation
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        [SetUp]
        public void Setup()
        {
            _inputTestTools.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            _inputTestTools.TearDown();
        }


        [UnityTest]
        public IEnumerator FullScreenAnimationsCannotBeSkipped()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            TestTools.StartGame("TripleGavelHitTest");
            
            var speechPanel = TestTools.FindInactiveInSceneByName<GameObject>("SpeechPanel");
            yield return null; 
           
            Assert.IsFalse(speechPanel.activeInHierarchy);
            yield return _inputTestTools.PressForFrame(_inputTestTools.keyboard.xKey);
            Assert.IsFalse(speechPanel.activeInHierarchy);
        }
    }
}

