using UnityEngine;
using UnityEngine.Events;

public class EvidenceController : MonoBehaviour, IEvidenceController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("This event is called when the evidence menu is opened.")]
    [SerializeField] private UnityEvent _onEvidenceMenuOpened;

    [Tooltip("This EvidenceDictionary should contain all the evidence available in this scene.")]
    [SerializeField] private EvidenceDictionary _masterEvidenceDictionary;

    [Tooltip("EvidenceDictionary here that contains evidence that the player should have access to at the start of the scene")]
    [SerializeField] private EvidenceDictionary _currentEvidenceDictionary;

    public EvidenceDictionary CurrentEvidenceDictionary => _currentEvidenceDictionary;
    
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
    }

    /// <summary>
    /// Adds a piece of evidence to the evidence menu. Gets an Evidence object
    /// from _masterEvidenceDictionary and adds it to _currentEvidenceDictionary
    /// </summary>
    /// <param name="evidence">The name of the evidence to add.</param>
    public void AddEvidence(string evidence)
    {
        if (_masterEvidenceDictionary == null)
        {
            Debug.LogError("Cannot add evidence. Evidence dictionary has not been assigned.");
        }
        
        _currentEvidenceDictionary.AddEvidence(_masterEvidenceDictionary.GetEvidence(evidence));
    }

    /// <summary>
    /// Removes a piece of evidence from the evidence menu.
    /// </summary>
    /// <param name="evidence">The name of the evidence to remove.</param>
    public void RemoveEvidence(string evidence)
    {
        if (_masterEvidenceDictionary == null)
        {
            Debug.LogError("Cannot remove evidence. Evidence dictionary has not been assigned.");
        }
        
        _currentEvidenceDictionary.RemoveEvidence(evidence);
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
    }
}
