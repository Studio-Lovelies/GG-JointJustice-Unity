using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// This class allows interaction between unity objects and ObjectDictionary objects.
/// Allows assigning of an object with an IObjectList interface to convert to a dictionary.
/// </summary>
/// <typeparam name="T">The type of values in the dictionary.</typeparam>
/// <typeparam name="S">The type that the assignable IObjectList should use.</typeparam>
public abstract class ObjectDictionaryInterface<T, S> : MonoBehaviour where T : Object where S : IObjectList<T>
{
    [SerializeReference, Tooltip("Drag an IObjectList here containing a list of all available objects.")]
    private S _availableObjectList;

    [field: SerializeField, Tooltip("Add any object that the player should have at the start of the scene here.")]
    private T[] _initialObjectArray;

    private readonly List<string> _currentObjectList = new List<string>();
    private Dictionary<string, T> _availableObjectDictionary;
    
    protected Dictionary<string, T> CurrentObjectDictionary = new Dictionary<string, T>();
    
    public T this[string objectName] => GetObject(objectName); // Use square brackets to get objects from the dictionary
    public int Count => CurrentObjectDictionary.Count;
    public T this[int objectIndex] => GetObjectAtIndex(objectIndex);
    
    /// <summary>
    /// Converts the list into a dictionary on awake
    /// </summary>
    private void Awake()
    {
        if (_availableObjectList != null)
        {
            _availableObjectDictionary = ArrayToDictionary(_availableObjectList.ObjectArray);
        }
        else
        {
            Debug.LogError("Master object list has not been assigned to CourtRecordObjectDictionary.");
            return;
        }
        
        CurrentObjectDictionary = ArrayToDictionary(_initialObjectArray);
        foreach (var obj in _initialObjectArray)
        {
            _currentObjectList.Add(obj.name);
        }
    }

    /// <summary>
    /// Converts a list of objects into a dictionary of objects
    /// so they can be accessed by name
    /// </summary>
    /// <param name="array">The list to convert to a dictionary.</param>
    /// <returns>The dictionary that the list has been converted to.</returns>
    private static Dictionary<string, T> ArrayToDictionary(IEnumerable<T> array)
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
        if (!IsObjectInDictionary(objectName, _availableObjectDictionary))
        {
            return;
        }

        if (CurrentObjectDictionary.ContainsKey(objectName))
        {
            Debug.LogWarning($"Object with name {objectName} has already been added to the dictionary.");
            return;
        }
        
        T obj = _availableObjectDictionary[objectName];
        CurrentObjectDictionary.Add(obj.name, obj);
        _currentObjectList.Add(obj.name);
    }

    /// <summary>
    /// Call this method when you want to remove an object from the dictionary.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName, CurrentObjectDictionary))
        {
            return;
        }

        _currentObjectList.Remove(objectName);
        CurrentObjectDictionary.Remove(objectName);
    }

    /// <summary>
    /// Call this method when you want to get object from the dictionary using the object's name.
    /// </summary>
    /// <param name="objectName">The name of the object to get.</param>
    /// <returns></returns>
    private T GetObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName, CurrentObjectDictionary))
        {
            return null;
        }

        return CurrentObjectDictionary[objectName];
    }

    /// <summary>
    /// Replaces an object in the current dictionary with another.
    /// </summary>
    /// <param name="valueName">The name of the object to replace.</param>
    /// <param name="altValue">The object to replace the original object with.</param>
    public void SubstituteValueWithAlt(string valueName, T altValue)
    {
        if (!IsObjectInDictionary(valueName, CurrentObjectDictionary))
        {
            return;
        }
        
        if (altValue == null)
        {
            Debug.LogWarning($"Tried to substitute value {altValue} with a null value.");
            return;
        }
        
        CurrentObjectDictionary[valueName] = altValue;
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
    private T GetObjectAtIndex(int index)
    {
        try
        {
            return CurrentObjectDictionary[_currentObjectList[index]];
        }
        catch (IndexOutOfRangeException exception)
        {
            Debug.LogError($"{exception.GetType().Name}: Index {index} out of range of dictionary.");
            return null;
        }
    }
}
