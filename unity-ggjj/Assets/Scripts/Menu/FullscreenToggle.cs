using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    
    void Awake()
    {
        SetFullscreen(this.toggle.isOn);
    }

    void Update()
    {
        
    }
    
    /// <summary>
    /// Set the screen to fullscreen mode if isFullscreen is true
    /// </summary>
    public void SetFullscreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        Debug.Log(isFullScreen ? "Fullscreen Enabled" : "Fullscreen Disabled");
    }
}
