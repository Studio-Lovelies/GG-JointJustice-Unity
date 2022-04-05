using System.Collections;
using Tests.PlayModeTests.Tools;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scenes
{
    public class CreditsTests
    {
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
