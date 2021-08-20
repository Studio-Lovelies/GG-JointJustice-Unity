using System;
using UnityEngine;

/// <summary>
/// Class to keep track of the currently highlighted position in a menu.
/// Contains methods to handle menu navigation.
/// </summary>
public class MenuNavigator
{
    private readonly IHightlightableMenuItem[] _highlightableMenuItems;
    private int _currentIndex;

    protected IHightlightableMenuItem CurrentlyHighlightedMenuItem;

    public int IndexCount { get; protected set; }

    public int CurrentIndex
    {
        get => _currentIndex;
        protected set => _currentIndex = value;
    }

    /// <summary>
    /// Initialises a MenuNavigator object which keeps track of the current highlighted position in the menu.
    /// Sets all menu items to not be highlighted, then highlights the menu item at initiallyHighlightedIndex.
    /// </summary>
    /// <param name="initiallyHighlightedMenuItemIndex">The menu item that should be highlighted
    /// when the menu is first created.</param>
    /// <param name="highlightableMenuItems">Array of highlightable menu items.</param>
    public MenuNavigator(int initiallyHighlightedMenuItemIndex, IHightlightableMenuItem[] highlightableMenuItems)
    {
        IndexCount = highlightableMenuItems.Length;
        _currentIndex = initiallyHighlightedMenuItemIndex;
        _highlightableMenuItems = highlightableMenuItems;

        SetUpMenuItems(highlightableMenuItems);
        
        try
        {
            _highlightableMenuItems[CurrentIndex].SetHighlighted(true);
        }
        catch (IndexOutOfRangeException exception)
        {
            Debug.LogError(
                $"{exception.GetType().Name}.\nInitially highlighted menu item index must be within bounds of menu items array. Was {initiallyHighlightedMenuItemIndex}. Expected 0 - {IndexCount - 1}");
        }
    }

    protected virtual void SetUpMenuItems(IHightlightableMenuItem[] highlightableMenuItems)
    {
        for (int i = 0; i < IndexCount; i++)
        {
            var index = i; // This prevents the arguments passed to OnMenuItemMouseOver event from changing
            highlightableMenuItems[i].OnMenuItemMouseOver.AddListener(() => { SetIndex(ref _currentIndex, index); });
            highlightableMenuItems[i].SetHighlighted(false);
        }
    }

    /// <summary>
    /// Increments the current index by one.
    /// Used by MenuController class for navigating using the keyboard. 
    /// </summary>
    public void IncrementPosition()
    { 
        IncrementIndexByNumber(ref _currentIndex, 1);
    }

    /// <summary>
    /// Increments the current index by one.
    /// Used by MenuController class for navigating using the keyboard. 
    /// </summary>
    public void DecrementPosition()
    { 
        IncrementIndexByNumber(ref _currentIndex, -1);
    }

    /// <summary>
    /// Increments (or decrements) the currently highlighted index by a given number
    /// If new position is out of range, wraps around to the other side of the menu.
    /// </summary>
    /// <param name="referenceIndex">A reference to the integer that stores the current index.</param>
    /// <param name="number">The number to increment by.</param>
    private void IncrementIndexByNumber(ref int referenceIndex, int number)
    { 
        SetHighlighted(false);
        referenceIndex += number;
        referenceIndex = Mathf.Clamp(referenceIndex, 0, IndexCount - 1);
        SetHighlighted(true);
    }

    /// <summary>
    /// Sets the current index to a specific value.
    /// Used when change of index is not increment e.g. when highlighting using the mouse
    /// </summary>
    /// <param name="referenceIndex">A reference to the integer that stores the current index.</param>
    /// <param name="targetIndex">The index to change the reference index to.</param>
    public void SetIndex(ref int referenceIndex, int targetIndex)
    {
        SetHighlighted(false);
        referenceIndex = targetIndex;
        SetHighlighted(true);
    }

    /// <summary>
    /// Sets the highlighted bool of the menu item at position CurrentIndex
    /// </summary>
    /// <param name="highlighted">Whether the menu item should be highlighted (true) or not (false)</param>
    protected virtual void SetHighlighted(bool highlighted)
    {
        CurrentlyHighlightedMenuItem = _highlightableMenuItems[CurrentIndex];
        _highlightableMenuItems[CurrentIndex].SetHighlighted(highlighted);
    }
    
    /// <summary>
    /// Selects the menu item at CurrentIndex
    /// </summary>
    public void SelectHighlightedMenuItem()
    {
        CurrentlyHighlightedMenuItem.Select();
    }
}
