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
    
    public abstract void SetHighlighted(bool highlighted);

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMenuItemMouseOver?.Invoke();
    }
}
