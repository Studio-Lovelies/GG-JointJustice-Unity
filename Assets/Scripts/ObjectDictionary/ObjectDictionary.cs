using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Class to convert two lists into two dictionaries, a master dictionary and a current dictionary.
/// The current dictionary contains objects that are currently accessible.
/// The master dictionary contains objects that can be added to the current dictionary.
/// Also contains error reporting for when objects cannot be found in the dictionaries.
/// </summary>
/// <typeparam name="T">The type of object to contain in the dictionaries.</typeparam>
public class ObjectDictionary<T> where T : Object
{
    private readonly Dictionary<string, T> _masterObjectDictionary;
    private readonly Dictionary<string, T> _currentObjectDictionary = new Dictionary<string, T>();
    private readonly List<string> _currentObjectList = new List<string>(); // Objects are stored in a list to guarantee order
    
    public T this[string objectName] => GetObject(objectName);
    public int Count => _currentObjectDictionary.Count;
    
    /// <summary>
    /// Takes two lists or arrays and converts them to dictionaries using ArrayToDictionary method.
    /// </summary>
    /// <param name="masterObjectList">The list of every possible object that can be added to the current list.</param>
    /// <param name="currentObjectList">The list of all objects that should be available at the start of a scene.</param>
    public ObjectDictionary(IEnumerable<T> masterObjectList, IEnumerable<T> currentObjectList)
    {
        if (masterObjectList != null)
        {
            _masterObjectDictionary = ArrayToDictionary(masterObjectList);
        }
        else
        {
            Debug.LogError("Master object list has not been assigned to CourtRecordObjectDictionary.");
            return;
        }

        if (currentObjectList != null)
        {
            var objectList = currentObjectList.ToList();
            _currentObjectDictionary = ArrayToDictionary(objectList);
            foreach (var obj in objectList)
            {
                _currentObjectList.Add(obj.name);
            }
        }
    }

    /// <summary>
    /// Converts a list of objects into a dictionary of objects
    /// so they can be accessed by name
    /// </summary>
    /// <param name="array">The list to convert to a dictionary.</param>
    /// <returns>The dictionary that the list has been converted to.</returns>
    private Dictionary<string, T> ArrayToDictionary(IEnumerable<T> array)
    {
        Dictionary<string, T> dictionary = new Dictionary<string, T>();
        foreach (T obj in array)
        {
            if (dictionary.ContainsKey(obj.name))
            {
                Debug.LogWarning($"Could not add object to dictionary, object with name {obj.name} already exists in the dictionary.");
                continue;
            }
            
            dictionary.Add(obj.name, obj);
        }

        return dictionary;
    }

    /// <summary>
    /// Call this method when you want to add an object to the current dictionary from the master dictionary.
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName, _masterObjectDictionary))
        {
            return;
        }

        if (_currentObjectDictionary.ContainsKey(objectName))
        {
            Debug.LogWarning($"Object with name {objectName} has already been added to the dictionary.");
            return;
        }
        
        T obj = _masterObjectDictionary[objectName];
        _currentObjectDictionary.Add(obj.name, obj);
        _currentObjectList.Add(obj.name);
    }

    /// <summary>
    /// Call this method when you want to remove an object from the dictionary.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName, _currentObjectDictionary))
        {
            return;
        }

        _currentObjectList.Remove(objectName);
        _currentObjectDictionary.Remove(objectName);
    }

    /// <summary>
    /// Call this method when you want to get object from the dictionary using the object's name.
    /// </summary>
    /// <param name="objectName">The name of the object to get.</param>
    /// <returns></returns>
    private T GetObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName, _currentObjectDictionary))
        {
            return null;
        }

        return _currentObjectDictionary[objectName];
    }

    /// <summary>
    /// Replaces an object in the current dictionary with another.
    /// </summary>
    /// <param name="valueName">The name of the object to replace.</param>
    /// <param name="altValue">The object to replace the original object with.</param>
    public void SubstituteValueWithAlt(string valueName, T altValue)
    {
        if (!IsObjectInDictionary(valueName, _currentObjectDictionary))
        {
            return;
        }
        
        if (altValue == null)
        {
            Debug.LogWarning($"Tried to substitute value {altValue} with a null value.");
            return;
        }
        
        _currentObjectDictionary[valueName] = altValue;
    }
    
    /// <summary>
    /// Method used by GetObject, AddObject, and RemoveObject methods to check
    /// if an object is in the dictionary before attempting to access it.
    /// </summary>
    /// <param name="objectName">The name of the object to look for in the dictionary.</param>
    /// <param name="dictionaryToCheck">The dictionary to look for the object in.</param>
    /// <returns>True if the object is in the dictionary, and false if it is not.</returns>
    private bool IsObjectInDictionary(string objectName, Dictionary<string, T> dictionaryToCheck)
    {
        if (!dictionaryToCheck.ContainsKey(objectName))
        {
            Debug.LogError($"Object {objectName} could not be found in the dictionary.");
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Accesses the object list. Allows accessing the dictionary by index.
    /// </summary>
    /// <param name="index">The index of the object to get.</param>
    /// <returns>The object at the specified index.</returns>
    public T GetObjectAtIndex(int index)
    {
        try
        {
            return _currentObjectDictionary[_currentObjectList[index]];
        }
        catch (IndexOutOfRangeException exception)
        {
            Debug.LogError($"{exception.GetType().Name}: Index {index} out of range of dictionary.");
            return null;
        }
    }
}


