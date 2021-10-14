using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used by highlightable menu items to enable and disable outline components.
/// </summary>
[RequireComponent(typeof(Outline))]
public class OutlineHighlight : MonoBehaviour, IHighlight
{
    private Outline _outline;

    /// <summary>
    /// Get Outline component and disable the highlight on awake.
    /// </summary>
    private void Awake()
    {
        _outline = GetComponent<Outline>();
        SetHighlighted(false);
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
