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

        try
        {
            _highlightableMenuItems[CurrentIndex].SetHighlighted(true);
        }
        catch (IndexOutOfRangeException exception)
        {
            Debug.LogError($"{exception.GetType().Name}.\nInitially highlighted menu item index must be within bounds of menu items array. Was {initiallyHighlightedMenuItemIndex}. Expected 0 - {IndexCount - 1}");
        }
    }
    
    /// <summary>
    /// Increments the current index by one.
    /// Used by MenuController class for navigating using the keyboard. 
    /// </summary>
    public void IncrementPosition()
    { 
        IncrementIndexByNumber(1);
    }

    /// <summary>
    /// Increments the current index by one.
    /// Used by MenuController class for navigating using the keyboard. 
    /// </summary>
    public void DecrementPosition()
    { 
        IncrementIndexByNumber(-1);
    }

    /// <summary>
    /// Increments (or decrements) the currently highlighted index by a given number
    /// Returns without changing anything if new index is out of range
    /// of _highlightableMenuItems array
    /// </summary>
    /// <param name="number">The number to increment by</param>
    private void IncrementIndexByNumber(int number)
    { 
        _highlightableMenuItems[CurrentIndex].SetHighlighted(false);
        CurrentIndex += number;
        CurrentIndex = Mathf.Clamp(CurrentIndex, 0, IndexCount - 1);
        _highlightableMenuItems[CurrentIndex].SetHighlighted(true);
    }

    /// <summary>
    /// Sets the current index to a specific value.
    /// Used when change of index is not increment e.g. when highlighting using the mouse
    /// </summary>
    /// <param name="index">The index to be selected</param>
    public void SetIndex(int index)
    {
        _highlightableMenuItems[CurrentIndex].SetHighlighted(false);
        CurrentIndex = index;
        _highlightableMenuItems[CurrentIndex].SetHighlighted(true);
    }
}
