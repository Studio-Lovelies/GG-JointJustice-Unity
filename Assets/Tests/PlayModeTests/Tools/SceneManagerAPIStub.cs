using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.PlayModeTests.Tools
{
    public class SceneManagerAPIStub : SceneManagerAPI
    {
        public List<string> loadedScenes = new List<string>();
        protected override AsyncOperation LoadSceneAsyncByNameOrIndex(string sceneName, int sceneBuildIndex, LoadSceneParameters parameters, bool mustCompleteNextFrame)
        {
            loadedScenes.Add(sceneName);
            return null;
        }
    }
}