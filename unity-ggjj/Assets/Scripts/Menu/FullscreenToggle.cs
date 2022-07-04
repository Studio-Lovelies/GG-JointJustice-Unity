using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Toggle>().isOn = Screen.fullScreen;
    }

    /// <summary>
    /// Set the screen to fullscreen mode if isFullscreen is true
    /// </summary>
    public void SetFullscreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
