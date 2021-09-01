using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

[RequireComponent(typeof(MenuItem))]
public class EvidenceMenuItem : MonoBehaviour
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

    public void UpdateEvidenceMenuInfo()
    {
        EvidenceMenu.UpdateEvidenceInfo(_evidence);
    }
}
