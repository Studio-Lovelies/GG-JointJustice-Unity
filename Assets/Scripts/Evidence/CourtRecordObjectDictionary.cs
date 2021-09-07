using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class CourtRecordObjectDictionary<T> where T : Object
{
    private Dictionary<string, T> _masterEvidenceDictionary;
    private Dictionary<string, T> _currentEvidenceDictionary;

    public int Count => _currentEvidenceDictionary.Count;

    public CourtRecordObjectDictionary(T[] masterEvidenceList, T[] currentEvidenceList)
    {
        if (masterEvidenceList != null)
        {
            _masterEvidenceDictionary = ArrayToEvidenceDictionary(masterEvidenceList);
        }
        else
        {
            Debug.LogError("Master evidence list has not been assigned to EvidenceDictionary.");
        }
        
        _currentEvidenceDictionary = ArrayToEvidenceDictionary(currentEvidenceList);
    }
    
    public CourtRecordObjectDictionary(Dictionary<string, T> masterEvidenceList, T[] currentEvidenceList)
    {
        if (masterEvidenceList == null)
        {
            Debug.LogError("Master evidence list has not been assigned to EvidenceDictionary.");
        }
        
        _currentEvidenceDictionary = ArrayToEvidenceDictionary(currentEvidenceList);
    }
    
    /// <summary>
    /// Converts a list of evidence into a dictionary of evidence
    /// so evidence an be accessed by name
    /// </summary>
    /// <param name="evidenceList">The list to convert to a dictionary.</param>
    /// <returns>The dictionary that the list has been converted to.</returns>
    private Dictionary<string, T> ArrayToEvidenceDictionary(T[] evidenceList)
    {
        var evidenceDictionary = new Dictionary<string, T>();
        foreach (var evidence in evidenceList)
        {
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

        if (_currentEvidenceDictionary.ContainsKey(evidenceName))
        {
            Debug.LogError($"Evidence with name {evidenceName} has already been added to the dictionary.");
            return;
        }
        
        T evidence = _masterEvidenceDictionary[evidenceName];
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
    public T GetEvidence(string evidenceName)
    {
        if (!IsEvidenceInDictionary(evidenceName, _currentEvidenceDictionary))
        {
            return null;
        }

        return _currentEvidenceDictionary[evidenceName];
    }

    /// <summary>
    /// Replaces a value in the current dictionary with another.
    /// </summary>
    /// <param name="valueName">The name of the value to replace.</param>
    /// <param name="altValue">The value to replace the original value with.</param>
    public void SubstituteValueWithAlt(string valueName, T altValue)
    {
        if (altValue == null)
        {
            Debug.LogError("Tried to substitute value with a null value.");
            return;
        }

        if (!IsEvidenceInDictionary(valueName, _currentEvidenceDictionary))
        {
            return;
        }
        
        _currentEvidenceDictionary[valueName] = altValue;
    }
    
    /// <summary>
    /// Method used by GetEvidence and AddEvidence methods to check
    /// if evidence is in the dictionary before attempting to access.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to look for in the dictionary.</param>
    /// <param name="dictionaryToCheck">The dictionary to look for the evidence object in.</param>
    /// <returns>True if the evidence is in the dictionary, and false if it is not.</returns>
    private bool IsEvidenceInDictionary(string evidenceName, Dictionary<string, T> dictionaryToCheck)
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
    public T GetValueAtIndex(int index)
    {
        try
        {
            return _currentEvidenceDictionary.Values.ElementAt(index);
        }
        catch (IndexOutOfRangeException exception)
        {
            Debug.LogError($"{exception.GetType().Name}: Index {index} out of range of dictionary.");
            return null;
        }
    }
}
