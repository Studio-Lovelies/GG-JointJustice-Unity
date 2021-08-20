using System;
using Codice.CM.Common;
using UnityEngine;

/// <summary>
/// Class to keep track of the currently highlighted position in a menu.
/// Contains methods to handle menu navigation.
/// Set Active to false to ignore inputs.
/// </summary>
public class MenuNavigator
{
    private readonly IHightlightableMenuItem[,] _highlightableMenuItems;
    
    public int ColumnCount { get; private set; }
    public int RowCount { get; private set; }
    public Vector2Int CurrentPosition { get; set; }
    public bool Active { get; set; } = true;

    /// <summary>
    /// Initialises a MenuNavigator object which keeps track of the current highlighted position in the menu.
    /// Sets all menu items to not be highlighted, then highlights the menu item at initiallyHighlightedIndex.
    /// </summary>
    /// <param name="indexCount">The number of menu items in the menu.</param>
    /// <param name="initiallyHighlightedPosition">The menu item that should be highlighted
    /// when the menu is first created.</param>
    /// <param name="highlightableMenuItems">Array of highlightable menu items.</param>
    public MenuNavigator(Vector2Int initiallyHighlightedPosition, int numberOfRows, IHightlightableMenuItem[] highlightableMenuItems)
    {
        CurrentPosition = initiallyHighlightedPosition;
        RowCount = numberOfRows;
        ColumnCount = highlightableMenuItems.Length / numberOfRows;
        _highlightableMenuItems = new IHightlightableMenuItem[ColumnCount, RowCount];

        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < ColumnCount; x++)
            {
                _highlightableMenuItems[x, y] = highlightableMenuItems[x + ColumnCount * y];
                int column = x; // This prevents the arguments passed to OnMenuItemMouseOver event from changing
                int row = y;
                _highlightableMenuItems[x, y].OnMouseOver.AddListener(() => SetPosition(new Vector2Int(column ,row)));
                _highlightableMenuItems[x, y].SetHighlighted(false);
            }
        }

        try
        {
            _highlightableMenuItems[CurrentPosition.x, CurrentPosition.y].SetHighlighted(true);
        }
        catch (IndexOutOfRangeException exception)
        {
            Debug.LogError($"{exception.GetType().Name}.\nInitially highlighted menu item index must be within bounds of menu items array. Was {initiallyHighlightedPosition}. Expected (0 - {ColumnCount - 1})(0 - {RowCount - 1})");
        }
    }

    /// <summary>
    /// Increments (or decrements) the currently highlighted index by a given number
    /// If new position is out of range, wraps around to the other side of the menu.
    /// </summary>
    /// <param name="number">The number to increment by</param>
    public void IncrementPositionByVector(Vector2Int vector)
    {
        if (!Active) return;
        
        _highlightableMenuItems[CurrentPosition.x, CurrentPosition.y].SetHighlighted(false);
        CurrentPosition += vector;
        CurrentPosition = new Vector2Int(
            (CurrentPosition.x + ColumnCount) % ColumnCount,
            (CurrentPosition.y + RowCount) % RowCount);
        _highlightableMenuItems[CurrentPosition.x, CurrentPosition.y].SetHighlighted(true);
    }

    /// <summary>
    /// Sets the current index to a specific value.
    /// Used when change of index is not increment e.g. when highlighting using the mouse
    /// </summary>
    /// <param name="index">The index to be selected</param>
    public void SetPosition(Vector2Int position)
    {
        if (!Active) return;
        
        _highlightableMenuItems[CurrentPosition.x, CurrentPosition.y].SetHighlighted(false);
        CurrentPosition = position;
        _highlightableMenuItems[CurrentPosition.x, CurrentPosition.y].SetHighlighted(true);
    }

    /// <summary>
    /// Calls the menu item's select method.
    /// </summary>
    public void SelectCurrentlyHighlightedMenuItem()
    {
        if (!Active) return;
        
        _highlightableMenuItems[CurrentPosition.x, CurrentPosition.y].Select(this);
    }
}
