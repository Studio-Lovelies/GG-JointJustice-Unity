using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [SerializeField, Tooltip("The transform used to instantiate menu items in.")]
    private Transform _evidenceContainer;

    [SerializeField, Tooltip("Drag an EvidenceDictionary here if the menu should have menu items on Awake")]
    private EvidenceDictionary _evidenceDictionary;
    
    private List<EvidenceMenuItem> _menuItems;
    
    public void UpdateEvidenceInfo(Evidence evidence)
    {
        _evidenceName.text = evidence.name;
        _evidenceDescription.text = evidence.Description;
        _evidenceIcon.sprite = evidence.Icon;
    }

    private void UpdateEvidence(List<Evidence> evidenceList)
    {
        foreach (var evidence in evidenceList)
        {
            EvidenceMenuItem evidenceMenuItem = Instantiate(_menuItemPrefab, _evidenceContainer);
            evidenceMenuItem.EvidenceMenu = this;
            evidenceMenuItem.Evidence = evidence;
            _menuItems.Add(evidenceMenuItem);
        }
    }
}
