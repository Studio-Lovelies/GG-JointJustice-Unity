using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    protected List<T> CurrentObjectList { get; private set; }
    
    protected Dictionary<string, T> AvailableObjectDictionary { get; private set; }

    // Use square brackets to get objects from the inventory
    public T this[string objectName] => AvailableObjectDictionary[objectName];
    public T this[int objectIndex] => CurrentObjectList[objectIndex];
    public int Count => CurrentObjectList.Count;
    
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
        
        AvailableObjectDictionary = EnumerableToDictionary<Dictionary<string, T>>(_availableObjectList.ObjectArray);
    }

    /// <summary>
    /// Call this method to add an object to the inventory;
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        if (!IsObjectInDictionary(objectName, AvailableObjectDictionary) || IsObjectInList(objectName, CurrentObjectList))
        {
            return;
        }

        CurrentObjectList.Add(AvailableObjectDictionary[objectName]);
    }

    /// <summary>
    /// Call this method to remove an object from the inventory.
    /// </summary>
    /// <param name="objectName">The name of the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        if (!IsObjectInList(objectName, CurrentObjectList))
        {
            Debug.LogWarning($"Could not remove {objectName} from object inventory as object does not exist in object inventory.");
            return;
        }
        
        CurrentObjectList.Remove(AvailableObjectDictionary[objectName]);
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

    /// <summary>
    /// Checks if an object is in a list. Used to prevent duplicate objects being added to the object list.
    /// </summary>
    /// <param name="objectName">The name of the object to check.</param>
    /// <param name="objectList">The list to search.</param>
    /// <returns>Whether or object was in the list (true) or not (false).</returns>
    public static bool IsObjectInList(string objectName, List<T> objectList)
    {
        T objectInstance = objectList.SingleOrDefault(objectInList => objectInList.name == objectName);
        return objectInstance != null;
    }
}
