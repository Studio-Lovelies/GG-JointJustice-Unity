using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine.SceneManagement;

namespace Tests.PlayModeTests.Scenes.MainMenu
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ReloadScene : Attribute, ITestAction
    {
        private const string MainMenuScene = "Assets/Scenes/MainMenu.unity";
        public void BeforeTest(ITest test)
        {
            SceneManager.LoadScene(MainMenuScene, LoadSceneMode.Additive);
        }

        public void AfterTest(ITest test)
        {
            SceneManager.UnloadScene(MainMenuScene);
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
