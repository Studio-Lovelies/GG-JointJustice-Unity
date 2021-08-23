using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles input. Add events to this class use them to communicate with the Controls instance.
/// </summary>
public class InputManager : MonoBehaviour
{
    private Controls Controls;

    // Add key press events here
    [SerializeField] private UnityEvent _onPausePressed;
    
    
    private void Awake()
    {
        Controls = new Controls();
        
        // Subscribe to Input System events here
        Controls.Player.Pause.performed += ctx => _onPausePressed?.Invoke();
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
