using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Menu))]
public class EvidenceMenu : MonoBehaviour
{
    [Tooltip("Drag the DialogueController here")]
    [SerializeField] private DialogueController _dialogueController;
    
    [SerializeField, Tooltip("Drag the evidence controller here")]
    private EvidenceController _evidenceController;

    [SerializeField, Tooltip("Drag the TextMeshProUGUI component used for displaying the evidence's name here")]
    private TextMeshProUGUI _evidenceName;
    
    [SerializeField, Tooltip("Drag the TextMeshProUGUI component used for displaying the evidence's description here.")]
    private TextMeshProUGUI _evidenceDescription;
    
    [SerializeField, Tooltip("Drag the Image component used for displaying the evidence's icon here.")]
    private Image _evidenceIcon;

    [SerializeField, Tooltip("Drag the Image component for the menu label here.")]
    private Image _menuLabel;
    
    [SerializeField, Tooltip("Drag the sprite used for the profiles menu label.")]
    private Sprite _profilesMenuLabel;

    [SerializeField, Tooltip("The boxes used to represent menu items.")]
    private EvidenceMenuItem[] _evidenceMenuItems;
    
    [SerializeField, Tooltip("Drag all buttons used to navigate the menu here so they can be disabled when necessary.")]
    private Button[] _navigationButtons;

    [SerializeField, Tooltip("This event is called when a piece of evidence has been clicked.")]
    private UnityEvent _onEvidenceClicked;

    private bool _profileMenuActive;
    private Sprite _evidenceMenuLabel;
    private int _currentPage;
    private int _numberOfPages;
    private int _startIndex;
    private Menu _menu;

    // when set to false, this menu can only be toggled
    // when set to true, this menu can be closed by presenting evidence and thereby following a different path of the active ink script
    public bool CanPresentEvidence { private get; set; }

    /// <summary>
    /// Get the menu on awake to access its DontResetSelectedOnClose property
    /// </summary>
    private void Awake()
    {
        _menu = GetComponent<Menu>();
        _evidenceMenuLabel = _menuLabel.sprite;
    }
    
    /// <summary>
    /// Updates the evidence menu with the name, icon, and description
    /// of the evidence currently being selected.
    /// Called by EvidenceMenuItems when they are selected.
    /// </summary>
    /// <param name="evidence"></param>
    public void UpdateEvidenceInfo(ICourtRecordObject evidence)
    {
        _evidenceName.text = evidence.CourtRecordName;
        _evidenceDescription.text = evidence.Description;
        _evidenceIcon.sprite = evidence.Icon;
    }

    /// <summary>
    /// When this menu is enabled it should be updated with any new evidence added.
    /// </summary>
    private void OnEnable()
    {
        _menuLabel.sprite = _evidenceMenuLabel;
        _profileMenuActive = false;

        if (!_menu.DontResetSelectedOnClose)
        {
            _currentPage = 0;
        }
        
        UpdateEvidenceMenu();
    }

    /// <summary>
    /// Updates the currently displayed evidence by looping through the menu item boxes
    /// and assigning the corresponding Evidence object in the dictionary to them.
    /// </summary>
    public void UpdateEvidenceMenu()
    {
        var objects = _profileMenuActive
            ? _evidenceController.CurrentProfiles.Cast<ICourtRecordObject>().ToArray()
            : _evidenceController.CurrentEvidence.Cast<ICourtRecordObject>().ToArray();
        

        CalculatePages(objects.Length);
        SetNavigationButtonsActive();
        DrawMenuItems(objects);
    }

    /// <summary>
    /// Calculates the number of pages, the current pages, and the starting index
    /// to start getting objects from the object dictionary.
    /// </summary>
    private void CalculatePages(float objectCount)
    {
        _numberOfPages = Mathf.CeilToInt(objectCount / _evidenceMenuItems.Length);
        _currentPage = Mathf.Clamp(_currentPage, 0,_numberOfPages == 0 ? 0 : _numberOfPages - 1); // Max value must always be positive 
        _startIndex = _currentPage * _evidenceMenuItems.Length;
    }

    /// <summary>
    /// Loops through the navigation buttons and disables them if
    /// there is less than one page.
    /// </summary>
    private void SetNavigationButtonsActive()
    {
        foreach (var button in _navigationButtons)
        {
            button.interactable = _numberOfPages > 1; // Navigation buttons not needed if less than 2 pages
        }
    }
    
    /// <summary>
    /// Loops through each menu item and gives it an ICourtRecordObject
    /// or disables it if there is no ICourtRecordObject to assign.
    /// </summary>
    private void DrawMenuItems(ICourtRecordObject[] objects)
    {
        for (int i = 0; i < _evidenceMenuItems.Length; i++)
        {
            if (i + _startIndex > objects.Length - 1)
            {
                _evidenceMenuItems[i].gameObject.SetActive(false);
            }
            else
            {
                _evidenceMenuItems[i].gameObject.SetActive(true);
                _evidenceMenuItems[i].CourtRecordObject = objects[_startIndex + i];
            }
        }
    }

    /// <summary>
    /// Increments the evidence page.
    /// Calls UpdateEvidenceMenu to display the page.
    /// </summary>
    public void IncrementPage()
    {
        if (!CanChangePage())
        {
            return;
        }
        
        _currentPage++;
        _currentPage %= _numberOfPages;
        UpdateEvidenceMenu();
    }

    /// <summary>
    /// Decrements the evidence page.
    /// Calls UpdateEvidenceMenu to display the page.
    /// </summary>
    public void DecrementPage()
    {
        if (!CanChangePage())
        {
            return;
        }
        
        _currentPage += _numberOfPages - 1;
        _currentPage %= _numberOfPages;
        UpdateEvidenceMenu();
    }

    /// <summary>
    /// Method to check if the page of the evidence menu can be changed.
    /// Should be changeable if there 2 or more pages.
    /// </summary>
    /// <returns></returns>
    private bool CanChangePage()
    {
        if (_numberOfPages <= 1)
        {
            Debug.LogError("Could not change page because there is one or fewer pages.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Called by EvidenceMenuItems when they are clicked.
    /// Used to call a method on the evidence controller so it can call an event.
    /// Also used to call en event to close this menu.
    /// </summary>
    /// <param name="evidence">The Evidence object that has been clicked.</param>
    public void OnEvidenceClicked(ICourtRecordObject evidence)
    {
        if (!CanPresentEvidence)
        {
            return;
        }
        _onEvidenceClicked.Invoke();
        _evidenceController.OnPresentEvidence(evidence);
    }

    /// <summary>
    /// Switches between the evidence menu and the profiles menu.
    /// Subscribe this to an input event to toggle the menus in game.
    /// </summary>
    public void ToggleProfileMenu()
    {
        if (!isActiveAndEnabled) return;

        if (_profileMenuActive)
        {
            _profileMenuActive = false;
            _menuLabel.sprite = _evidenceMenuLabel;
        }
        else
        {
            _profileMenuActive = true;
            _menuLabel.sprite = _profilesMenuLabel;
        }
        
        UpdateEvidenceMenu();
        _menu.SelectInitialButton();
    }
}
