using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used by highlightable menu items to enable and disable image components used to give a highlight effect.
/// Image components should be a child of the highlighted item.
/// </summary>
public class ImageHighlight : MonoBehaviour, IHighlight
{
    private Image _image;
    
    public void Awake()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
    }
    
    public void SetHighlighted(bool isHighlighted)
    {
        _image.enabled = isHighlighted;
    }
}
