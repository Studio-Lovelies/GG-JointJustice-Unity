using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectList<T> : IList<T> where T : UnityEngine.Object
{
    private List<T> _list;

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public int Count { get; }
    public bool IsReadOnly { get; }
    public bool IsSynchronized { get; }
    public object SyncRoot { get; }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public void Add(T item)
    {
        // if (!IsObjectInDictionary(objectName, AvailableObjectDictionary) || IsObjectInList(objectName, CurrentObjectList))
        // {
        //     return;
        // }
        //
        // CurrentObjectList.Add(AvailableObjectDictionary[objectName]);
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int index)
    {
        _list.CopyTo(array, index);
    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public T this[int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    
    /// <summary>
    /// Checks if an object is in a list. Used to prevent duplicate objects being added to the object list.
    /// </summary>
    /// <param name="objectName">The name of the object to check.</param>
    /// <param name="objectList">The list to search.</param>
    /// <returns>Whether or object was in the list (true) or not (false).</returns>
    private bool IsObjectInList(string objectName, List<T> objectList)
    {
        T objectInstance = objectList.SingleOrDefault(objectInList => objectInList.name == objectName);
        return objectInstance != null;
    }
}