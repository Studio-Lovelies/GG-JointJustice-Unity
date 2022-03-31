using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EvidenceController : MonoBehaviour, IEvidenceController
{
    [SerializeField] private NarrativeGameState _narrativeGameState;
    
    [Tooltip("This event is called when the PRESENT_EVIDENCE action is called.")]
    [SerializeField] private UnityEvent _onRequirePresentEvidence;
    
    public List<EvidenceData> CurrentEvidence { get; } = new List<EvidenceData>();
    public List<ActorData> CurrentProfiles { get; } = new List<ActorData>();

    /// <summary>
    /// Adds a piece of evidence to the evidence menu.
    /// </summary>
    /// <param name="evidenceData">evidence to add.</param>
    public void AddEvidence(EvidenceData evidenceData)
    {
        CurrentEvidence.Add(evidenceData);
    }

    /// <summary>
    /// Removes a piece of evidence from the evidence menu.
    /// </summary>
    /// <param name="evidenceData">evidence to remove</param>
    public void RemoveEvidence(EvidenceData evidenceData)
    {
        CurrentEvidence.Remove(evidenceData);
    }

    /// <summary>
    /// Adds an actor to the court record.
    /// </summary>
    /// <param name="actorData">The actor to add.</param>
    public void AddRecord(ActorData actorData)
    {
        CurrentProfiles.Add(actorData);
    }

    /// <summary>
    /// Method called by DirectorActionDecoder to open the evidence menu and require the user to present a piece of evidence.
    /// Calls an event which should open (and disable closing of) the evidence menu.
    /// </summary>
    public void RequirePresentEvidence()
    {
        _onRequirePresentEvidence.Invoke();
    }

    /// <summary>
    /// Substitutes a piece of evidence with another piece of evidence.
    /// </summary>
    /// <param name="initialEvidenceData">The evidenceData to be substituted</param>
    /// <param name="substituteEvidenceData">The evidenceData to substitute <see cref="initialEvidenceData"/> with</param>
    public void SubstituteEvidence(EvidenceData initialEvidenceData, EvidenceData substituteEvidenceData)
    {
        CurrentEvidence[CurrentEvidence.IndexOf(initialEvidenceData)] = substituteEvidenceData;
    }
}
