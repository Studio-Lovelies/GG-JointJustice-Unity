using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// /// This is an abstract class that buttons can inherit from in order to
/// implement their own highlighting methods
/// When a user hovers over or selects a button it will be
/// highlighted.
/// </summary>
public abstract class HighlightableMenuItem : MonoBehaviour, IHightlightableMenuItem, IPointerEnterHandler
{
    [field: SerializeField, Tooltip("This event is called when the menu item is selected or clicked on.")]
    public UnityEvent<MenuNavigator> OnSelected { get; private set; }
    
    private bool _wasHighlighted;
    private IHighlightEnabler _highlightEnabler;

    protected IHighlightEnabler HighlightEnabler => _highlightEnabler;

    public UnityEvent OnMouseOver { get; } = new UnityEvent();

    /// <summary>
    /// Searches for IHighlightEnabler interfaces
    /// </summary>
    protected virtual void Awake()
    {
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
        HighlightEnabler.SetHighlighted(highlighted);
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
        bool outlineEnabled = HighlightEnabler.HighlightEnabled;
        HighlightEnabler.SetHighlighted(interactable && _wasHighlighted);
        _wasHighlighted = outlineEnabled;
    }

    /// <summary>
    /// If the menu item has an on click event this called to add listeners to it.
    /// </summary>
    /// <param name="listener">The listener to be called when the on click event is activated.</param>
    public abstract void AddOnClickListener(UnityAction listener);
}
