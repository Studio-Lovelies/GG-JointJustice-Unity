using System;
using UnityEngine;

/// <summary>
/// Class to keep track of the currently highlighted position in a menu.
/// Contains methods to handle menu navigation.
/// </summary>
public class MenuNavigator
{
    private int _currentIndex;
    private readonly int _indexCount;
    private readonly IHightlightableMenuItem[] _highlightableMenuItems;

    /// <summary>
    /// Initialises a MenuNavigator object which keeps track of the current highlighted position in the menu.
    /// Sets all menu items to not be highlighted, then highlights the menu item at initiallyHighlightedIndex.
    /// </summary>
    /// <param name="indexCount">The number of menu items in the menu.</param>
    /// <param name="initiallyHighlightedButtonIndex">The menu item that should be highlighted
    /// when the menu is first created.</param>
    /// <param name="highlightableMenuItems">Array of highlightable menu items.</param>
    public MenuNavigator(int indexCount, int initiallyHighlightedButtonIndex, IHightlightableMenuItem[] highlightableMenuItems)
    {
        _indexCount = indexCount;
        _currentIndex = initiallyHighlightedButtonIndex;
        _highlightableMenuItems = highlightableMenuItems;

        foreach (var highlightableButton in _highlightableMenuItems)
        {
            highlightableButton.SetHighlighted(false);
        }
        _highlightableMenuItems[_currentIndex].SetHighlighted(true);
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
        _highlightableMenuItems[_currentIndex].SetHighlighted(false);
        _currentIndex += number;
        _currentIndex = Mathf.Clamp(_currentIndex, 0, _indexCount - 1);
        _highlightableMenuItems[_currentIndex].SetHighlighted(true);
    }
}
