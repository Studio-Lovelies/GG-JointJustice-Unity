using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Evidence Dictionary", menuName = "Evidence/Evidence Dictionary")]
public class EvidenceDictionary : ScriptableObject
{
    [SerializeField, Tooltip("Add all the evidence required for this list here.")]
    private List<Evidence> _evidenceList;
    
    public Dictionary<string, Evidence> Dictionary { get; private set; }

    /// <summary>
    /// Converts the list of evidence objects into a dictionary so they can be accessed by name.
    /// </summary>
    private void OnEnable()
    {
        if (_evidenceList != null && (Dictionary == null || Dictionary.Count != _evidenceList.Count))
        {
            Dictionary = new Dictionary<string, Evidence>();

            foreach (var evidence in _evidenceList)
            {
                Dictionary.Add(evidence.name, evidence);
            }
        }
    }

    /// <summary>
    /// Replaces an Evidence object with its designated alternate evidence.
    /// </summary>
    /// <param name="evidence">The name of the evidence to substitute.</param>
    public void SubstituteEvidenceWithAlt(string evidence)
    {
        if (!Dictionary.ContainsKey(evidence))
        {
            Debug.LogError($"Evidence {evidence} was not found in the dictionary.");
            return;
        }
        
        if (Dictionary[evidence].AltEvidence != null)
        {
            Dictionary[evidence] = Dictionary[evidence].AltEvidence;
        }
    }
}
