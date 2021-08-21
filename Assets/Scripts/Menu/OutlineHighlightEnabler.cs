using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used by HighlightableMenuItems to enable and disable outline components.
/// </summary>
public class OutlineHighlightEnabler : MonoBehaviour, IHighlightEnabler
{
    private Outline _outline;

    public bool HighlightEnabled => _outline.enabled;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void SetHighlighted(bool highlighted)
    {
        // Prevent null reference exception if this method is called before Awake
        if (_outline == null)
        {
            _outline = GetComponent<Outline>();
        }
        
        _outline.enabled = highlighted;
    }
}
