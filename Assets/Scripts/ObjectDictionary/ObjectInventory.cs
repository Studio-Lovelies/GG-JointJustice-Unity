using System.Collections.Generic;
using UnityEngine;
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
    [SerializeReference, Tooltip("Drag an IObjectList here containing a list of all available objects.")]
    private S _availableObjectList;

    [field: SerializeField, Tooltip("Add any object that the player should have at the start of the scene here.")]
    private T[] _currentObjectList;
    
    private Dictionary<string, T> _availableObjectDictionary;
    
    protected OrderedObjectStorage<T> ObjectStorage { get; private set; }

    // Use square brackets to get objects from the inventory
    public T this[string objectName] => ObjectStorage[objectName];
    public T this[int objectIndex] => ObjectStorage[objectIndex];
    public int Count => ObjectStorage.Count;
    
    /// <summary>
    /// Converts the available objects list into a dictionary
    /// and create an OrderedObjectStorage on awake.
    /// </summary>
    private void Awake()
    {
        if (_availableObjectList == null)
        {
            Debug.LogError($"Available object list has not been assigned to {name} on {gameObject.name}.");
            return;
        }

        ObjectStorage = new OrderedObjectStorage<T>(_currentObjectList);
        _availableObjectDictionary = OrderedObjectStorage<T>.EnumerableToDictionary<Dictionary<string, T>>(_availableObjectList.ObjectArray);
    }

    /// <summary>
    /// Call this method to add an object to the inventory;
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        if (!OrderedObjectStorage<T>.IsObjectInDictionary(objectName, _availableObjectDictionary))
        {
            return;
        }
        
        ObjectStorage.Add(objectName, _availableObjectDictionary[objectName]);
    }

    /// <summary>
    /// Call this method to remove an object from the inventory.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        ObjectStorage.Remove(objectName);
    }
}
