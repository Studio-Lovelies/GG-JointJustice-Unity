using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorActionDecoder : MonoBehaviour
{
    public void OnNewActionLine(string line)
    {
        if (line.Length == 0)
        {
            Debug.LogError(line);
        }
        else
        {
            Debug.LogWarning(line);
        }
        
    }
}
