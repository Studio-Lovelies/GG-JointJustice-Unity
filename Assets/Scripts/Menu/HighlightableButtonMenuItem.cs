using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// A HighlightableMenuItem that specifically uses Unity buttons.
/// </summary>
[RequireComponent(typeof(Outline))]
public class HighlightableButtonMenuItem : HighlightableMenuItem
{
    private Button _button;

    protected override void Awake()
    {
        base.Awake();

        _button = GetComponent<Button>();
    }

    /// <summary>
    /// Used to disable and reenable the button component.
    /// </summary>
    /// <param name="interactable">Whether the button should be interactable or not</param>
    public override void SetInteractable(bool interactable)
    {
        base.SetInteractable(interactable);
        _button.interactable = interactable;
    }

    public override void AddOnClickListener(UnityAction listener)
    {
        _button.onClick.AddListener(listener);
    }
}
