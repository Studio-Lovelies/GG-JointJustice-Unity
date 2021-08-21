using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Attach this class to every item in a menu.
/// Used by MenuNavigators to set highlighting of menu items.
/// Communicates with button component to add listeners to onClick events.
/// </summary>
[RequireComponent(typeof(Button))]
public class MenuItem : MonoBehaviour, IMenuItem, IPointerEnterHandler
{
    [SerializeField, Tooltip("This event is called when the menu item is selected or clicked on.")]
    private UnityEvent<MenuNavigator> OnSelected;

    [SerializeField, Tooltip("This event is called when the menu item is highlighted.")]
    private UnityEvent OnHightlighted;
    
    private bool _wasHighlighted;
    private Button _button;
    private IHighlightEnabler _highlightEnabler;

    public UnityEvent OnMouseOver { get; } = new UnityEvent();

    /// <summary>
    /// Gets the components required for the menu item to function.
    /// </summary>
    protected void Awake()
    {
        _button = GetComponent<Button>();
        if (!TryGetComponent(out _highlightEnabler))
        {
            Debug.LogError("Unable to find component with HighlightableEnabler interface.");
        }
    }
    
    /// <summary>
    /// Used to call the SetHighlighted method on the IHighlightEnabler
    /// </summary>
    /// <param name="highlighted">Whether the menu item should be highlighted (true) or not (false).</param>
    public void SetHighlighted(bool highlighted)
    {
        _highlightEnabler?.SetHighlighted(highlighted);
        OnHightlighted?.Invoke();
    }

    /// <summary>
    /// Method that is called when a menu is selected.
    /// A menu item might be selected if a user highlights it and presses a specifies key.
    /// </summary>
    /// <param name="menuNavigator">The menu navigator of the menu the button belongs to. Passed so it can be set active or inactive.</param>
    public void Select(MenuNavigator menuNavigator)
    {
        if (!menuNavigator.Active) return;
        OnSelected?.Invoke(menuNavigator);
    }
    
    /// <summary>
    /// Invokes OnMenuItemMouseOver when the mouse is over the menu item.
    /// This is subscribed to by MenuController so it knows which button to set as highlighted when
    /// navigating menus using the mouse.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseOver?.Invoke();
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

    /// <summary>
    /// Method called by MenuNavigators to add listeners to the menu item's button.
    /// </summary>
    /// <param name="listener">The listener to be called when the on click event is activated.</param>
    public void AddOnClickListener(UnityAction listener)
    {
        // if (_button == null) _button = GetComponent<Button>(); // In case this is called before Awake method
        _button.onClick.AddListener(listener);
    }
}
