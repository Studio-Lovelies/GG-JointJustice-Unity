using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Handles input. Add events to this class use them to communicate with the Controls instance.
/// </summary>
public class UnityInputSystemInterface : MonoBehaviour, Controls.IPlayerActions
{
    private Controls _controls;

    // Add key press events here
    [SerializeField] private UnityEvent OnAdvanceText;
    [SerializeField] private UnityEvent OnSpeedupTextStart;
    [SerializeField] private UnityEvent OnSpeedupTextEnd;
    [SerializeField] private UnityEvent OnCaseMenuOpened;


    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            Debug.Log("Set callbacks");
        }
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    void Controls.IPlayerActions.OnLeftMouseButton(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnDirectionalButtons(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnSelect(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnPress(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnMenu(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnPause(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnLeft(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnRight(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnUp(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }

    void Controls.IPlayerActions.OnDown(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Not implemented");
    }
}
