using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MockHighlightableMenuItem : IHightlightableMenuItem
{
    public UnityEvent OnMenuItemMouseOver { get; } = new UnityEvent();

    public void SetHighlighted(bool highlighted)
    {
        
    }

    public void Select()
    {
        
    }
}
