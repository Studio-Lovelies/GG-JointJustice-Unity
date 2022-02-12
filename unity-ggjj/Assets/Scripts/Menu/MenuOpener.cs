using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Class that handles opening and closing of submenus. Disables navigation
/// of the parent menu so only items in the child menu can be accessed.
/// </summary>
public class MenuOpener : MonoBehaviour
{
    [field: Tooltip("Drag the menu controller of the menus to open or close here.")]
    [field: SerializeField]
    public Menu MenuToOpen { get; set; }
    
    [Tooltip("This event is called when the menu is enabled")]
    [SerializeField]
    private UnityEvent _onMenuOpened;
    
    [Tooltip("This event is called when the menu is disabled")]
    [SerializeField]
    private UnityEvent _onMenuClosed;
    

    private Button _button;
    private Selectable _cachedSelectedButtonAfterClose;
    private Menu _parentMenu;
    private bool _menuCannotBeOpened;
    private bool _menuOpenedThisFrame;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _parentMenu = GetComponentInParent<Menu>();

        // Don't disable self when opening menu if opening menu this is part of
        if (_parentMenu == MenuToOpen)
        {
            _parentMenu = null;
        }
    }
    
    /// <summary>
    /// Opens the designated menu. Sets the current menu to be inactive
    /// and sets the initially selected button.
    /// </summary>
    public void OpenMenu()
    {
        if (_menuCannotBeOpened)
        {
            return;
        }
        
        if (MenuToOpen == null)
        {
            Debug.LogError($"Menu has not been set on {this}", this);
            return;
        }

        MenuToOpen.gameObject.SetActive(true);

        if (_parentMenu != null)
        {
            _parentMenu.SetMenuInteractable(false);
        }

        if (MenuToOpen.DontResetSelectedOnClose && _cachedSelectedButtonAfterClose != null && _cachedSelectedButtonAfterClose.isActiveAndEnabled)
        {
            _cachedSelectedButtonAfterClose.Select();
        }
        else
        {
            MenuToOpen.SelectInitialButton();
        }

        _onMenuOpened.Invoke();
    }
    
    /// <summary>
    /// Closes the previously opened menu if that menu does not currently have any child menus open.
    /// </summary>
    public void CloseMenu()
    {
        if (_menuCannotBeOpened)
        {
            return;
        }
        
        ForceCloseMenu();
    }

    /// <summary>
    /// Closes the menu but ignores the _menuCannotBeOpened bool
    /// </summary>
    public void ForceCloseMenu()
    {
        if (!MenuToOpen.Active || _menuOpenedThisFrame) return;
        
        MenuToOpen.gameObject.SetActive(false);
        
        if (_parentMenu != null)
        {
            _parentMenu.SetMenuInteractable(true);
            _button.Select();
        }

        if (MenuToOpen.DontResetSelectedOnClose)
        {
            _cachedSelectedButtonAfterClose = MenuToOpen.SelectedButton;
        }

        _onMenuClosed.Invoke();

        _menuCannotBeOpened = false;
    }

    /// <summary>
    /// Toggles a menu between open and closed
    /// </summary>
    public void ToggleMenu()
    {
        if (MenuToOpen.Active)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    /// <summary>
    /// Sets whether the menu can be opened.
    /// Should be subscribed to events that will disable opening of menus.
    /// Specifically states CANNOT be opened so it can be set by value of _isBusy in DialogueController
    /// </summary>
    /// <param name="menuCannotBeOpened">Whether the menu can be opened (false) or or not (true)</param>
    public void SetMenuCannotBeOpened(bool menuCannotBeOpened)
    {
        _menuCannotBeOpened = menuCannotBeOpened;
    }

    /// <summary>
    /// Starts CanCloseDelay on enable so the menu cannot close on the same frame
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(CanCloseDelay());
    }

    /// <summary>
    /// Waits one frame then enables closing of the menu so the menu cannot close on the same frame.
    /// </summary>
    private IEnumerator CanCloseDelay()
    {
        _menuOpenedThisFrame = true;
        yield return null;
        _menuOpenedThisFrame = false;
    }
}
