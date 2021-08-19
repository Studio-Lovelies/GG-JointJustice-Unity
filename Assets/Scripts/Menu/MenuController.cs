using System.Linq;
using UnityEngine;

/// <summary>
/// Class to manage menus that the user can navigate.
/// Controls highlighting of menu buttons.
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField, Tooltip("Drag all of your menu items here.")]
    private HighlightableMenuItem[] _highlightableMenuItems;
    
    [SerializeField, Min(0), Tooltip("The first menu item that will appear highlighted. Zero indexed.")] 
    private int _initiallyHighlightedMenuItemIndex;

    private MenuNavigator _menuNavigator;

    /// <summary>
    /// Subscribes to the OnMenuItemMouseOver event of all menu items, allowing for navigation with the mouse.
    /// Creates a MenuNavigator object which keeps track of which menu item is currently highlighted.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < _highlightableMenuItems.Length; i++)
        {
            var index = i; // This prevents the argument of HighlightMenuItem from changing
            _highlightableMenuItems[i].OnMenuItemMouseOver.AddListener(() => _menuNavigator.SetIndex(index));
        }
        
        _menuNavigator = new MenuNavigator(_initiallyHighlightedMenuItemIndex,_highlightableMenuItems.ToArray<IHightlightableMenuItem>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _menuNavigator.DecrementPosition();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _menuNavigator.IncrementPosition();
        }
    }
}
