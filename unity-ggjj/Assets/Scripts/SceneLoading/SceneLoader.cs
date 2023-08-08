using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoading
{
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        private ITransition _transition;
        private AsyncOperation _sceneLoadOperation;
        private bool _isLoadingScene;

        private void Awake()
        {
            _transition = GetComponent<ITransition>();
        }
    
        public void LoadScene(string sceneName)
        {
            if (_isLoadingScene) { return; }
            _sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
            _isLoadingScene = true;

            if (_transition == null) { return; }
            _sceneLoadOperation.allowSceneActivation = false;
            _transition.Transition(() => _sceneLoadOperation.allowSceneActivation = true);
        }
    }
}
