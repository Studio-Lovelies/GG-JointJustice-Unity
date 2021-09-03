using UnityEngine;
using UnityEngine.Events;

public class EvidenceController : MonoBehaviour, IEvidenceController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("This event is called when the evidence menu is opened.")]
    [SerializeField] private UnityEvent _onEvidenceMenuOpened;

    [Tooltip("This event will be called when a menu item is added, removed, or modified")]
    [SerializeField] private UnityEvent<EvidenceDictionary> _onEvidenceModified;

    [Tooltip("This EvidenceDictionary should contain all the evidence available in this scene.")]
    [SerializeField] private EvidenceDictionary _masterEvidenceDictionary;

    [Tooltip("EvidenceDictionary here that contains evidence that the player should have access to at the start of the scene")]
    [SerializeField] private EvidenceDictionary _currentEvidenceDictionary;
    
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

        if (_currentEvidenceDictionary == null)
        {
            _currentEvidenceDictionary = ScriptableObject.CreateInstance<EvidenceDictionary>();
        }
        
        _onEvidenceModified.Invoke(_currentEvidenceDictionary);
    }

    public void AddEvidence(string evidence)
    {
        _currentEvidenceDictionary.AddEvidence(_masterEvidenceDictionary.GetEvidence(evidence));
        _onEvidenceModified.Invoke(_currentEvidenceDictionary);
    }

    public void RemoveEvidence(string evidence)
    {
        _currentEvidenceDictionary.RemoveEvidence(evidence);
        _onEvidenceModified.Invoke(_currentEvidenceDictionary);
    }

    public void AddToCourtRecord(string actor)
    {
        Debug.LogWarning("AddToCourtRecord not implemented");
    }

    /// <summary>
    /// Method called by DirectorActionDecoder to open the evidence menu.
    /// Calls an event which should open (and disable closing of) the evidence menu.
    /// </summary>
    public void OpenEvidenceMenu()
    {
        _onEvidenceMenuOpened.Invoke();
    }

    /// <summary>
    /// Substitutes a piece of evidence with its assigned alternate evidence.
    /// </summary>
    /// <param name="evidence"></param>
    public void SubstituteEvidence(string evidence)
    {
        _currentEvidenceDictionary.SubstituteEvidenceWithAlt(evidence);
        _onEvidenceModified.Invoke(_currentEvidenceDictionary);
    }
}
