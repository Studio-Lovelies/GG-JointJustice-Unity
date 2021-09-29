using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectDictionary<T> where T : Object
{
    private List<T> _currentObjectList;
    private Dictionary<string, T> _availableObjectDictionary;

    public T this[string objectName] => _availableObjectDictionary[objectName];
    public T this[int objectIndex] => _currentObjectList[objectIndex];
    public int CurrentObjectListCount => _currentObjectList.Count;

    public ObjectDictionary(IEnumerable<T> availableOjectList, IEnumerable<T> currentObjectList)
    {
        _availableObjectDictionary = availableOjectList.ToDictionary(obj => obj.name, obj => obj);
        _currentObjectList = currentObjectList == null ? new List<T>() : new List<T>(currentObjectList);
    }

    public void AddObject(string objectName)
    {
        _currentObjectList.Add(this[objectName]);
    }

    public void RemoveObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName) || !IsObjectInList(this[objectName]))
        {
            return;
        }

        _currentObjectList.Remove(_currentObjectList[_currentObjectList.IndexOf(this[objectName])]);
    }

    public void SubstituteObject(string originalObjectName, T newObject)
    {
        _currentObjectList[_currentObjectList.IndexOf(this[originalObjectName])] = newObject;
    }

    private bool IsObjectInDictionary(string objectName)
    {
        if (_availableObjectDictionary.ContainsKey(objectName))
        {
            return true;
        }
        
        Debug.LogError($"Object {objectName} could not be found in dictionary of available objects.");
        return false;
    }

    private bool IsObjectInList(T obj)
    {
        if (_currentObjectList.SingleOrDefault(listObject => listObject == obj))
        {
            return true;
        }
        
        Debug.LogError($"Object {obj.name} could not be found in the list of current objects.");
        return false;
    }
}