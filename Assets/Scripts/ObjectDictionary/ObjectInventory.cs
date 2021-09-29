using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

/// <summary>
/// This class uses an OrderedObjectStorage and its own Dictionary to handle
/// adding objects to a dictionary from a list of predefined objects.
/// Allows assigning of an object with an IObjectList interface to convert to a dictionary.
/// </summary>
/// <typeparam name="T">The type of values to be stored.</typeparam>
/// <typeparam name="S">The type that the assignable IObjectList should use.</typeparam>
public abstract class ObjectInventory<T, S> : MonoBehaviour where T : Object where S : IObjectList<T>
{
    [Tooltip("Drag an object list scriptable object here.")]
    [SerializeField] private S _objectList;

    [Tooltip("Drag objects that should be in the inventory when the scene starts here.")]
    [SerializeField] protected List<T> CurrentObjectList;
    
    protected Dictionary<string, T> AvailableObjectDictionary { get; private set; }
    
    
    // Use square brackets to get objects from the inventory
    public T this[string objectName] => ObjectDictionary[objectName];
    public T this[int objectIndex] => CurrentObjectList[objectIndex];
    public int Count => ObjectDictionary.CurrentObjectListCount;

    protected ObjectDictionary<T> ObjectDictionary { get; private set; }

    /// <summary>
    /// Convert list to a dictionary on awake.
    /// </summary>
    private void Awake()
    {
        ObjectDictionary = new ObjectDictionary<T>(_objectList.ObjectArray, CurrentObjectList);
    }

    /// <summary>
    /// Call this method to add an object to the inventory;
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        ObjectDictionary.AddObject(objectName);
    }

    /// <summary>
    /// Call this method to remove an object from the inventory.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        ObjectDictionary.RemoveObject(objectName);
    }
}
