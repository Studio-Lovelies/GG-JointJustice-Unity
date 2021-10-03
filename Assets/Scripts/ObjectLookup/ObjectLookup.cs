using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Testable class that retrieves objects by name from a dictionary and stores them in a list
/// that is used for displaying them in order.
/// </summary>
/// <typeparam name="T">The type of values to be stored.</typeparam>
public class ObjectLookup<T> where T : Object
{
    private readonly List<T> _currentObjectList;
    private readonly Dictionary<string, T> _availableObjectDictionary;

    public T this[string objectName] => _availableObjectDictionary[objectName];
    public int AvailableObjectCount => _availableObjectDictionary.Count;
    public int CurrentObjectCount => _currentObjectList.Count;

    /// <summary>
    /// Creates an ObjectDictionary by converting the list of available objects to the dictionary
    /// and creating a new current object list based on the list of current objects inputted.
    /// </summary>
    /// <param name="availableOjectList">List to be converted to a dictionary of available objects.</param>
    /// <param name="currentObjectList">List to be added to list of current objects on creation.</param>
    public ObjectLookup(IEnumerable<T> availableOjectList, IEnumerable<T> currentObjectList)
    {
        _availableObjectDictionary = availableOjectList.ToDictionary(obj => obj.name, obj => obj);
        _currentObjectList = currentObjectList == null ? new List<T>() : new List<T>(currentObjectList);
    }

    /// <summary>
    /// Adds an object to the list of current objects.
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName))
        {
            return;
        }

        if (IsObjectInList(_availableObjectDictionary[objectName]))
        {
            Debug.LogWarning($"Object {objectName} is already in the list of current objects.");
            return;
        }

        _currentObjectList.Add(this[objectName]);
    }

    /// <summary>
    /// Removes an object from the list of current objects.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName))
        {
            return;
        }

        if (!IsObjectInList(this[objectName]))
        {
            Debug.LogWarning($"Object {objectName} could not be found in the list of current objects.");
            return;
        }

        _currentObjectList.Remove(this[objectName]);
    }

    /// <summary>
    /// Replaces an object in the list of current objects with another one.
    /// </summary>
    /// <param name="originalObjectName">The name of the object to replace.</param>
    /// <param name="newObject">The object to replace the object with.</param>
    public void SubstituteObject(string originalObjectName, T newObject)
    {
        if (newObject == null)
        {
            Debug.LogWarning($"Could not substitute {originalObjectName}. Provided alternate object was null.");
            return;
        }
        
        if (!IsObjectInDictionary(originalObjectName) || !IsObjectInList(_availableObjectDictionary[originalObjectName]))
        {
            return;
        }
        
        _currentObjectList[_currentObjectList.IndexOf(this[originalObjectName])] = newObject;
    }

    /// <summary>
    /// Method to check if an object is in the dictionary of available objects and give an appropriate error message if not.
    /// </summary>
    /// <param name="objectName">The name of the object to check for.</param>
    /// <returns>Whether the object was in the dictionary (true) or not (false).</returns>
    private bool IsObjectInDictionary(string objectName)
    {
        if (_availableObjectDictionary.ContainsKey(objectName))
        {
            return true;
        }
        
        throw new ArgumentException($"Object {objectName} could not be found in dictionary of available objects.");
    }

    /// <summary>
    /// Method to check if an object is in the list of current objects.
    /// No error as this is used to check if object should be in list and if object should not be in list.
    /// </summary>
    /// <param name="obj">The object to check for.</param>
    /// <returns>Whether the object was in the list (true) or not (false).</returns>
    private bool IsObjectInList(T obj)
    {
        return _currentObjectList.Count(listObject => listObject == obj) > 0;
    }

    /// <summary>
    /// Get an object from the current object list by its build index.
    /// Not accessed using brackets [] to avoid confusion with
    /// accessing objects from dictionary of available objects
    /// </summary>
    /// <param name="objectIndex"></param>
    /// <returns></returns>
    public T GetObjectInList(int objectIndex)
    {
        return _currentObjectList[objectIndex];
    }
}