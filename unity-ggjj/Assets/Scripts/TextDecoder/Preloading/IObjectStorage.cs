using System.Collections.Generic;

public interface IObjectStorage
{
    public T GetObject<T>(string objectName) where T : class;
    public IEnumerable<T> GetObjectsOfType<T>() where T : class;
}