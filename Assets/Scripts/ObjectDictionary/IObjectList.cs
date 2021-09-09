using UnityEngine;

public interface IObjectList<T> where T : Object
{
    T[] ObjectArray { get; }
}