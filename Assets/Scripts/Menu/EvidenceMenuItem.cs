using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(MenuItem))]
public class EvidenceMenuItem : MonoBehaviour, ISelectHandler
{
    [SerializeField, Tooltip("Drag the Image component used to display the evidence's icon here")]
    private Image _image;

    [SerializeField, Tooltip("The evidence menu this evidence menu item is a child of")]
    private EvidenceMenu _evidenceMenu;
    private Evidence _evidence;

    public Evidence Evidence
    {
        get => _evidence;
        set
        {
            _evidence = value;
            _image.sprite = value.Icon;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        _evidenceMenu.UpdateEvidenceInfo(_evidence);
    }

    public void OnEvidenceClicked()
    {
        _evidenceMenu.OnEvidenceClicked(_evidence);
    }
}
