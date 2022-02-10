using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Handles input. Add events to this class use them to communicate with the Controls instance.
/// </summary>
public class UnityInputSystemInterface : MonoBehaviour, Controls.IUIActions
{
    private Controls _controls;
    private bool _selectPressed;
    private IEnumerator _lastSpeedupCoroutine;

    [SerializeField] private float _delayBeforeSpeedup = 0.5f; //In seconds

    // Add key press events here
    [SerializeField] private UnityEvent _onContinueStory;
    [SerializeField] private UnityEvent _onPressWitness;
    [SerializeField] private UnityEvent _onSpeedupTextStart;
    [SerializeField] private UnityEvent _onSpeedupTextEnd;
    [SerializeField] private UnityEvent _onCaseMenuOpened;
    [SerializeField] private UnityEvent _onPauseMenuOpened;
    
    public bool MenuOpen { get; set; }

    /// <summary>
    /// Called when the object is enabled
    /// </summary>
    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.UI.SetCallbacks(this);
        _controls.Enable();
    }

    /// <summary>
    /// Called when the object is disabled
    /// </summary>
    private void OnDisable()
    {
        _controls.UI.SetCallbacks(null);
        _controls.Disable();
    }

    /// <summary>
    /// Coroutine function (unity threading) which waits for a certain delay before starting to speed up the appearing text (if the button is still being pressed)
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAndSpeedUp()
    {
        yield return new WaitForSeconds(_delayBeforeSpeedup);

        if (_selectPressed)
        {
            _onSpeedupTextStart.Invoke();
        }
    }

    /// <summary>
    /// Called when the left mouse button is pressed anywhere on the screen.
    /// </summary>
    /// <param name="context"></param>
    void Controls.IUIActions.OnLeftMouseButton(InputAction.CallbackContext context)
    {
        //Unused for now
    }

    /// <summary>
    /// Called when any of the directional buttons are pressed
    /// </summary>
    /// <param name="context"></param>
    void Controls.IUIActions.OnDirectionalButtons(InputAction.CallbackContext context)
    {
        //Unused for now
    }

    /// <summary>
    /// Called when the select button is pressed (x)
    /// </summary>
    /// <param name="context"></param>
    void Controls.IUIActions.OnSelect(InputAction.CallbackContext context)
    {
        if (MenuOpen)
        {
            return;
        }

        if (context.performed)
        {
            _onContinueStory.Invoke();
            _selectPressed = true;
            _lastSpeedupCoroutine = WaitAndSpeedUp();
            if (_lastSpeedupCoroutine != null)
            {
                StartCoroutine(_lastSpeedupCoroutine);
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _selectPressed = false;
            if (_lastSpeedupCoroutine != null)
            {
                StopCoroutine(_lastSpeedupCoroutine);
                _onSpeedupTextEnd.Invoke();
                _lastSpeedupCoroutine = null;
            }

        }
    }

    /// <summary>
    /// Called when the press witness button is pressed (c)
    /// </summary>
    /// <param name="context"></param>
    void Controls.IUIActions.OnPress(InputAction.CallbackContext context)
    {
        if (context.performed)
            _onPressWitness.Invoke();
    }

    /// <summary>
    /// Called when the menu button is pressed (z)
    /// </summary>
    /// <param name="context"></param>
    void Controls.IUIActions.OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            _onCaseMenuOpened.Invoke();
    }

    /// <summary>
    /// Called when the pause button is pressed (esc)
    /// </summary>
    /// <param name="context"></param>
    void Controls.IUIActions.OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            _onPauseMenuOpened.Invoke();
    }

    void Controls.IUIActions.OnCursor(InputAction.CallbackContext context)
    {
        //Unused for now
    }
}
