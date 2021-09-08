using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// This class allows interaction between unity objects and ObjectDictionary objects.
/// Allows assigning of an object with an IObjectList interface to convert to a dictionary.
/// </summary>
/// <typeparam name="T">The type of object to store in the dictionary.</typeparam>
/// <typeparam name="S">The type that the assignable IObjectList should use.</typeparam>
public abstract class ObjectDictionaryInterface<T, S> : MonoBehaviour where T : Object where S : IObjectList<T>
{
    [SerializeReference, Tooltip("Drag an Actor Dictionary here containing all the actors available in the scene.")]
    private S _masterObjectList;
    
    [field: SerializeField, Tooltip("Add any evidence that the player should have at the start of the scene here.")]
    protected T[] CurrentObjectList { get; private set; }
    
    protected ObjectDictionary<T> ObjectsDictionary;
    
    // Use square brackets to get objects from the dictionary
    public T this[string objectName] => ObjectsDictionary[objectName];

    public int Count => ObjectsDictionary.Count;
    
    /// <summary>
    /// Converts the actor list into a dictionary on awake
    /// </summary>
    private void Awake()
    {
        if (_masterObjectList == null)
        {
            Debug.LogError($"Master Object List has not been assigned to {name} on {gameObject.name}.");
            return;
        }
        
        ObjectsDictionary =
            new ObjectDictionary<T>(_masterObjectList.ObjectList, CurrentObjectList);
    }

    /// <summary>
    /// Call this method to add an object to the dictionary.
    /// </summary>
    /// <param name="objectName">The name of the object to add.</param>
    public void AddObject(string objectName)
    {
        ObjectsDictionary.AddObject(objectName);
    }

    /// <summary>
    /// Call this method to remove an object from the dictionary
    /// </summary>
    /// <param name="objectName">The name o the object to remove.</param>
    public void RemoveObject(string objectName)
    {
        ObjectsDictionary.RemoveObject(objectName);
    }

    /// <summary>
    /// Call this method to get an object at a specific index of the dictionary.
    /// </summary>
    /// <param name="index">The index of the object to get.</param>
    /// <returns>The object at the specified index.</returns>
    public T GetObjectAtIndex(int index)
    {
        return ObjectsDictionary.GetObjectAtIndex(index);
    }
}
