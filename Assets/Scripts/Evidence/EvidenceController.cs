using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EvidenceController : MonoBehaviour, IEvidenceController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("This event is called when the PRESENT_EVIDENCE action is called.")]
    [FormerlySerializedAs("_onOpenEvidenceMenu")]
    [SerializeField] private UnityEvent _onRequirePresentEvidence;

    [Tooltip("This event is called when a piece of evidence is clicked")]
    [SerializeField] private UnityEvent<ICourtRecordObject> _onPresentEvidence;
    
    [Tooltip("Drag an EvidenceInventory component here.")]
    [SerializeField] public EvidenceInventory _evidenceInventory;

    [Tooltip("Drag an ActorInventory component here.")]
    [SerializeField] public ActorInventory _actorInventory;

    [Tooltip("Drag an EvidenceMenu component here, which will updated when the game state (i.e. ability to present evidence) changes.")]
    [SerializeField] public EvidenceMenu _evidenceMenu;

    /// <summary>
    /// Called either when invoking <see cref="DialogueController._onCrossExaminationLoopActive" />
    /// or when a `PRESENT_EVIDENCE`-action has been encountered
    /// </summary>
    /// <param name="canPresentEvidence">Set to true, if evidence can be presented, set to false if presenting evidence is currently disabled</param>
    public void SetCanPresentEvidence(bool canPresentEvidence)
    {
        _evidenceMenu.CanPresentEvidence = canPresentEvidence;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Evidence Controller doesn't have an action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.Decoder.EvidenceController = this;
        }

        if (_evidenceInventory == null)
        {
            Debug.LogError("EvidenceDictionary has not been assigned to evidence controller.");
        }
    }

    /// <summary>
    /// Adds a piece of evidence to the evidence menu. Gets an Evidence object
    /// from _masterEvidenceDictionary and adds it to _currentEvidenceDictionary
    /// </summary>
    /// <param name="evidence">The name of the evidence to add.</param>
    public void AddEvidence(string evidence)
    {
        _evidenceInventory.AddObject(evidence);
    }

    /// <summary>
    /// Removes a piece of evidence from the evidence menu.
    /// </summary>
    /// <param name="evidence">The name of the evidence to remove.</param>
    public void RemoveEvidence(string evidence)
    {
        _evidenceInventory.RemoveObject(evidence);
    }

    /// <summary>
    /// Adds an actor to the court record.
    /// </summary>
    /// <param name="actorName">The name of the actor to add.</param>
    public void AddToCourtRecord(string actorName)
    {
        _actorInventory.AddObject(actorName);
    }

    /// <summary>
    /// Method called by DirectorActionDecoder to open the evidence menu and require the user to present a piece of evidence.
    /// Calls an event which should open (and disable closing of) the evidence menu.
    /// </summary>
    public void RequirePresentEvidence()
    {
        SetCanPresentEvidence(true);
        _onRequirePresentEvidence.Invoke();
    }

    /// <summary>
    /// Substitutes a piece of evidence with its assigned alternate evidence.
    /// </summary>
    /// <param name="evidence"></param>
    public void SubstituteEvidenceWithAlt(string evidence)
    {
        _evidenceInventory.SubstituteEvidenceWithAlt(evidence);
    }

    /// <summary>
    /// This method is called by the EvidenceMenu when evidence has been
    /// clicked and needs to be presented.
    /// </summary>
    /// <param name="evidence">The evidence to present.</param>
    public void OnPresentEvidence(ICourtRecordObject evidence)
    {
        _onPresentEvidence.Invoke(evidence);
    }
}
