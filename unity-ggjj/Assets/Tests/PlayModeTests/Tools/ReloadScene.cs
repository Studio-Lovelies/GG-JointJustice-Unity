using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine.SceneManagement;

namespace Tests.PlayModeTests.Tools
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ReloadScene : Attribute, ITestAction
    {
        private readonly string _sceneName;

        public ReloadScene(string sceneName)
        {
            _sceneName = sceneName;
        }

        public void BeforeTest(ITest test)
        {
            SceneManager.LoadScene(_sceneName);
        }

        public void AfterTest(ITest test)
        {
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
