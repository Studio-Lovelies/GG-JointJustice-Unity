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
    /// Creates a testable menu navigator object
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < _highlightableMenuItems.Length; i++)
        {
            var i1 = i;
            _highlightableMenuItems[i].OnMenuItemMouseOver.AddListener(() => HighlightMenuItem(i1));
        }
        
        _menuNavigator = new MenuNavigator(_initiallyHighlightedMenuItemIndex,_highlightableMenuItems.ToArray<IHightlightableMenuItem>());
    }

    private void HighlightMenuItem(int i)
    {
        _menuNavigator.SetIndex(i);
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
