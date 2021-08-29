using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Handles input. Add events to this class use them to communicate with the Controls instance.
/// </summary>
public class UnityInputSystemInterface : MonoBehaviour, Controls.IPlayerActions
{
    private Controls _controls;
    private bool _selectPressed;
    private IEnumerator _lastSpeedupCoroutine;

    [SerializeField] private float _delayBeforeSpeedup = 0.5f; //In seconds

    // Add key press events here
    [SerializeField] private UnityEvent OnContinueStory;
    [SerializeField] private UnityEvent OnPressWitness;
    [SerializeField] private UnityEvent OnSpeedupTextStart;
    [SerializeField] private UnityEvent OnSpeedupTextEnd;
    [SerializeField] private UnityEvent OnCaseMenuOpened;

    [SerializeField] private UnityEvent OnPauseMenuOpened;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private IEnumerator WaitAndSpeedUp()
    {
        yield return new WaitForSeconds(_delayBeforeSpeedup);

        if (_selectPressed)
        {
            OnSpeedupTextStart.Invoke();
        }
    }

    void Controls.IPlayerActions.OnLeftMouseButton(InputAction.CallbackContext context)
    {
        //Unused for now
    }

    void Controls.IPlayerActions.OnDirectionalButtons(InputAction.CallbackContext context)
    {
        //Unused for now
    }

    void Controls.IPlayerActions.OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnContinueStory.Invoke();
            _selectPressed = true;
            _lastSpeedupCoroutine = WaitAndSpeedUp();
            StartCoroutine(_lastSpeedupCoroutine);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _selectPressed = false;
            if (_lastSpeedupCoroutine != null)
            {
                StopCoroutine(_lastSpeedupCoroutine);
                OnSpeedupTextEnd.Invoke();
                _lastSpeedupCoroutine = null;
            }

        }
    }

    void Controls.IPlayerActions.OnPress(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnPressWitness.Invoke();
    }

    void Controls.IPlayerActions.OnMenu(InputAction.CallbackContext context)
    {
        OnCaseMenuOpened.Invoke();
    }

    void Controls.IPlayerActions.OnPause(InputAction.CallbackContext context)
    {
        OnPauseMenuOpened.Invoke();
    }

    void Controls.IPlayerActions.OnLeft(InputAction.CallbackContext context)
    {
        //Unused for now
    }

    void Controls.IPlayerActions.OnRight(InputAction.CallbackContext context)
    {
        //Unused for now
    }

    void Controls.IPlayerActions.OnUp(InputAction.CallbackContext context)
    {
        //Unused for now
    }

    void Controls.IPlayerActions.OnDown(InputAction.CallbackContext context)
    {
        //Unused for now
    }
}
