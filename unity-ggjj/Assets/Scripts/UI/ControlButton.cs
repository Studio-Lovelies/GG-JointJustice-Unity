using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI
{
    public class ControlButton : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputActionReference;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private UnityEvent _onButtonRebind;

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
        
        private void Awake()
        {
            UpdateButton();
        }

        public void BeginRebind()
        {
            _inputActionReference.action.Disable();
            _rebindingOperation = _inputActionReference.action.PerformInteractiveRebinding(0)
                .WithControlsExcluding("<Mouse>")
                .OnPotentialMatch(OnPotentialMatch)
                .OnComplete(OnRebindComplete)
                .OnCancel(OnRebindComplete)
                .OnMatchWaitForAnother(0.1f)
                .Start();
        }

        private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
        {
            _inputActionReference.action.Enable();
            rebindingOperation.Dispose();
            _rebindingOperation = null;
            _onButtonRebind.Invoke();
            UpdateButton();
        }

        private void OnPotentialMatch(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
        {
            foreach (var action in _inputActionReference.asset.actionMaps[0].actions)
            {
                foreach (var control in action.controls)
                {
                    if (control == rebindingOperation.selectedControl)
                    {
                        rebindingOperation.Cancel();
                    }
                }
            }
        }

        private void UpdateButton()
        {
            _text.text = _inputActionReference.action.GetBindingDisplayString().Split('|')[0].Trim();
        }

        public void Cancel()
        {
            _rebindingOperation?.Cancel();
        }
    }
}
