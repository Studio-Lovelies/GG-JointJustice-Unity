using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles input. Add events to this class use them to communicate with the Controls instance.
/// </summary>
public class UnityInputSystemInterface : MonoBehaviour
{
    private Controls _controls;

    // Add key press events here
    [SerializeField] private UnityEvent _onPausePressed;

    private void Awake()
    {
        _controls = new Controls();
        
        // Subscribe to Input System events here
        _controls.Player.Pause.performed += ctx => _onPausePressed?.Invoke();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
