using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class acts as a wrapper around OrderedDictionary to ensure
/// type conversion is handled properly and also provides error reporting.
/// Also provides a method for converting multiple Objects into a dictionary using their names as keys.
/// </summary>
/// <typeparam name="T">The type of object to store.</typeparam>
public class OrderedObjectStorage<T> where T : Object
{
    private readonly OrderedDictionary _objectDictionary;

    public T this[string objectName] => GetObject(objectName);
    public T this[int objectIndex] => GetObjectAtIndex(objectIndex);
    public int Count => _objectDictionary.Count;

    public OrderedObjectStorage(IEnumerable<T> objectList)
    {
        _objectDictionary = EnumerableToDictionary<OrderedDictionary>(objectList);
    }

    /// <summary>
    /// Adds an object to the dictionary.
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    /// <param name="objectInstance">The object to add.</param>
    public void Add(string objectName, T objectInstance)
    {
        if (_objectDictionary.Contains(objectName))
        {
            Debug.LogWarning($"Object with name {objectName} has already been added to the dictionary.");
            return;
        }
        
        _objectDictionary.Add(objectName, objectInstance);
    }

    /// <summary>
    /// Removes an object from the dictionary.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void Remove(string objectName)
    {
        if (!IsObjectInDictionary(objectName, _objectDictionary))
        {
            return;
        }
        
        _objectDictionary.Remove(objectName);
    }

    /// <summary>
    /// Gets an object from the dictionary.
    /// Used internally by this class. To get objects from the instances of this class use square brackets.
    /// </summary>
    /// <param name="objectName">The name of the object to get.</param>
    /// <returns>The object with the name specified.</returns>
    private T GetObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName, _objectDictionary))
        {
            return null;
        }

        return _objectDictionary[objectName] as T;
    }

    /// <summary>
    /// Gets an object at a specific index of the dictionary.
    /// Used internally by this class. To get objects from the instances of this class use square brackets.
    /// </summary>
    /// <param name="index">The index of the object to get.</param>
    /// <returns>The object at the index specified.</returns>
    private T GetObjectAtIndex(int index)
    {
        if (index < 0 || index > _objectDictionary.Count)
        {
            Debug.LogError($"Index must be within bounds of object dictionary: 0 - {_objectDictionary.Count}.");
            return null;
        }

        return _objectDictionary.Cast<DictionaryEntry>().ElementAt(index).Value as T;
    }
    
    /// <summary>
    /// Replaces a value in the dictionary with another while keeping the key the same.
    /// </summary>
    /// <param name="key">The key the object is stored at.</param>
    /// <param name="newValue">The value to replace the current value with.</param>
    public void SubstituteObject(string key, T newValue)
    {
        if (!IsObjectInDictionary(key, _objectDictionary))
        {
            return;
        }
        
        if (newValue == null)
        {
            Debug.LogWarning($"Tried to substitute value with key {key} with a null value");
            return;
        }
        
        _objectDictionary[key] = newValue;
    }

    /// <summary>
    /// Used internally by this class to check if a key (and therefore
    /// by extension an object) exists in the dictionary.
    /// </summary>
    /// <param name="key">The key used to represent the object in the dictionary.</param>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static bool IsObjectInDictionary(string key, IDictionary dictionary)
    {
        if (!dictionary.Contains(key))
        {
            Debug.LogError($"Object with key {key} could not be found in the dictionary.");
            return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// Converts an object implementing IEnumerable into a dictionary.
    /// </summary>
    /// <param name="enumerable">The object to convert.</param>
    /// <typeparam name="S">The type of dictionary to return.</typeparam>
    /// <returns>The resulting dictionary.</returns>
    public static S EnumerableToDictionary<S>(IEnumerable<T> enumerable) where S : IDictionary, new()
    {
        S dictionary = new S();
        foreach (T obj in enumerable)
        {
            if (dictionary.Contains(obj.name))
            {
                Debug.LogWarning($"Could not add object to dictionary, object with name {obj.name} already exists in the dictionary.");
                continue;
            }
            dictionary.Add(obj.name, obj);
        }
        return dictionary;
    }
}