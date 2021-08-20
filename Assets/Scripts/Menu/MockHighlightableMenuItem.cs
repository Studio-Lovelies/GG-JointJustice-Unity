using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mock class of HighlightableMenuItem used for testing
/// </summary>
public class MockHighlightableMenuItem : IHightlightableMenuItem
{
    public UnityEvent OnMenuItemMouseOver { get; } = new UnityEvent();

    public void Select()
    {
        
    }

    public void SetHighlighted(bool highlighted)
    {
        
    }
}
