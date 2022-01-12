using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectStorage : IObjectStorage
{
    private Dictionary<string, Object> _objects = new Dictionary<string, Object>();

    public int Count => _objects.Count;

    /// <summary>
    /// Adds an object to the dictionary using its name as a key.
    /// Gives a warning if the object has already been added to the dictionary.
    /// </summary>
    /// <param name="obj">The object to add.</param>
    public void Add(Object obj)
    {
        try
        {
            _objects.Add(obj.name, obj);
        }
        catch (ArgumentException)
        {
            Debug.LogWarning($"Obj \"{obj.name}\" has already been added to the dictionary.");
        }
    }
    
    /// <summary>
    /// Gets an object from the dictionary, attempting to cast it to a specified type
    /// </summary>
    /// <param name="objectName">The name of the object to get</param>
    /// <typeparam name="T">The type of the object to get</typeparam>
    /// <returns>An object with name objectName</returns>
    public T GetObject<T>(string objectName) where T : Object
    {
        return (T)_objects[objectName];
    }

    /// <summary>
    /// Gets all the objects of a certain type.
    /// </summary>
    /// <typeparam name="T">The type of object to get.</typeparam>
    /// <returns>The objects found.</returns>
    public IEnumerable<T> GetObjectsOfType<T>() where T : Object
    {
        return _objects.Values.Where(obj => obj is T).Cast<T>();
    }
}

public interface IObjectStorage
{
    public T GetObject<T>(string objectName) where T : Object;
    public IEnumerable<T> GetObjectsOfType<T>() where T : Object;
}