using UnityEngine;

/// <summary>
/// Wraps Resources.Load in a class accessible via interface
/// in order to avoid dependencies on UnityEngine
/// </summary>
public class ResourceLoader : IObjectLoader
{
    public Object Load(string path)
    {
        return Resources.Load(path);
    }
}