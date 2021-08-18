using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for outlined buttons. These buttons use UnityEngine.UI's
/// Outline component to indicate that they are highlighted
/// </summary>
[RequireComponent(typeof(Outline))]
public class OutlinedHightlightableMenuItem : HightlightableMenuItem
{
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public override void SetHighlighted(bool highlighted)
    {
        _outline.enabled = highlighted;
    }
}
