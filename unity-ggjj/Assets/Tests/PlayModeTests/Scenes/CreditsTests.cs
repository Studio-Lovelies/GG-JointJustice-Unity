using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes
{
    public class CreditsTests
    {
        [UnityTest]
        public IEnumerator CreditsCanBeLoadedViaAction()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            var gameState = Object.FindObjectOfType<NarrativeGameState>();
            var actionDecoder = Object.FindObjectOfType<ActionDecoderComponent>();
            actionDecoder.Decoder.NarrativeGameState = gameState;
            Assert.AreNotEqual(SceneManager.GetActiveScene().name, "Credits");
            actionDecoder.OnNewActionLine("&LOAD_SCENE:Credits\n");
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "Credits");

        [UnityTest]
        public IEnumerator CreditsCanBeSkipped()
        {
            yield return SceneManager.LoadSceneAsync("Credits");
            var inputTestTools = new InputTestTools();
            inputTestTools.Setup();
            yield return inputTestTools.PressForFrame(inputTestTools.Keyboard.xKey);
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "MainMenu");
        }
    }
}
