using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Menu))]
public class EvidenceMenu : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the TextMeshProUGUI component used for displaying the evidence's name here")]
    private TextMeshProUGUI _evidenceName;
    
    [SerializeField, Tooltip("Drag the TextMeshProUGUI component used for displaying the evidence's description here.")]
    private TextMeshProUGUI _evidenceDescription;
    
    [SerializeField, Tooltip("Drag the Image component used for displaying the evidence's icon here.")]
    private Image _evidenceIcon;

    [SerializeField, Tooltip("Drag the prefab to use for each menu item here.")]
    private EvidenceMenuItem _menuItemPrefab;

    [SerializeField, Tooltip("The boxes used to represent menu items.")]
    private EvidenceMenuItem[] _evidenceMenuItems;

    [SerializeField, Tooltip("Drag an EvidenceDictionary here if the menu should have menu items on Awake")]
    private EvidenceDictionary _evidenceDictionary;

    [SerializeField, Tooltip("This event is called when a piece of evidence has been clicked.")]
    private UnityEvent<Evidence> _onEvidenceClicked;

    private List<EvidenceMenuItem> _menuItems = new List<EvidenceMenuItem>();
    private int currentPage;
    private int numberOfPages;
    
    private void Awake()
    {
        if (_evidenceDictionary != null)
        {
            UpdateEvidenceMenu(_evidenceDictionary, 0);
        }
    }
    
    public void UpdateEvidenceInfo(Evidence evidence)
    {
        _evidenceName.text = evidence.DisplayName;
        _evidenceDescription.text = evidence.Description;
        _evidenceIcon.sprite = evidence.Icon;
    }

    private void UpdateEvidenceMenu(EvidenceDictionary evidenceDictionary, int startIndex)
    {
        numberOfPages = Mathf.CeilToInt((float)evidenceDictionary.List.Count / _evidenceMenuItems.Length);
        
        for (int i = 0; i < _evidenceMenuItems.Length; i++)
        {
            if (i + startIndex > evidenceDictionary.List.Count - 1)
            {
                _evidenceMenuItems[i].gameObject.SetActive(false);
            }
            else
            {
                _evidenceMenuItems[i].gameObject.SetActive(true);
                _evidenceMenuItems[i].Evidence = evidenceDictionary.List[i + startIndex];
            }
        }
    }

    public void IncrementPage()
    {
        currentPage++;
        currentPage %= numberOfPages;
        UpdateEvidenceMenu(_evidenceDictionary, currentPage * _evidenceMenuItems.Length);
    }

    public void DecrementPage()
    {
        currentPage += numberOfPages - 1;
        currentPage %= numberOfPages;
        UpdateEvidenceMenu(_evidenceDictionary, currentPage * _evidenceMenuItems.Length);
    }

    public void OnEvidenceClicked(Evidence evidence)
    {
        Debug.Log(evidence); // TODO Remove
        _onEvidenceClicked.Invoke(evidence);
    }
}
