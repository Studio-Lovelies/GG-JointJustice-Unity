using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
    [Serializable]
    public class Control
    {
        [SerializeField] private InputActionReference _inputActionReference;
        [SerializeField] private UnityEvent<InputAction.CallbackContext> _event;

        public void Enable()
        {
            _inputActionReference.action.performed += Invoke;
            _inputActionReference.action.Enable();
        }

        public void Invoke(InputAction.CallbackContext inputCallbackContext)
        {
            _event.Invoke(inputCallbackContext);
        }

        public void Disable()
        {
            _inputActionReference.action.performed -= Invoke;
        }
    }
}