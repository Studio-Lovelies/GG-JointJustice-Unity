using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class ControlButton : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputActionReference;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            UpdateButton();
        }

        public void BeginRebind()
        {
            _inputActionReference.action.Disable();
            _inputActionReference.action.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnComplete(OnRebindComplete)
                .OnMatchWaitForAnother(0.1f)
                .Start();
        }

        private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
        {
            _inputActionReference.action.Enable();
            rebindingOperation.Dispose();
            UpdateButton();
        }

        private void UpdateButton()
        {
            _text.text = _inputActionReference.action.GetBindingDisplayString();
        }
    }
}
