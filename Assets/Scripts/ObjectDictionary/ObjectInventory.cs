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
    [SerializeField] private List<T> _currentObjectList;

    protected ObjectLookup<T> ObjectLookup { get; private set; }
    
    // Use square brackets to get objects from the inventory
    public T this[string objectName] => ObjectLookup[objectName];
    public T this[int objectIndex] => _currentObjectList[objectIndex];
    public int Count => ObjectLookup.CurrentObjectCount;

    /// <summary>
    /// Convert list to a dictionary on awake.
    /// </summary>
    private void Awake()
    {
        ObjectLookup = new ObjectLookup<T>(_objectList.ObjectArray, _currentObjectList);
    }

    /// <summary>
    /// Call this method to add an object to the inventory;
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        ObjectLookup.AddObject(objectName);
    }

    /// <summary>
    /// Call this method to remove an object from the inventory.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        ObjectLookup.RemoveObject(objectName);
    }
}
