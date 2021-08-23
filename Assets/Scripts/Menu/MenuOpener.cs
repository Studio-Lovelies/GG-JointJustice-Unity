using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that handles opening and closing of submenus. Disables navigation
/// of the parent menu so only items in the child menu can be accessed.
/// </summary>
public class MenuOpener : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the menu controller of the menus to open or close here.")]
    private MenuController _menuToOpen;

    private Button _button;
    private Selectable _previouslySelectedButton;
    private MenuController _menuController;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _menuController = GetComponentInParent<MenuController>();
    }
    
    /// <summary>
    /// Opens the designated menu. Sets the current menu to be inactive
    /// and sets the initially selected button.
    /// </summary>
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
        
        if (_menuToOpen.DontResetSelectedOnClose && _previouslySelectedButton != null)
        {
            _previouslySelectedButton.Select();
        }
    }
    
    /// <summary>
    /// Closes the previously opened menu if it that menu does not currently have any child menus open.
    /// </summary>
    public void CloseMenu()
    {
        if (!_menuToOpen.Active) return;
        
        _menuToOpen.gameObject.SetActive(false);
        if (_menuController != null)
        {
            _menuController.SetMenuInteractable(true);
        }

        if (_menuToOpen.DontResetSelectedOnClose)
        {
            _previouslySelectedButton = _menuToOpen.SelectedButton;
        }
        
        _button.Select();
    }
}
