using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used by highlightable menu items to enable and disable image components used to give a highlight effect.
/// Image components should be a child of the highlighted item.
/// </summary>
public class ImageHighlight : MonoBehaviour, IHighlight
{
    [SerializeField, Tooltip("The Image component that will display the highlight sprite")]
    private Image _image;

    /// <summary>
    /// Disable the highlight on awake
    /// </summary>
    public void Awake()
    {
        _image.enabled = false;
    }
    
    public void SetHighlighted(bool isHighlighted)
    {
        _image.enabled = isHighlighted;
    }
}
