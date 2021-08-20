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

    public void OpenMenu(MenuNavigator menuNavigator)
    {
        _menu.gameObject.SetActive(true);
        _menu.ParentMenuNavigator = menuNavigator;
        menuNavigator.Active = false;
    }

    public void CloseMenu()
    {
        _menu.ParentMenuNavigator.Active = true;
        _menu.gameObject.SetActive(false);
    }
}
