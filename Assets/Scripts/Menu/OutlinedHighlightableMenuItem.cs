using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for outlined buttons. These buttons use UnityEngine.UI's
/// Outline component to indicate that they are highlighted
/// </summary>
[RequireComponent(typeof(Outline))]
public class OutlinedHighlightableMenuItem : HighlightableMenuItem
{
    private Outline _outline;
    private Button _button;
    
    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _button = GetComponent<Button>();
    }

    public override void SetHighlighted(bool highlighted)
    {
        _outline.enabled = highlighted;
    }

    public override void Select()
    {
        _button.onClick?.Invoke();
    }
}
