using System.Collections;
using NUnit.Framework;
using SceneLoading;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Suites.Scenes
{
    public class SplashScreenTests
    {
        private InputTestTools input = new();
        
        [SetUp]
        public void Setup()
        {
            input.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            input.TearDown();
        }

        [UnityTest]
        public IEnumerator SplashScreenFadesLogoInAndOut()
        {
            yield return SceneManager.LoadSceneAsync("Splash");

            var fadeToImageTransition = Object.FindAnyObjectByType<FadeToImageTransition>();
            var fadeTime = fadeToImageTransition.FadeTime;
            var blackScreen = fadeToImageTransition.GetComponent<Image>();
            
            Assert.AreEqual(1f, blackScreen.color.a);
            yield return new WaitForSeconds(fadeTime/8);
            Assert.AreNotEqual(1f, blackScreen.color.a);
            Assert.AreNotEqual(0f, blackScreen.color.a);
            yield return new WaitForSeconds(fadeTime/8);
            yield return new WaitForSeconds(fadeTime/4);
            Assert.AreEqual(0f, blackScreen.color.a);
            yield return new WaitForSeconds(fadeTime/4);
            yield return new WaitForSeconds(fadeTime/8);
            Assert.AreNotEqual(1f, blackScreen.color.a);
            Assert.AreNotEqual(0f, blackScreen.color.a);
            yield return new WaitForSeconds(fadeTime/8);
            Assert.AreEqual(1f, blackScreen.color.a);
        }

        [UnityTest]
        public IEnumerator SplashScreenLoadsMainMenuWithBothImages()
        {
            yield return SceneManager.LoadSceneAsync("Splash");
            SplashScreen splashScreen = Object.FindAnyObjectByType<SplashScreen>();
            var imageName = splashScreen.GetComponent<Image>().sprite.texture.name;
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "MainMenu");
            
            yield return SceneManager.LoadSceneAsync("Splash");
            splashScreen = Object.FindAnyObjectByType<SplashScreen>();
            
            // override randomness and force the splash image to be different on the second run based on the first image
            if (imageName.Contains("1"))
            {
                splashScreen.GetRandomValue = () => 0f;
            } else if (imageName.Contains("2"))
            {
                splashScreen.GetRandomValue = () => 1f;
            }

            yield return null;
            Assert.AreNotEqual(imageName, splashScreen.GetComponent<Image>().sprite.texture.name);
            yield return TestTools.WaitForState(() => SceneManager.GetActiveScene().name == "MainMenu");
        }
    }
}
