using System.Collections.Generic;
using Object = UnityEngine.Object;

public interface IObjectStorage
{
    public T GetObject<T>(string objectName) where T : class;
    public IEnumerable<T> GetObjectsOfType<T>() where T : class;
}