using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Evidence Dictionary", menuName = "Evidence/Evidence Dictionary")]
public class EvidenceDictionary : ScriptableObject
{
    [SerializeField, Tooltip("Add all the evidence required for evidence dictionary here.")]
    private List<Evidence> _evidenceList;

    private Dictionary<string, Evidence> _evidenceDictionary;

    public int Count => _evidenceDictionary.Count;

    /// <summary>
    /// Converts the list of evidence objects into a dictionary so they can be accessed by name.
    /// </summary>
    private void OnEnable()
    {
        if (_evidenceList != null && (_evidenceDictionary == null || _evidenceDictionary.Count != _evidenceList.Count))
        {
            _evidenceDictionary = new Dictionary<string, Evidence>();

            foreach (var evidence in _evidenceList)
            {
                _evidenceDictionary.Add(evidence.name, evidence);
            }
        }
    }

    /// <summary>
    /// Replaces an Evidence object with its designated alternate evidence.
    /// </summary>
    /// <param name="evidence">The name of the evidence to substitute.</param>
    public void SubstituteEvidenceWithAlt(string evidence)
    {
        if (!_evidenceDictionary.ContainsKey(evidence))
        {
            Debug.LogError($"Evidence {evidence} was not found in the dictionary.");
            return;
        }
        
        if (_evidenceDictionary[evidence].AltEvidence != null)
        {
            _evidenceDictionary[evidence] = _evidenceDictionary[evidence].AltEvidence;
        }
    }

    /// <summary>
    /// Method to add evidence to the evidence dictionary.
    /// Does not add to evidence list so that evidence dictionary can be reset on restart.
    /// </summary>
    /// <param name="evidence">The evidence object to add.</param>
    public void AddEvidence(Evidence evidence)
    {
        if (_evidenceDictionary.ContainsValue(evidence))
        {
            Debug.LogError($"Evidence {evidence.name} has already been added to the dictionary.");
            return;
        }
        
        _evidenceDictionary.Add(evidence.name, evidence);
    }

    /// <summary>
    /// Method to remove evidence from the evidence dictionary
    /// Does not remove from evidence list so that evidence dictionary can be reset on restart.
    /// </summary>
    /// <param name="evidence">The name of the evidence to remove.</param>
    public void RemoveEvidence(string evidence)
    {
        if (!_evidenceDictionary.ContainsKey(evidence))
        {
            Debug.LogError($"Evidence {evidence} was not found in the dictionary.");
            return;
        }
        
        _evidenceDictionary.Remove(evidence);
    }

    /// <summary>
    /// Allows the dictionary to be accessed by index instead of evidence name.
    /// </summary>
    /// <param name="index">The index of the evidence to get.</param>
    /// <returns>The evidence at the specified index.</returns>
    public Evidence GetEvidenceAtIndex(int index)
    {
        return _evidenceDictionary.Values.ElementAt(index);
    }
}
