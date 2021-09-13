using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

/// <summary>
/// This class allows interaction between unity objects and ObjectDictionary objects.
/// Allows assigning of an object with an IObjectList interface to convert to a dictionary.
/// </summary>
/// <typeparam name="T">The type of values in the dictionary.</typeparam>
/// <typeparam name="S">The type that the assignable IObjectList should use.</typeparam>
public abstract class ObjectDictionaryInterface<T, S> : MonoBehaviour, IDictionary<string, T> where T : Object where S : IObjectList<T>
{
    [SerializeReference, Tooltip("Drag an IObjectList here containing a list of all available objects.")]
    private S _availableObjectList;
    
    [field: SerializeField, Tooltip("Add any object that the player should have at the start of the scene here.")]
    protected T[] CurrentObjectList { get; private set; }
    
    protected ObjectDictionary<T> ObjectDictionary;
    
    // Use square brackets to get objects from the dictionary
    public T this[string objectName] => ObjectDictionary[objectName];

    public int Count => ObjectDictionary.Count;
    
    /// <summary>
    /// Converts the list into a dictionary on awake
    /// </summary>
    private void Awake()
    {
        if (_availableObjectList == null)
        {
            Debug.LogError($"Available object list has not been assigned to {name} on {gameObject.name}.");
            return;
        }
        ObjectDictionary = new ObjectDictionary<T>(_availableObjectList.ObjectArray, CurrentObjectList);
    }

    /// <summary>
    /// Call this method to add an object to the dictionary.
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        ObjectDictionary.AddObject(objectName);
    }

    /// <summary>
    /// Call this method to remove an object from the dictionary
    /// </summary>
    /// <param name="objectName">The name o the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        ObjectDictionary.RemoveObject(objectName);
    }

    /// <summary>
    /// Call this method to get an object at a specific index of the dictionary.
    /// </summary>
    /// <param name="index">The index of the object to get.</param>
    /// <returns>The object at the specified index.</returns>
    public T GetObjectAtIndex(int index)
    {
        return ObjectDictionary[index];
    }
}
