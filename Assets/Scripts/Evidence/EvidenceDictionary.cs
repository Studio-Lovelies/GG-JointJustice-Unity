using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvidenceDictionary : MonoBehaviour, IEvidenceDictionary
{
    [SerializeField, Tooltip("Drag an EvidenceList here containing all the evidence available in the scene")]
    private EvidenceList _masterEvidenceList;

    [SerializeField, Tooltip("Add any evidence that the player should have at the start of the scene here.")]
    private Evidence[] _currentEvidenceList;

    private Dictionary<string, Evidence> _masterEvidenceDictionary;
    private Dictionary<string, Evidence> _currentEvidenceDictionary;

    public int Count => _currentEvidenceDictionary.Count;
    
    /// <summary>
    /// Converts the evidence list into a dictionary on awake
    /// </summary>
    private void Awake()
    {
        if (_masterEvidenceList != null)
        {
            _masterEvidenceDictionary = ArrayToEvidenceDictionary(_masterEvidenceList.EvidenceArray);
        }
        else
        {
            Debug.LogError("Master evidence list has not been assigned to EvidenceDictionary.");
        }
        
        _currentEvidenceDictionary = ArrayToEvidenceDictionary(_currentEvidenceList);
    }

    /// <summary>
    /// Converts a list of evidence into a dictionary of evidence
    /// so evidence an be accessed by name
    /// </summary>
    /// <param name="evidenceList">The list to convert to a dictionary.</param>
    /// <returns>The dictionary that the list has been converted to.</returns>
    private Dictionary<string, Evidence> ArrayToEvidenceDictionary(Evidence[] evidenceList)
    {
        var evidenceDictionary = new Dictionary<string, Evidence>();
        foreach (var evidence in evidenceList)
        {
            Debug.Log(evidence.name);
            evidenceDictionary.Add(evidence.name, evidence);
        }
        return evidenceDictionary;
    }

    /// <summary>
    /// Call this when method you want to add evidence to the dictionary.
    /// </summary>
    /// <param name="evidenceName">The evidence to add.</param>
    public void AddEvidence(string evidenceName)
    {
        if (!IsEvidenceInDictionary(evidenceName, _masterEvidenceDictionary))
        {
            return;
        }

        Evidence evidence = _masterEvidenceDictionary[evidenceName];
        _currentEvidenceDictionary.Add(evidence.name, evidence);
    }

    /// <summary>
    /// Call this method when you want to remove evidence from the dictionary.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to remove.</param>
    public void RemoveEvidence(string evidenceName)
    {
        if (!IsEvidenceInDictionary(evidenceName, _currentEvidenceDictionary))
        {
            return;
        }

        _currentEvidenceDictionary.Remove(evidenceName);
    }

    /// <summary>
    /// Call this method when you want to get evidence from the dictionary using the evidence's name.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to get.</param>
    /// <returns></returns>
    public Evidence GetEvidence(string evidenceName)
    {
        if (!IsEvidenceInDictionary(evidenceName, _currentEvidenceDictionary))
        {
            return null;
        }

        return _currentEvidenceDictionary[evidenceName];
    }
    
    /// <summary>
    /// Replaces an Evidence object with its designated alternate evidence.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to substitute.</param>
    public void SubstituteEvidenceWithAlt(string evidenceName)
    {
        if (!IsEvidenceInDictionary(evidenceName, _currentEvidenceDictionary))
        {
            return;
        }

        if (_currentEvidenceDictionary[evidenceName].AltEvidence == null)
        {
            Debug.LogError($"Evidence {evidenceName} does not have an alternate Evidence object assigned");
            return;
        }
        
        _currentEvidenceDictionary[evidenceName] = _currentEvidenceDictionary[evidenceName].AltEvidence;
    }

    /// <summary>
    /// Method used by GetEvidence and AddEvidence methods to check
    /// if evidence is in the dictionary before attempting to access.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to look for in the dictionary.</param>
    /// <param name="dictionaryToCheck">The dictionary to look for the evidence object in.</param>
    /// <returns>True if the evidence is in the dictionary, and false if it is not.</returns>
    private bool IsEvidenceInDictionary(string evidenceName, Dictionary<string, Evidence> dictionaryToCheck)
    {
        if (!dictionaryToCheck.ContainsKey(evidenceName))
        {
            Debug.LogError($"Evidence {evidenceName} could not be found in the evidence dictionary.");
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Allows the dictionary to be accessed by index instead of evidence name.
    /// </summary>
    /// <param name="index">The index of the evidence to get.</param>
    /// <returns>The evidence at the specified index.</returns>
    public Evidence GetEvidenceAtIndex(int index)
    {
        try
        {
            return _currentEvidenceDictionary.Values.ElementAt(index);
        }
        catch (IndexOutOfRangeException exception)
        {
            Debug.LogError($"{exception.GetType().Name}: Index {index} out of range of evidence dictionary.");
            return null;
        }
    }
}
