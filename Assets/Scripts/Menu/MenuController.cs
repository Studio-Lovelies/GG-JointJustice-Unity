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

    [SerializeField, Tooltip("Enable this if the rows of the menu are vertical. This will swap the functions of the left/right arrows and up/down arrows.")]
    private bool _verticalRows;
    
    private MenuNavigator _menuNavigator;
    
    public MenuNavigator ParentMenuNavigator { get; set; }
    public bool CanClose { get; set; }

    /// <summary>
    /// Subscribes to the OnMenuItemMouseOver event of all menu items, allowing for navigation with the mouse.
    /// Creates a MenuNavigator object which keeps track of which menu item is currently highlighted.
    /// </summary>
    private void Awake()
    {
        _menuNavigator = new MenuNavigator(_initiallyHighlightedPosition, _numberOfRows, _highlightableMenuItems.ToArray<IHightlightableMenuItem>());
        
        foreach (var menuItem in _highlightableMenuItems)
        {
            menuItem.GetComponent<Button>()?.onClick.AddListener(() => menuItem.Select(_menuNavigator));
        }
    }

    public void NagivateByVector(Vector2Int vector)
    {
        if (_verticalRows)
        {
            Vector2Int store = vector;
            vector.x = -store.y;
            vector.y = store.x;
        }
        
        if (gameObject.activeInHierarchy)
        {
            _menuNavigator.IncrementPositionByVector(vector);
        }
    }

    public void SelectCurrentlyHighlightedMenuItem()
    {
        if (gameObject.activeInHierarchy)
        {
            _menuNavigator.SelectCurrentlyHighlightedMenuItem();
        }
    }
}
