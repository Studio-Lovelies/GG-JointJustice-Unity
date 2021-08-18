using System;
using UnityEngine;

/// <summary>
/// Class to keep track of the currently highlighted position in a menu.
/// Contains methods to handle menu navigation.
/// </summary>
public class MenuNavigator
{
    private readonly IHightlightableMenuItem[] _highlightableMenuItems;

    public int IndexCount => _highlightableMenuItems.Length;
    public int CurrentIndex { get; private set; }
    
    /// <summary>
    /// Initialises a MenuNavigator object which keeps track of the current highlighted position in the menu.
    /// Sets all menu items to not be highlighted, then highlights the menu item at initiallyHighlightedIndex.
    /// </summary>
    /// <param name="indexCount">The number of menu items in the menu.</param>
    /// <param name="initiallyHighlightedMenuItemIndex">The menu item that should be highlighted
    /// when the menu is first created.</param>
    /// <param name="highlightableMenuItems">Array of highlightable menu items.</param>
    public MenuNavigator(int initiallyHighlightedMenuItemIndex, IHightlightableMenuItem[] highlightableMenuItems)
    {
        CurrentIndex = initiallyHighlightedMenuItemIndex;
        _highlightableMenuItems = highlightableMenuItems;

        foreach (var highlightableButton in _highlightableMenuItems)
        {
            highlightableButton.SetHighlighted(false);
        }
        _highlightableMenuItems[CurrentIndex].SetHighlighted(true);
    }
    
    public void IncrementPosition()
    { 
        IncrementPositionByNumber(1);
    }

    public void DecrementPosition()
    { 
        IncrementPositionByNumber(-1);
    }

    /// <summary>
    /// Increments (or decrements) the currently highlighted index by a given number
    /// Returns without changing anything if new index is out of range
    /// of _highlightableMenuItems array
    /// </summary>
    /// <param name="number">The number to increment by</param>
    private void IncrementPositionByNumber(int number)
    {
        _highlightableMenuItems[CurrentIndex].SetHighlighted(false);
        CurrentIndex += number;
        CurrentIndex = Mathf.Clamp(CurrentIndex, 0, IndexCount - 1);
        _highlightableMenuItems[CurrentIndex].SetHighlighted(true);
    }
}
