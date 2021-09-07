using UnityEngine;

public interface IObjectList<T> where T : Object
{
    T[] ObjectList { get; }
}