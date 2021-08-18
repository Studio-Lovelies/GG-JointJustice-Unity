using System.Linq;
using UnityEngine;

/// <summary>
/// Class to manage menus that the user can navigate.
/// Controls highlighting of menu buttons.
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField, Tooltip("Drag all of your menu items here.")]
    private HightlightableMenuItem[] _highlightableButtons;
    
    [SerializeField, Tooltip("The first menu item that will appear highlighted. Zero indexed.")] 
    private int _initiallyHighlightedButtonIndex;

    private MenuNavigator _menuNavigator;

    /// <summary>
    /// Creates a testable menu navigator object
    /// </summary>
    private void Start()
    {
        _menuNavigator = new MenuNavigator(_initiallyHighlightedButtonIndex,_highlightableButtons.ToArray<IHightlightableMenuItem>());
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
