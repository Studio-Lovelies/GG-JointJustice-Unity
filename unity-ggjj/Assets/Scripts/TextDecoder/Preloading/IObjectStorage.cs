using System.Collections.Generic;
using Object = UnityEngine.Object;

public interface IObjectStorage
{
    public T GetObject<T>(string objectName) where T : Object;
    public IEnumerable<T> GetObjectsOfType<T>() where T : Object;
}