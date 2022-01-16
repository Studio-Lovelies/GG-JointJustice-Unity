using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Menu))]
public class ChoiceMenu : MonoBehaviour
{
    [Tooltip("Drag the prefab for choice menu items here.")]
    [SerializeField] private MenuItem _choiceMenuItem;

    [Tooltip("Drag the Menu component here.")]
    [SerializeField] private Menu _menu;
    
    [Tooltip("Drag a menu opener component here.")]
    [SerializeField] private MenuOpener _menuOpener;

    [Tooltip("This event is called when an choice is clicked.")]
    [SerializeField] private UnityEvent<int> _onChoiceClicked;

    /// <summary>
    /// Creates a choice menu using a choice list.
    /// Opens the menu, instantiates the correct number of buttons and
    /// assigns their text and onClick events.
    /// </summary>
    /// <param name="choiceList">The list of choices in the choice menu.</param>
    public void InitialiseChoiceMenu(List<Choice> choiceList)
    {
        if (gameObject.activeInHierarchy)
        {
            return; // Don't make another menu if its already active
        }
        
        if (!HasRequiredComponents())
        {
            return;
        }
        
        _menuOpener.OpenMenu();
        
        if (_choiceMenuItem == null)
        {
            Debug.LogError("Could not create choice menu. Choice menu item prefab has not been assigned.", gameObject);
        }
        
        foreach (var choice in choiceList)
        {
            MenuItem menuItem = Instantiate(_choiceMenuItem, transform);
            if (choice.index == 0)
            {
                _menu.SelectInitialButton();
            }
            menuItem.Text = choice.text;
            ((Button)menuItem.Selectable).onClick.AddListener(() => _onChoiceClicked.Invoke(choice.index));
        }
    }

    /// <summary>
    /// Call this to deactivate the choice menu after clicking a choice.
    /// </summary>
    public void DeactivateChoiceMenu()
    {
        _menuOpener.CloseMenu();
        
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Check if all required components have been assigned.
    /// </summary>
    /// <returns>Whether the required components have been assigned (true) or not (false).</returns>
    private bool HasRequiredComponents()
    {
        bool hasRequiredComponents = true;
        
        if (_menu == null)
        {
            PrintMissingComponent("Menu");
            hasRequiredComponents = false;
        }
        
        if (_menuOpener == null)
        {
            PrintMissingComponent("MenuOpener");
            hasRequiredComponents = false;
        }

        return hasRequiredComponents;
    }

    /// <summary>
    /// Method used by HasRequiredComponents method to print an appropriates error message.
    /// </summary>
    /// <param name="componentName">The name of the missing component.</param>
    private void PrintMissingComponent(string componentName)
    {
        Debug.LogError($"{componentName} has not been assigned to component {this} on {gameObject.name}.", this);
    }
}
