using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A version of the MenuController class adjusted to handle 2D (grid layout) menus.
/// </summary>
public class MenuController2D : MenuController
{
    [SerializeField, Min(0),
     Tooltip("The number of menu items in one row of the menu. Set to zero if there is only one row")]
    private int _numberOfMenuItemsPerRow;

    private MenuNavigator2D _menuNavigator2D;
    
    /// <summary>
    /// Subscribes to the OnMenuItemMouseOver event of all menu items, allowing for navigation with the mouse.
    /// Creates a MenuNavigator object which keeps track of which menu item is currently highlighted.
    /// </summary>
    private void Start()
    { 
        _menuNavigator2D = new MenuNavigator2D(_initiallyHighlightedMenuItemIndex, _numberOfMenuItemsPerRow, _highlightableMenuItems.ToArray<IHightlightableMenuItem>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _menuNavigator2D.DecrementPosition();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _menuNavigator2D.IncrementPosition();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _menuNavigator2D.SelectHighlightedMenuItem();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _menuNavigator2D.DecrementRow();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _menuNavigator2D.IncrementRow();
        }
    }
}
