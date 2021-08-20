using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for navigating two dimension (or grid based) menus.
/// </summary>
public class MenuNavigator2D : MenuNavigator
{
    private readonly IHightlightableMenuItem[,] _highlightableMenuItems2D;
    private int _currentRow;

    public int RowCount { get; set; }
    public Vector2Int CurrentPosition => new Vector2Int(CurrentIndex, _currentRow);

    /// <summary>
    /// Calls the base constructor, and translates the given array into a 2D array.
    /// </summary>
    /// <param name="initiallyHighlightedMenuItemIndex">The menu item that should be highlighted
    /// when the menu is first created.</param>
    /// <param name="numberOfMenuItemsPerRow">The number of items in a row. Used to calculate how the array should be stored in a 2D array.</param>
    /// <param name="highlightableMenuItems">Array of highlightable menu items.</param>
    public MenuNavigator2D(int initiallyHighlightedMenuItemIndex, int numberOfMenuItemsPerRow, IHightlightableMenuItem[] highlightableMenuItems) : base(initiallyHighlightedMenuItemIndex, highlightableMenuItems)
    {
        IndexCount = numberOfMenuItemsPerRow;
        RowCount = highlightableMenuItems.Length / numberOfMenuItemsPerRow;
        _highlightableMenuItems2D = new IHightlightableMenuItem[numberOfMenuItemsPerRow, RowCount];
    }

    protected override void SetUpMenuItems(IHightlightableMenuItem[] highlightableMenuItems)
    {
        int currentIndexOfHighlightableMenuItems = 0;
        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < IndexCount; x++)
            {
                _highlightableMenuItems2D[x, y] = highlightableMenuItems[currentIndexOfHighlightableMenuItems++];
                Debug.Log(_highlightableMenuItems2D[x, y]);
                int index = x; // This prevents the arguments passed to OnMenuItemMouseOver event from changing
                int row = y;
                
                _highlightableMenuItems2D[x, y].OnMenuItemMouseOver.AddListener(() =>
                {
                    SetHighlighted(false);
                    CurrentIndex = index;
                    _currentRow = row;
                    SetHighlighted(true);
                });
            }
        }
    }

    /// <summary>
    /// Increments the current row by one.
    /// Used by MenuController class for navigating using the keyboard. 
    /// </summary>
    public void IncrementRow()
    {
        IncrementRowByNumber(ref _currentRow, -1);
    }
    
    /// <summary>
    /// Decrements the current row by one.
    /// Used by MenuController class for navigating using the keyboard. 
    /// </summary>
    public void DecrementRow()
    {
        IncrementRowByNumber(ref _currentRow, 1);
    }

    /// <summary>
    /// Increments (or decrements) the current row by a given number.
    /// If new position is out of range, wraps around to the other side of the menu.
    /// </summary>
    /// <param name="referenceRow">A reference to the integer that stores the current row.</param>
    /// <param name="number">The number to increment by.</param>
    private void IncrementRowByNumber(ref int referenceRow, int number)
    {
        SetHighlighted(false);
        referenceRow += number;
        referenceRow = Mathf.Clamp(referenceRow, 0, RowCount - 1);
        SetHighlighted(true);
    }

    /// <summary>
    /// Sets the highlighted bool of the menu item at position CurrentIndex.
    /// Adjusted to use the current index and row.
    /// </summary>
    /// <param name="highlighted">Whether the menu item should be highlighted (true) or not (false)</param>
    protected override void SetHighlighted(bool highlighted)
    {
        CurrentlyHighlightedMenuItem = _highlightableMenuItems2D[CurrentIndex, _currentRow];
        _highlightableMenuItems2D[CurrentIndex, _currentRow].SetHighlighted(highlighted);
    }
}
