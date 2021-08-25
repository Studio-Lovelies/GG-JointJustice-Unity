using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugInputScript : MonoBehaviour
{
    public UnityEvent _onSpacePressed;
    public UnityEvent _onCPressed;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _onSpacePressed.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _onCPressed.Invoke();
        }
    }
}
