using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvidenceController : MonoBehaviour, IEvidenceController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("This event is called when the PRESENT_EVIDENCE action is called.")]
    [SerializeField] private UnityEvent _onPresentEvidence;

    [Tooltip("Drag an EvidenceDictionary component here.")]
    [SerializeField] private EvidenceDictionary _evidenceDictionary;

    [Tooltip("Drag en EvidenceMenu component here.")]
    [SerializeField] private EvidenceMenu _evidenceMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Evidence Controller doesn't have an action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.SetEvidenceController(this);
        }

        if (_evidenceDictionary == null)
        {
            Debug.LogError("EvidenceDictionary has not been assigned to evidence controller.");
        }

        if (_evidenceMenu == null)
        {
            Debug.LogError("EvidenceMenu has not been assigned to evidence controller.");
        }

        if (_evidenceDictionary != null && _evidenceMenu != null)
        {
            _evidenceMenu.EvidenceDictionary = _evidenceDictionary;
        }
    }

    /// <summary>
    /// Adds a piece of evidence to the evidence menu. Gets an Evidence object
    /// from _masterEvidenceDictionary and adds it to _currentEvidenceDictionary
    /// </summary>
    /// <param name="evidence">The name of the evidence to add.</param>
    public void AddEvidence(string evidence)
    {
        if (!HasEvidenceDictionary())
        {
            return;
        }
        
        _evidenceDictionary.AddEvidence(evidence);
    }

    /// <summary>
    /// Removes a piece of evidence from the evidence menu.
    /// </summary>
    /// <param name="evidence">The name of the evidence to remove.</param>
    public void RemoveEvidence(string evidence)
    {
        if (!HasEvidenceDictionary())
        {
            return;
        }
            
        _evidenceDictionary.RemoveEvidence(evidence);
    }

    public void AddToCourtRecord(string actor)
    {
        Debug.LogWarning("AddToCourtRecord not implemented");
    }

    /// <summary>
    /// Method called by DirectorActionDecoder to open the evidence menu to allow presenting of evidence.
    /// Calls an event which should open (and disable closing of) the evidence menu.
    /// </summary>
    public void PresentEvidence()
    {
        _onPresentEvidence.Invoke();
    }

    /// <summary>
    /// Substitutes a piece of evidence with its assigned alternate evidence.
    /// </summary>
    /// <param name="evidence"></param>
    public void SubstituteEvidence(string evidence)
    {
        if (!HasEvidenceDictionary())
        {
            return;
        }
        
        _evidenceDictionary.SubstituteEvidenceWithAlt(evidence);
    }

    /// <summary>
    /// Checks if an evidence dictionary has been assigned.
    /// Should be called before accessing the evidence dictionary.
    /// </summary>
    /// <returns></returns>
    public bool HasEvidenceDictionary()
    {
        if (_evidenceDictionary == null)
        {
            Debug.LogError("EvidenceDictionary has not been assigned to evidence controller.");
            return false;
        }

        return true;
    }
}
