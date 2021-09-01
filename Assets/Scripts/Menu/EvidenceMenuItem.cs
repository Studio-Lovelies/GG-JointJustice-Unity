using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(MenuItem))]
public class EvidenceMenuItem : MonoBehaviour, ISelectHandler
{
    [SerializeField, Tooltip("Drag the Image component used to display the evidence's icon here")]
    private Image _image;

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
    
    public EvidenceMenu EvidenceMenu { get; set; } // Set by evidence menu on instantiation

    public void OnSelect(BaseEventData eventData)
    {
        EvidenceMenu.UpdateEvidenceInfo(_evidence);
    }
}
