using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectStorage : IObjectStorage
{
    private readonly Dictionary<string, Object> _objects = new Dictionary<string, Object>();

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
        catch (ArgumentException exception)
        {
            throw new ObjectLoadingException($"Obj \"{obj.name}\" has already been added to object storage.", exception);
        }
    }
    
    /// <summary>
    /// Gets an object from the dictionary, attempting to cast it to a specified type
    /// </summary>
    /// <param name="objectName">The name of the object to get</param>
    /// <typeparam name="T">The type of the object to get</typeparam>
    /// <returns>An object with name objectName</returns>
    public T GetObject<T>(string objectName) where T : class
    {
        try
        {
            return _objects[objectName] as T;
        }
        catch (KeyNotFoundException exception)
        {
            throw new ObjectLoadingException($"Object \"{objectName}\" was not found in object storage", exception);
        }
    }

    /// <summary>
    /// Gets all the objects of a certain type.
    /// </summary>
    /// <typeparam name="T">The type of object to get.</typeparam>
    /// <returns>The objects found.</returns>
    public IEnumerable<T> GetObjectsOfType<T>() where T : class
    {
        return _objects.Values.Where(obj => obj is T).Cast<T>();
    }

    /// <summary>
    /// Method to check if an object is currently in storage
    /// </summary>
    /// <param name="obj">The object to look for</param>
    /// <returns>Whether the object was in storage (true) or not (false)</returns>
    public bool Contains(Object obj)
    {
        return _objects.ContainsValue(obj);
    }
}