using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Menu))]
public class EvidenceMenu : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the evidence controller here")]
    private EvidenceController _evidenceController;
    
    [SerializeField, Tooltip("Drag the TextMeshProUGUI component used for displaying the evidence's name here")]
    private TextMeshProUGUI _evidenceName;
    
    [SerializeField, Tooltip("Drag the TextMeshProUGUI component used for displaying the evidence's description here.")]
    private TextMeshProUGUI _evidenceDescription;
    
    [SerializeField, Tooltip("Drag the Image component used for displaying the evidence's icon here.")]
    private Image _evidenceIcon;

    [SerializeField, Tooltip("The boxes used to represent menu items.")]
    private EvidenceMenuItem[] _evidenceMenuItems;
    
    [SerializeField, Tooltip("Drag all buttons used to navigate the menu here so they can be disabled when necessary.")]
    private Button[] _navigationButtons;

    [SerializeField, Tooltip("This event is called when a piece of evidence has been clicked.")]
    private UnityEvent<Evidence> _onEvidenceClicked;

    private EvidenceDictionary EvidenceDictionary => _evidenceController.CurrentEvidenceDictionary;
    private int _currentPage;
    private int _numberOfPages;
    private int _startIndex;

    /// <summary>
    /// Updates the evidence menu with the name, icon, and description
    /// of the evidence currently being selected.
    /// Called by EvidenceMenuItems when they are selected.
    /// </summary>
    /// <param name="evidence"></param>
    public void UpdateEvidenceInfo(Evidence evidence)
    {
        _evidenceName.text = evidence.DisplayName;
        _evidenceDescription.text = evidence.Description;
        _evidenceIcon.sprite = evidence.Icon;
    }

    /// <summary>
    /// When this menu is enabled it should update be updated with any new evidence added.
    /// </summary>
    private void OnEnable()
    {
        UpdateEvidenceMenu();
    }

    /// <summary>
    /// Updates the currently displayed evidence by looping through the menu item boxes
    /// and assigning the corresponding Evidence object in the dictionary to them.
    /// </summary>
    public void UpdateEvidenceMenu()
    {
        _numberOfPages = Mathf.CeilToInt((float)EvidenceDictionary.Count / _evidenceMenuItems.Length);
        
        foreach (var button in _navigationButtons)
        {
            button.interactable = !(_numberOfPages < 2); // Navigation buttons not needed if less than 2 pages
        }

        for (int i = 0; i < _evidenceMenuItems.Length; i++)
        {
            if (i + _startIndex > EvidenceDictionary.Count - 1)
            {
                _evidenceMenuItems[i].gameObject.SetActive(false);
            }
            else
            {
                _evidenceMenuItems[i].gameObject.SetActive(true);
                _evidenceMenuItems[i].Evidence = EvidenceDictionary.GetEvidenceAtIndex(i + _startIndex);
            }
        }
    }

    /// <summary>
    /// Increments the evidence page.
    /// Calls UpdateEvidenceMenu to display the page.
    /// </summary>
    public void IncrementPage()
    {
        _currentPage++;
        _currentPage %= _numberOfPages;
        _startIndex = _currentPage * _evidenceMenuItems.Length;
        UpdateEvidenceMenu();
    }

    /// <summary>
    /// Decrements the evidence page.
    /// Calls UpdateEvidenceMenu to display the page.
    /// </summary>
    public void DecrementPage()
    {
        _currentPage += _numberOfPages - 1;
        _currentPage %= _numberOfPages;
        _startIndex = _currentPage * _evidenceMenuItems.Length;
        UpdateEvidenceMenu();
    }

    /// <summary>
    /// Called by EvidenceMenuItems when they are clicked.
    /// Calls an event and passes the selected Evidence object.
    /// </summary>
    /// <param name="evidence">The Evidence object that has been clicked.</param>
    public void OnEvidenceClicked(Evidence evidence)
    {
        _onEvidenceClicked.Invoke(evidence);
    }
}
