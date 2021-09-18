using UnityEngine;

public interface IObjectList<out T> where T : Object
{
    T[] ObjectArray { get; }
}