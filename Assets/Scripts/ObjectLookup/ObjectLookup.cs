using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Stores all available objects that are available in a scene in a dictionary.
/// Values in the dictionary are accessed by name and are added to a list 
/// that consists only of values that exist in the dictionary.
/// The list represents the objects currently owned by the player,
/// and is used for displaying these objects in the evidence menu.
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
    /// <param name="availableObjectList">List to be converted to a dictionary of available objects.</param>
    /// <param name="currentObjectList">List to be added to list of current objects on creation.</param>
    public ObjectLookup(IEnumerable<T> availableObjectList, IEnumerable<T> currentObjectList)
    {
        _availableObjectDictionary = availableObjectList.ToDictionary(obj => obj.name, obj => obj);
        _currentObjectList = currentObjectList == null ? new List<T>() : new List<T>(currentObjectList);
    }

    /// <summary>
    /// Adds an object to the list of current objects.
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        VerifyObjectInDictionary(objectName);
        T obj = this[objectName];
        VerifyObjectNotInList(obj);
        _currentObjectList.Add(obj);
    }

    /// <summary>
    /// Removes an object from the list of current objects.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        VerifyObjectInDictionary(objectName);
        T obj = this[objectName];
        VerifyObjectInList(obj);
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
        
        VerifyObjectInDictionary(originalObjectName);
        T obj = this[originalObjectName];
        VerifyObjectInList(obj);
        _currentObjectList[_currentObjectList.IndexOf(obj)] = newObject;
    }

    /// <summary>
    /// Method to check if an object is in the dictionary of available objects and give an appropriate error message if not.
    /// </summary>
    /// <param name="objectName">The name of the object to check for.</param>
    /// <returns>Whether the object was in the dictionary (true) or not (false).</returns>
    private void VerifyObjectInDictionary(string objectName)
    {
        if (!_availableObjectDictionary.ContainsKey(objectName))
        {
            throw new ArgumentException($"Object {objectName} could not be found in dictionary of available objects.");
        }
    }

    /// <summary>
    /// Method to check if an object is in the list of current objects.
    /// Throws an ArgumentException if the object is NOT in the list.
    /// </summary>
    /// <param name="obj">The object to check for.</param>
    /// <returns>Whether the object was in the list (true) or not (false).</returns>
    private void VerifyObjectInList(T obj)
    {
        if (!IsObjectInList(obj))
        {
            throw new ArgumentException($"Object {obj.name} could not be found in the list of current objects.");
        }
    }

    /// <summary>
    /// Method to check if an object is NOT in the list of current objects.
    /// Throws an ArgumentException if the object is in the list.
    /// </summary>
    /// <param name="obj">The object to check for.</param>
    /// <returns>Whether the object was in the list (true) or not (false).</returns>
    private void VerifyObjectNotInList(T obj)
    {
        if (IsObjectInList(obj))
        {
            throw new ArgumentException($"Could not add object {obj.name} to list as it is already in the list of current objects.");
        }
    }

    /// <summary>
    /// Checks if an object is in the list of current objects.
    /// </summary>
    /// <param name="obj">The object that is being checked.</param>
    /// <returns>If the object is in the list (true) or not (false).</returns>
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
    /// <returns>The object from the list.</returns>
    public T GetObjectInList(int objectIndex)
    {
        return _currentObjectList[objectIndex];
    }
}