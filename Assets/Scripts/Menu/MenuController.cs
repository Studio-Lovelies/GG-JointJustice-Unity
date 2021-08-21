using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to manage menus that the user can navigate.
/// Controls highlighting of menu buttons.
/// </summary>
public class MenuController : MonoBehaviour
{
    [Header("Menu Settings")]
    
    [SerializeField, Tooltip("Drag all of your menu items here. Menu items will be ordered top left to right from top to bottom.")]
    private HighlightableMenuItem[] _highlightableMenuItems;
    
    [SerializeField, Min(0), Tooltip("The first menu item that will appear highlighted. Zero indexed.")] 
    private Vector2Int _initiallyHighlightedPosition;

    [SerializeField, Min(1), Tooltip("The number of rows in the menu. Used for two dimensional grid based menus")]
    private int _numberOfRows = 1;

    [Header("Navigation Options")]

    [SerializeField, Tooltip("Disabling this will stop the highlighted menu item wrapping around to the beginning after reaching the end of the menu.")]
    private bool _canWrap;

    [SerializeField, Tooltip("Enable this if the rows of the menu are vertical. This will swap the functions of the left/right arrows and up/down arrows.")]
    private bool _verticalRows;

    [SerializeField, Tooltip("Enable this to flip the x navigation")]
    private bool _flipXNavigation;
    
    [SerializeField, Tooltip("Enable this to flip the y navigation")]
    private bool _flipYNavigation;

    [SerializeField, Tooltip("Enable this to make the position reset to its default position when the menu is enabled.")]
    private bool _resetPositionOnEnable;

    private bool _canSelect;
    private MenuNavigator _menuNavigator;

    public MenuNavigator ParentMenuNavigator { get; set; }
    public bool MenuNavigatorActive => _menuNavigator.Active;
    public bool CanClose { get; set; }

    /// <summary>
    /// Subscribes to the OnMenuItemMouseOver event of all menu items, allowing for navigation with the mouse.
    /// Creates a MenuNavigator object which keeps track of which menu item is currently highlighted.
    /// If there is a button, subscribes the menu items Select method to the button's onClick event.
    /// </summary>
    private void Awake()
    {
        _menuNavigator = new MenuNavigator(_initiallyHighlightedPosition, _numberOfRows, _canWrap,_highlightableMenuItems.ToArray<IHightlightableMenuItem>());
    }

    /// <summary>
    /// Method to handle navigation through the menu.
    /// Uses options to flip x and y navigation or use vertical rows
    /// depending on the orientation of the menu.
    /// </summary>
    /// <param name="vector">The direction in which in navigate.</param>
    public void NagivateByVector(Vector2Int vector)
    {
        if (gameObject.activeInHierarchy)
        {
            if (_flipXNavigation) vector.x *= -1;
            if (_flipYNavigation) vector.y *= -1;
            
            if (_verticalRows)
            {
                Vector2Int store = vector;
                vector.x = store.y;
                vector.y = store.x;
            }
            
            _menuNavigator.IncrementPositionByVector(vector);
        }
    }

    /// <summary>
    /// Used by PlayerInput so it can select a menu item.
    /// </summary>
    public void SelectCurrentlyHighlightedMenuItem()
    {
        if (gameObject.activeInHierarchy && _canSelect)
        {
            _menuNavigator.SelectCurrentlyHighlightedMenuItem();
        }
    }

    /// <summary>
    /// Tasks that should be run when the menu controller is enabled.
    /// Starts a coroutine to create a delay that it cannot be selected on the same frame it is enabled.
    /// Resets the highlighted position to its original position of _resetPositionOnEnable is true.
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(CanSelectDelay());
        
        if (_resetPositionOnEnable)
        {
            _menuNavigator.SetPosition(_initiallyHighlightedPosition);
        }
    }

    /// <summary>
    /// Delays selecting by one frame so the menu cannot select anything on the frame it is opened.
    /// </summary>
    private IEnumerator CanSelectDelay()
    {
        _canSelect = false;
        yield return new WaitForEndOfFrame();
        _canSelect = true;
    }
}
