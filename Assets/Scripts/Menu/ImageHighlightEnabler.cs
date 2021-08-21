using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used by HighlightableMenuItems to enable and disable image components.
/// </summary>
public class ImageHighlightEnabler : MonoBehaviour, IHighlightEnabler
{
    [SerializeField, Tooltip("The Image component that will display the highlight sprite")]
    private Image _image;

    public bool HighlightEnabled => _image.enabled;

    public void SetHighlighted(bool highlighted)
    {
        _image.enabled = highlighted;
    }
}
