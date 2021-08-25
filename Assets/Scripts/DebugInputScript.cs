using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugInputScript : MonoBehaviour
{
    public UnityEvent _onSpacePressed;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _onSpacePressed.Invoke();
        }
    }
}
