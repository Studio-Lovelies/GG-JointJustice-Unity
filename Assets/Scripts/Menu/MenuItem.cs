using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

/// <summary>
/// Attach this class to every item in a menu.
/// Used by MenuNavigators to set highlighting of menu items.
/// Communicates with button component to add listeners to onClick events.
/// </summary>
[RequireComponent(typeof(Button))]
public class MenuItem : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    private bool _wasHighlighted;
    private Button _button;
    private MenuController _menuController;
    private IHighlightEnabler _highlightEnabler;

    /// <summary>
    /// Gets the components required for the menu item to function.
    /// </summary>
    protected void Awake()
    {
        _button = GetComponent<Button>();
        _menuController = GetComponentInParent<MenuController>();

        if (!TryGetComponent(out _highlightEnabler))
        {
            Debug.LogError("Unable to find component with HighlightableEnabler interface.");
        }
        else
        {
            _highlightEnabler.SetHighlighted(false);
        }
    }

    /// <summary>
    /// Invokes OnMenuItemMouseOver when the mouse is over the menu item.
    /// This is subscribed to by MenuController so it knows which button to set as highlighted when
    /// navigating menus using the mouse.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _button.Select();
        _highlightEnabler?.SetHighlighted(true);
    }

    /// <summary>
    /// Sets the menu item to be intractable.
    /// Disables and re-enables the highlight.
    /// Child classes should implement this depending on how they enable/disable intractability.
    /// </summary>
    /// <param name="interactable">Whether the menu item should be interactable (true) or not (false)</param>
    public virtual void SetInteractable(bool interactable)
    {
        if (_highlightEnabler != null)
        {
            bool outlineEnabled = _highlightEnabler.HighlightEnabled;
            _highlightEnabler.SetHighlighted(interactable && _wasHighlighted);
            _wasHighlighted = outlineEnabled;
        }

        _button.interactable = interactable;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _highlightEnabler?.SetHighlighted(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _highlightEnabler?.SetHighlighted(false);
    }
}
