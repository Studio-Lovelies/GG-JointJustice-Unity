using System.Collections;
using Tests.PlayModeTests.Tools;
using UnityEngine;
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
            actionDecoder.OnNewActionLine("&LOAD_SCENE:Credits\n");
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "Credits");
        }
    }
}
