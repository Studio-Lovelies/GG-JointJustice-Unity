using UnityEngine;
using UnityEngine.Events;
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
    private bool _wasOutlined;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _button = GetComponent<Button>();
    }

    public override void SetHighlighted(bool highlighted)
    {
        _outline.enabled = highlighted;
    }

    public override void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
        bool enabled = _outline.enabled;
        _outline.enabled = interactable && _wasOutlined;
        _wasOutlined = enabled;
    }

    public override void AddOnClickListener(UnityAction listener)
    {
        _button.onClick.AddListener(listener);
    }
}
