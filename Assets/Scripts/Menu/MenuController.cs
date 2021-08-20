using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to manage menus that the user can navigate.
/// Controls highlighting of menu buttons.
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField, Tooltip("Drag all of your menu items here. Menu items will be ordered top left to right from top to bottom.")]
    private HighlightableMenuItem[] _highlightableMenuItems;
    
    [SerializeField, Min(0), Tooltip("The first menu item that will appear highlighted. Zero indexed.")] 
    private Vector2Int _initiallyHighlightedPosition;

    [SerializeField, Min(1), Tooltip("The number of rows in the menu. Used for two dimensional grid based menus")]
    private int _numberOfRows = 1;
    
    private MenuNavigator _menuNavigator;
    
    public MenuNavigator ParentMenuNavigator { get; set; }

    /// <summary>
    /// Subscribes to the OnMenuItemMouseOver event of all menu items, allowing for navigation with the mouse.
    /// Creates a MenuNavigator object which keeps track of which menu item is currently highlighted.
    /// </summary>
    private void Start()
    {
        _menuNavigator = new MenuNavigator(_initiallyHighlightedPosition, _numberOfRows, _highlightableMenuItems.ToArray<IHightlightableMenuItem>());
        
        foreach (var menuItem in _highlightableMenuItems)
        {
            menuItem.GetComponent<Button>()?.onClick.AddListener(() => menuItem.Select(_menuNavigator));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _menuNavigator.IncrementPositionByVector(Vector2Int.left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _menuNavigator.IncrementPositionByVector(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _menuNavigator.IncrementPositionByVector(Vector2Int.down);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _menuNavigator.IncrementPositionByVector(Vector2Int.up);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _menuNavigator.SelectCurrentlyHighlightedMenuItem();
        }
    }
    
    public void SelectMenuItem()
    {
        _menuNavigator.SelectCurrentlyHighlightedMenuItem();
    }
}
