using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Defines a menu. Used by MenuOpener to enable and disable the menu.
/// Keeps track of which menu item in the menu should be highlighted.
/// Chooses the initial menu item to be selected.
/// </summary>
public class Menu : MonoBehaviour
{
    [SerializeField, Tooltip("The first button that will be selected")]
    private Button _initiallyHighlightedButton;
    
    [field: SerializeField, Tooltip("Enable this if you want the selected button to be the same as when you closed the menu")]
    public bool DontResetSelectedOnClose { get; private set; }

    public UnityEvent<bool> OnSetInteractable { get; } = new UnityEvent<bool>();
    public Selectable SelectedButton { get; set; } // Set by child buttons when they are selected
    public bool Active => gameObject.activeInHierarchy && (SelectedButton == null || SelectedButton.interactable); // Returns true when no child menus are active ONLY if this menu is enabled

    private void Update()
    {
        // Stop button being deselected when clicking somewhere other than a button
        if (EventSystem.current.currentSelectedGameObject == null && SelectedButton != null && Active)
        {
            SelectedButton.Select();
        }
    }
    
    /// <summary>
    /// Enables and disables this section of menu.
    /// Used when opening sub menus.
    /// </summary>
    /// <param name="interactable">Whether the buttons should be interactable or not</param>
    public void SetMenuInteractable(bool interactable)
    {
        OnSetInteractable?.Invoke(interactable);
    }
    
    /// <summary>
    /// If set, selects an initial button other than the first one in the hierarchy.
    /// </summary>
    public void SelectInitialButton()
    {
        if (_initiallyHighlightedButton == null || !_initiallyHighlightedButton.isActiveAndEnabled)
        {
            Selectable selectable = GetComponentInChildren<Selectable>();
            if (selectable.interactable)
            {
                selectable.Select();
            }
            return;
        }
        
        _initiallyHighlightedButton.Select();
    }

    /// <summary>
    /// Called when the menu is disabled. Should be used to set DialogueController to not busy.
    /// </summary>
    private void OnDisable()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
