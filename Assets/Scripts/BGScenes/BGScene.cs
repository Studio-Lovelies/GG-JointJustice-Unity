using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScene : MonoBehaviour
{
    public Actor ActiveActor { get; private set; }

    /// <summary>
    /// Sets the active actor to the first actor in the scene.
    /// </summary>
    void Awake()
    {
        ActiveActor = GetComponentInChildren<Actor>();
    }

    
}
