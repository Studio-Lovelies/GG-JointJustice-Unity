using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Class that takes an Object and an IObjectList and creates an ObjectLookup object.
/// Humble object that allows unity components to communicate with the testable ObjectLookup class.
/// </summary>
/// <typeparam name="T">The type of values to be stored.</typeparam>
/// <typeparam name="S">A class implementing IObjectList with type T.</typeparam>
public abstract class ObjectInventory<T, S> : MonoBehaviour where T : Object where S : IObjectList<T>
{
    [Tooltip("Drag an object list scriptable object here.")]
    [SerializeField] private S _objectList;
    
    [Tooltip("Drag objects that should be in the inventory when the scene starts here.")]
    [SerializeField] private List<T> _currentObjectList;

    protected ObjectLookup<T> ObjectLookup { get; private set; }
    
    // Use square brackets to get objects from the inventory
    public T this[string objectName] => ObjectLookup[objectName];
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
