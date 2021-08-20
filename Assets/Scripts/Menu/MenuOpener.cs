using System.Collections;
using UnityEngine;

/// <summary>
/// Class that handles opening of submenus. Disables navigation
/// of the parent menu so only items in the submenu can be accessed.
/// </summary>
public class MenuOpener : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the menu controller of the menu to open here.")]
    private MenuController _menu;
    
    private MenuNavigator _menuNavigator;

    /// <summary>
    /// Opens the menu, and disables and stores the menu navigator of the parent so it can be re-enabled later.
    /// </summary>
    /// <param name="menuNavigator">The parent's menu navigator</param>
    public void OpenMenu(MenuNavigator menuNavigator)
    {
        StartCoroutine(CanCloseDelay());
        _menu.gameObject.SetActive(true);
        _menu.ParentMenuNavigator = menuNavigator;
        menuNavigator.Active = false;
    }

    /// <summary>
    /// Closes the window and re-enables the parents menu navigator.
    /// </summary>
    public void CloseMenu()
    {
        if (!_menu.CanClose) return;
        
        _menu.ParentMenuNavigator.Active = true;
        _menu.gameObject.SetActive(false);
    }

    /// <summary>
    /// Adds a frame delay when opening the menu so that is cannot close on the same frame.
    /// </summary>
    private IEnumerator CanCloseDelay()
    {
        _menu.CanClose = false;
        yield return new WaitForEndOfFrame();
        _menu.CanClose = true;
    }
}
