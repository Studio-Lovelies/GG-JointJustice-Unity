using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mock class of HighlightableMenuItem used for testing
/// </summary>
public class MockHighlightableMenuItem : IHightlightableMenuItem
{
    public UnityEvent OnMouseOver { get; } = new UnityEvent();

    public void SetHighlighted(bool highlighted)
    {
        
    }

    public void Select(MenuNavigator menuNavigator)
    {
        
    }
}
