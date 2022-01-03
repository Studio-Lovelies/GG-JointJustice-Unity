using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectStorage<T> where T : Object
{
    private Dictionary<string, T> _objects = new Dictionary<string, T>();

    public void Add(T obj)
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
}