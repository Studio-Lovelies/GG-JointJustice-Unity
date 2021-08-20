using System.Linq;
using UnityEngine;

/// <summary>
/// Class to manage menus that the user can navigate.
/// Controls highlighting of menu buttons.
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField, Tooltip("Drag all of your menu items here.")]
    protected HighlightableMenuItem[] _highlightableMenuItems;
    
    [SerializeField, Min(0), Tooltip("The first menu item that will appear highlighted. Zero indexed.")] 
    protected int _initiallyHighlightedMenuItemIndex;

    private MenuNavigator _menuNavigator;

    /// <summary>
    /// Subscribes to the OnMenuItemMouseOver event of all menu items, allowing for navigation with the mouse.
    /// Creates a MenuNavigator object which keeps track of which menu item is currently highlighted.
    /// </summary>
    private void Start()
    { 
        _menuNavigator = new MenuNavigator(_initiallyHighlightedMenuItemIndex, _highlightableMenuItems.ToArray<IHightlightableMenuItem>());
    }
}
