using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class ControlButton : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputActionReference;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private UnityEvent _onButtonRebind;
        [SerializeField] private Color _rebindColor;

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
        private Menu _menu;
        private Image _image;
        private Color _originalColor;
        
        private void Awake()
        {
            UpdateButton();
            _menu = GetComponentInParent<Menu>();
            _image = GetComponent<Image>();
            _originalColor = _image.color;
        }

        public void BeginRebind()
        {
            _image.color = _rebindColor;
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
            _image.color = _originalColor;
            _inputActionReference.action.Enable();
            rebindingOperation.Dispose();
            _rebindingOperation = null;
            _onButtonRebind.Invoke();
            _menu.OnSetInteractable.Invoke(true);
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
