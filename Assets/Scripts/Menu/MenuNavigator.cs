using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class to keep track of the currently highlighted position in a menu.
/// Contains methods to handle menu navigation.
/// Set Active to false to ignore inputs.
/// </summary>
public class MenuNavigator
{
    private readonly IHightlightableMenuItem[,] _highlightableMenuItems;
    private UnityEvent<bool> _onActive = new UnityEvent<bool>();
    private bool _active = true;
    private bool _canWrap;

    public int ColumnCount { get; private set; }
    public int RowCount { get; private set; }
    public Vector2Int CurrentPosition { get; set; }
    public bool Active
    {
        get => _active;
        set
        {
            _active = value;
            _onActive?.Invoke(value);
        }
    }

    /// <summary>
    /// Initialises a MenuNavigator object which keeps track of the current highlighted position in the menu.
    /// Sets all menu items to not be highlighted, then highlights the menu item at initiallyHighlightedIndex.
    /// </summary>
    /// <param name="initiallyHighlightedPosition">The menu item that should be highlighted
    ///     when the menu is first created.</param>
    /// <param name="numberOfRows">The number of rows in the menu. Used to calculate how the 2D array should be created.</param>
    /// <param name="highlightableMenuItems">Array of highlightable menu items.</param>
    /// <param name="canWrap"></param>
    public MenuNavigator(Vector2Int initiallyHighlightedPosition, int numberOfRows, bool canWrap,
        IHightlightableMenuItem[] highlightableMenuItems)
    {
        CurrentPosition = initiallyHighlightedPosition;
        RowCount = numberOfRows;
        ColumnCount = highlightableMenuItems.Length / numberOfRows;
        _canWrap = canWrap;
        _highlightableMenuItems = new IHightlightableMenuItem[ColumnCount, RowCount];

        ConvertTo2DArrayAndAddListeners(highlightableMenuItems);
        ClampCurrentPosition();
        
        _highlightableMenuItems[CurrentPosition.x, CurrentPosition.y].SetHighlighted(true);
    }

    /// <summary>
    /// Clamps CurrentPosition so it does not lie outside of the bounds of HighlightableMenuItems
    /// </summary>
    private void ClampCurrentPosition()
    {
        Vector2Int position = CurrentPosition;
        position = new Vector2Int(Mathf.Clamp(position.x, 0, ColumnCount), Mathf.Clamp(position.y, 0, RowCount));
        CurrentPosition = position;
    }

    /// <summary>
    /// Converts the given 1D array into a 2D array so it can be accessed using a set of coordinates.
    /// Subscribes SetPosition to each menu item's OnMouseOver event.
    /// Subscribes SelectCurrentlyHighlightedMenuItem to each menu item's on click event.
    /// </summary>
    /// <param name="highlightableMenuItems">The 1D array to convert.</param>
    private void ConvertTo2DArrayAndAddListeners(IHightlightableMenuItem[] highlightableMenuItems)
    {
        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < ColumnCount; x++)
            {
                _highlightableMenuItems[x, y] = highlightableMenuItems[x + ColumnCount * y];
                int column = x; // This prevents the arguments passed to OnMenuItemMouseOver event from changing
                int row = y;
                _highlightableMenuItems[x, y].OnMouseOver.AddListener(() => SetPosition(new Vector2Int(column, row)));
                _highlightableMenuItems[x, y].SetHighlighted(false);
                _highlightableMenuItems[x, y].AddOnClickListener(() =>
                {
                    SetPosition(new Vector2Int(column, row));
                    SelectCurrentlyHighlightedMenuItem();
                });
                _onActive.AddListener(value => _highlightableMenuItems[column, row].SetInteractable(value));
            }
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
        
        // Either clamp or wrap the position depending on value of _canWrap
        CurrentPosition =_canWrap ?
            new Vector2Int(
            (CurrentPosition.x + ColumnCount) % ColumnCount,
            (CurrentPosition.y + RowCount) % RowCount) :
            new Vector2Int(
                Mathf.Clamp(CurrentPosition.x, 0, ColumnCount - 1), 
                Mathf.Clamp(CurrentPosition.y, 0, RowCount - 1));

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
