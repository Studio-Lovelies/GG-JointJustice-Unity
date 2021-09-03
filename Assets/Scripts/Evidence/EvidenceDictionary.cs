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
                AddEvidence(evidence);
            }
        }
    }

    /// <summary>
    /// Replaces an Evidence object with its designated alternate evidence.
    /// </summary>
    /// <param name="evidence">The name of the evidence to substitute.</param>
    public void SubstituteEvidenceWithAlt(string evidence)
    {
        if (!CheckIfEvidenceInDictionary(evidence))
        {
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
        if (evidence == null)
        {
            Debug.LogError($"Attempted to add a null evidence objection to evidence dictionary {name}");
        }
        
        if (_evidenceDictionary.ContainsValue(evidence))
        {
            Debug.LogError($"Evidence {evidence.name} has already been added to the dictionary.");
            return;
        }
        
        _evidenceDictionary.Add(evidence.name, evidence);
    }

    /// <summary>
    /// Method to retrieve evidence from the evidence dictionary.
    /// </summary>
    /// <param name="evidence">Name of the evidence to get.</param>
    /// <returns>The Evidence object retrieved.</returns>
    public Evidence GetEvidence(string evidence)
    {
        if (!CheckIfEvidenceInDictionary(evidence))
        {
            return null;
        }

        return _evidenceDictionary[evidence];
    }

    /// <summary>
    /// Method to remove evidence from the evidence dictionary
    /// Does not remove from evidence list so that evidence dictionary can be reset on restart.
    /// </summary>
    /// <param name="evidence">The name of the evidence to remove.</param>
    public void RemoveEvidence(string evidence)
    {
        if (!CheckIfEvidenceInDictionary(evidence))
        {
            return;
        }
        
        _evidenceDictionary.Remove(evidence);
    }
    
    /// <summary>
    /// Method used to check if a specified Evidence object is in _evidenceDictionary.
    /// </summary>
    /// <param name="evidence">Name of the evidence to check.</param>
    /// <returns>Whether the evidence was in the dictionary (true) or not (false)</returns>
    private bool CheckIfEvidenceInDictionary(string evidence)
    {
        if (_evidenceDictionary.ContainsKey(evidence))
        {
            return true;
        }

        Debug.LogError($"Evidence {evidence} could not be found in the dictionary.");
        return false;
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

    /// <summary>
    /// Create a new list and dictionary when an instance is created
    /// </summary>
    private void Awake()
    {
        if (_evidenceList == null)
        {
            _evidenceList = new List<Evidence>();
        }

        if (_evidenceDictionary == null)
        {
            _evidenceDictionary = new Dictionary<string, Evidence>();
        }
    }
}
