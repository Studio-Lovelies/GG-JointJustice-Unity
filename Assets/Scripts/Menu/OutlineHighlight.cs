using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used by highlightable menu items to enable and disable outline components.
/// </summary>
public class OutlineHighlight : MonoBehaviour, IHighlight
{
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void SetHighlighted(bool isHighlighted)
    {
        // Prevent null reference exception if this method is called before Awake
        if (_outline == null)
        {
            _outline = GetComponent<Outline>();
        }
        
        _outline.enabled = isHighlighted;
    }
}
