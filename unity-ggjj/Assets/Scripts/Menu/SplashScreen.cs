using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private Sprite _alternateSprite;
    [SerializeField] private float _duration;
    [SerializeField] private SceneLoader _sceneLoader;
    
    private IEnumerator Start()
    {
        if (Random.Range(0f, 1f) < 0.05f)
        {
            GetComponent<Image>().sprite = _alternateSprite;
        }

        yield return new WaitForSeconds(_duration);
        _sceneLoader.LoadScene("MainMenu");
    }
}
