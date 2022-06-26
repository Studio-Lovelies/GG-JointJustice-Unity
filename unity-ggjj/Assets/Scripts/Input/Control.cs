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
        [SerializeField] private UnityEvent _event;

        public void Enable()
        {
            _inputActionReference.action.started += Invoke;
        }

        public void Invoke(InputAction.CallbackContext ctx)
        {
            _event.Invoke();
        }

        public void Disable()
        {
            _inputActionReference.action.started -= Invoke;
        }
    }
}