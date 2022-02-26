using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private List<InputModule> _inputModules;

    private void Awake()
    {
        _inputModules = GetComponentsInChildren<InputModule>().ToList();
    }

    /// <summary>
    /// Sets a specified input module to enabled and disables all others
    /// </summary>
    /// <param name="inputModule"></param>
    public void SetInput(InputModule inputModule)
    {
        _inputModules.ForEach(module => module.enabled = module == inputModule);
    }
}
