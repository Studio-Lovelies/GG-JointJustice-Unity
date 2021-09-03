using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Menu))]
public class EvidenceMenu : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the TextMeshProUGUI component used for displaying the evidence's name here")]
    private TextMeshProUGUI _evidenceName;
    
    [SerializeField, Tooltip("Drag the TextMeshProUGUI component used for displaying the evidence's description here.")]
    private TextMeshProUGUI _evidenceDescription;
    
    [SerializeField, Tooltip("Drag the Image component used for displaying the evidence's icon here.")]
    private Image _evidenceIcon;

    [SerializeField, Tooltip("The boxes used to represent menu items.")]
    private EvidenceMenuItem[] _evidenceMenuItems;

    [SerializeField, Tooltip("Drag an EvidenceDictionary here if the menu should have menu items on Awake")]
    private EvidenceDictionary _evidenceDictionary;

    [SerializeField, Tooltip("This event is called when a piece of evidence has been clicked.")]
    private UnityEvent<Evidence> _onEvidenceClicked;
    
    private int _currentPage;
    private int _numberOfPages;

    #if UNITY_EDITOR
    // TODO remove. Debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            _evidenceDictionary.SubstituteEvidenceWithAlt("Bent Coins");
            UpdateEvidenceMenu(_currentPage * _evidenceMenuItems.Length);
        }
    }
    #endif
    
    /// <summary>
    /// Awake method is used to initialise the menu if an evidence dictionary has been assigned.
    /// </summary>
    private void Awake()
    {
        if (_evidenceDictionary != null)
        {
            _numberOfPages = Mathf.CeilToInt((float)_evidenceDictionary.Dictionary.Count / _evidenceMenuItems.Length);
            UpdateEvidenceDictionary(_evidenceDictionary);
            UpdateEvidenceMenu(0);
        }
    }
    
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
    /// Call this to update the evidence dictionary used by this evidence menu.
    /// </summary>
    /// <param name="evidenceDictionary">The evidence dictionary that should be assigned to this menu.</param>
    public void UpdateEvidenceDictionary(EvidenceDictionary evidenceDictionary)
    {
        _evidenceDictionary = evidenceDictionary;
        _numberOfPages = Mathf.CeilToInt((float)evidenceDictionary.Dictionary.Count / _evidenceMenuItems.Length);
    }
    
    /// <summary>
    /// Updates the currently displayed evidence.
    /// </summary>
    /// <param name="startIndex">The index of the evidence list that will appear first.</param>
    private void UpdateEvidenceMenu(int startIndex)
    {
        for (int i = 0; i < _evidenceMenuItems.Length; i++)
        {
            if (i + startIndex > _evidenceDictionary.Dictionary.Count - 1)
            {
                _evidenceMenuItems[i].gameObject.SetActive(false);
            }
            else
            {
                _evidenceMenuItems[i].gameObject.SetActive(true);
                _evidenceMenuItems[i].Evidence = _evidenceDictionary.Dictionary.Values.ElementAt(i + startIndex);
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
        UpdateEvidenceMenu(_currentPage * _evidenceMenuItems.Length);
    }

    /// <summary>
    /// Decrements the evidence page.
    /// Calls UpdateEvidenceMenu to display the page.
    /// </summary>
    public void DecrementPage()
    {
        _currentPage += _numberOfPages - 1;
        _currentPage %= _numberOfPages;
        UpdateEvidenceMenu(_currentPage * _evidenceMenuItems.Length);
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
