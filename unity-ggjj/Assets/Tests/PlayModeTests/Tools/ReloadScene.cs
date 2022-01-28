using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Tests.PlayModeTests.Tools
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ReloadScene : Attribute, ITestAction
    {
        private readonly string _absoluteScenePath;

        public ReloadScene(string absoluteScenePath)
        {
            _absoluteScenePath = absoluteScenePath;
        }

        public void BeforeTest(ITest test)
        {
            EditorSceneManager.LoadSceneInPlayMode(_absoluteScenePath, new LoadSceneParameters(LoadSceneMode.Additive));
        }

        public void AfterTest(ITest test)
        {
            SceneManager.UnloadSceneAsync(_absoluteScenePath);
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
