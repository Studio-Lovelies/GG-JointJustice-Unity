using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that handles opening and closing of submenus. Disables navigation
/// of the parent menu so only items in the submenu can be accessed.
/// </summary>
public class MenuOpener : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the menu controller of the menus to open or close here.")]
    private MenuController _menuToOpen;

    private Button _button;
    private MenuController _menuController;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _menuController = GetComponentInParent<MenuController>();
    }
    
    public void OpenMenu()
    {
        if (_menuToOpen == null)
        {
            Debug.LogError($"Menu has not been set on {this}", this);
            return;
        }
            
        _menuToOpen.gameObject.SetActive(true);
        _menuToOpen.SelectInitialButton();
        _menuController.SetMenuInteractable(false);
    }
    
    public void CloseMenu()
    {
        _menuToOpen.gameObject.SetActive(false);
        _menuController.SetMenuInteractable(true);
        _button.Select();
    }

    /// <summary>
    /// Adds a frame delay when opening the menu so that is cannot close on the same frame.
    /// </summary>
    private IEnumerator CanCloseDelay()
    {
        yield return new WaitForEndOfFrame();
    }
}
