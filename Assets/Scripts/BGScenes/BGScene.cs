using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScene : MonoBehaviour
{
    public Actor ActiveActor { get; private set; }

    void Awake()
    {
        ActiveActor = GetComponentInChildren<Actor>();
    }

    
}
