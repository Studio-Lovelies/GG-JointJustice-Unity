using System.Collections.Generic;
using Object = UnityEngine.Object;

public interface IObjectStorage
{
    public T GetObject<T>(string objectName);
    public IEnumerable<T> GetObjectsOfType<T>();
}