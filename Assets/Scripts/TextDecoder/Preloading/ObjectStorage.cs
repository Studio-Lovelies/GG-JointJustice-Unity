using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectStorage : IObjectStorage
{
    private Dictionary<string, Object> _objects = new Dictionary<string, Object>();

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
    
    public T GetObject<T>(string objectName) where T : Object
    {
        Debug.Log(objectName);
        return (T)_objects[objectName];
    }
}

public interface IObjectStorage
{
    public T GetObject<T>(string objectName) where T : Object;
}