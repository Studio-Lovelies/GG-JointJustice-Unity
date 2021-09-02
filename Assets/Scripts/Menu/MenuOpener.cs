using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Class that handles opening and closing of submenus. Disables navigation
/// of the parent menu so only items in the child menu can be accessed.
/// </summary>
public class MenuOpener : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the scene's dialogue controller here")]
    private DialogueController _dialogueController;    

    [SerializeField, Tooltip("Drag the menu controller of the menus to open or close here.")]
    private Menu _menuToOpen;
    
    [SerializeField, Tooltip("This event is called when the menu is enabled")]
    private UnityEvent _onMenuOpened;
    
    [SerializeField, Tooltip("This event is called when the menu is disabled")]
    private UnityEvent _onMenuClosed;

    private Button _button;
    private Selectable _cachedSelectedButtonAfterClose;
    private Menu _parentMenu;

    private void Awake()
    {
        if (_dialogueController == null)
        {
            Debug.LogWarning($"DialogController has not been assigned to MenuOpener on {gameObject.name}. Menu will be accessible when scene's DialogueController is busy.");
        }
        
        _button = GetComponent<Button>();
        _parentMenu = GetComponentInParent<Menu>();
        
        // Don't disable self on line 44 if opening self
        if (_parentMenu == _menuToOpen)
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
        if (_dialogueController != null && _dialogueController.IsBusy)
        {
            Debug.LogWarning("Cannot open menu, DialogueController is busy.");
            return;
        }
        
        if (_menuToOpen == null)
        {
            Debug.LogError($"Menu has not been set on {this}", this);
            return;
        }
        
        _menuToOpen.gameObject.SetActive(true);

        if (_parentMenu != null)
        {
            _parentMenu.SetMenuInteractable(false);
        }

        if (_menuToOpen.DontResetSelectedOnClose && _cachedSelectedButtonAfterClose != null)
        {
            _cachedSelectedButtonAfterClose.Select();
        }
        else
        {
            _menuToOpen.SelectInitialButton();
        }

        _onMenuOpened.Invoke();
    }
    
    /// <summary>
    /// Closes the previously opened menu if it that menu does not currently have any child menus open.
    /// </summary>
    public void CloseMenu()
    {
        if (_dialogueController != null && _dialogueController.IsBusy)
        {
            Debug.LogWarning("Cannot close menu, DialogueController is busy.");
            return;
        }
        
        if (!_menuToOpen.Active) return;
        
        _menuToOpen.gameObject.SetActive(false);
        
        if (_parentMenu != null)
        {
            _parentMenu.SetMenuInteractable(true);
            _button.Select();
        }

        if (_menuToOpen.DontResetSelectedOnClose)
        {
            _cachedSelectedButtonAfterClose = _menuToOpen.SelectedButton;
        }

        _onMenuClosed.Invoke();
    }

    /// <summary>
    /// Toggles a menu between open and closed
    /// </summary>
    public void ToggleMenu()
    {
        if (_menuToOpen.Active)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
}
