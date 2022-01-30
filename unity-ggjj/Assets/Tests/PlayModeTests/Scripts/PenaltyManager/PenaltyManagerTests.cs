using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scripts.PenaltyManager
{
    public class PenaltyTests
    {
        private global::PenaltyManager _penaltyManager;
        private readonly InputTestTools _inputTestTools = new InputTestTools();

        public Keyboard Keyboard => _inputTestTools.Keyboard;
    
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScenes/Penalties - Test Scene.unity", new LoadSceneParameters(LoadSceneMode.Additive));
            _penaltyManager = Object.FindObjectOfType<global::PenaltyManager>();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            yield return SceneManager.UnloadSceneAsync("Penalties - Test Scene");
        }
    
        [UnityTest]
        public IEnumerator PenaltiesAreEnabledOnCrossExaminationStart()
        {
            Assert.IsTrue(!_penaltyManager.isActiveAndEnabled || _penaltyManager.PenaltiesLeft == 0);
            yield return _inputTestTools.PressForFrame(Keyboard.xKey);
            Assert.IsTrue(_penaltyManager.isActiveAndEnabled);
        }
    
        [UnityTest]
        public IEnumerator PenaltiesAreDisabledOnCrossExaminationEnd()
        {
            yield return _inputTestTools.PressForFrame(Keyboard.xKey);
            Assert.IsTrue(_penaltyManager.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(Keyboard.xKey);
            Assert.IsFalse(_penaltyManager.isActiveAndEnabled);
        }
    
        [UnityTest]
        public IEnumerator NumberOfPenaltiesCanBeReset()
        {
            var dialogueController = Object.FindObjectOfType<global::DialogueController>();

            for (int i = 0; i < 3; i++)
            {
                yield return _inputTestTools.PressForFrame(Keyboard.xKey);
            }
            
            yield return _inputTestTools.PressForFrame(Keyboard.zKey);
            yield return _inputTestTools.PressForFrame(Keyboard.enterKey);

            var subStory = GameObject.Find("SubStory(Clone)");
            yield return TestTools.DoUntilStateIsReached(() => _inputTestTools.ProgressStory(dialogueController), () => subStory == null);
            
            Assert.AreEqual(4, _penaltyManager.PenaltiesLeft);
            yield return _inputTestTools.PressForFrame(Keyboard.xKey);
            Assert.AreEqual(5, _penaltyManager.PenaltiesLeft);
        }
    }
}
