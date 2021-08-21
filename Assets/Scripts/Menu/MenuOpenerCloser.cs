using System.Collections;
using UnityEngine;

/// <summary>
/// Class that handles opening and closing of submenus. Disables navigation
/// of the parent menu so only items in the submenu can be accessed.
/// </summary>
public class MenuOpenerCloser : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the menu controller of the menus to open or close here.")]
    private MenuController[] _menus;

    /// <summary>
    /// Opens the menu, and disables and stores the menu navigator of the parent so it can be re-enabled later.
    /// </summary>
    /// <param name="menuNavigator">The parent's menu navigator</param>
    public void OpenMenu(MenuNavigator menuNavigator)
    {
        foreach (var menu in _menus)
        {
            if (menu == null)
            {
                Debug.LogError($"Menu has not been set on {this}", this);
                return;
            }

        StartCoroutine(CanCloseDelay(menu));
        menu.gameObject.SetActive(true);
        menu.ParentMenuNavigator = menuNavigator;
        menuNavigator.Active = false;
        }
    }

    /// <summary>
    /// Closes the window and re-enables the parent's menu navigator.
    /// </summary>
    /// <param name="inactiveOverrideClose">Whether the menu should be closed even if it is inactive.</param>
    public void CloseMenu(bool inactiveOverrideClose = false)
    {
        foreach (var menu in _menus)
        {
            if (!menu.gameObject.activeInHierarchy || (!menu.MenuNavigatorActive && !inactiveOverrideClose)) return;

            menu.ParentMenuNavigator.Active = true;
            menu.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Adds a frame delay when opening the menu so that is cannot close on the same frame.
    /// </summary>
    private IEnumerator CanCloseDelay(MenuController menu)
    {
        menu.CanClose = false;
        yield return new WaitForEndOfFrame();
        menu.CanClose = true;
    }
}
