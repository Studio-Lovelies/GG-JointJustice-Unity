using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// /// This is an abstract class that buttons can inherit from in order to
/// implement their own highlighting methods
/// When a user hovers over or selects a button it will be
/// highlighted.
/// </summary>
[Serializable]
public abstract class HighlightableMenuItem : MonoBehaviour, IHightlightableMenuItem, IPointerEnterHandler
{ 
    public UnityEvent OnMenuItemMouseOver { get; } = new UnityEvent();
    
    /// <summary>
    /// Implement this method to tell your menu item how it should handle highlighting.
    /// </summary>
    /// <param name="highlighted">Whether the menu item should be highlighted (true) or not (false).</param>
    public abstract void SetHighlighted(bool highlighted);

    /// <summary>
    /// Invokes OnMenuItemMouseOver when the mouse is over the menu item.
    /// This is subscribed to by MenuController so it knows which button to set as highlighted when
    /// navigating menus using the mouse.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMenuItemMouseOver?.Invoke();
    }
}
