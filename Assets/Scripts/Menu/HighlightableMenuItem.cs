using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// /// This is an abstract class that buttons can inherit from in order to
/// implement their own highlighting methods
/// When a user hovers over or selects a button it will be
/// highlighted.
/// </summary>
[Serializable]
public abstract class HighlightableMenuItem : MonoBehaviour, IHightlightableMenuItem, IPointerEnterHandler
{
    [field: SerializeField] public UnityEvent<MenuNavigator> OnSelected { get; private set; }
    [field: SerializeField] public UnityEvent<MenuNavigator> OnClick { get; private set; }
    public UnityEvent OnMouseOver { get; } = new UnityEvent();

    /// <summary>
    /// Implement this method to tell your menu item how it should handle highlighting.
    /// </summary>
    /// <param name="highlighted">Whether the menu item should be highlighted (true) or not (false).</param>
    public abstract void SetHighlighted(bool highlighted);

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
    /// Child classes should implement this depending on how they enable/disable intractability.
    /// </summary>
    /// <param name="interactable">Whether the menu item should be interactable (true) or not (false)</param>
    public abstract void SetInteractable(bool interactable);

    /// <summary>
    /// If the menu item has an on click event this called to add listeners to it.
    /// </summary>
    /// <param name="listener">The listener to be called when the on click event is activated.</param>
    public abstract void AddOnClickListener(UnityAction listener);
}
