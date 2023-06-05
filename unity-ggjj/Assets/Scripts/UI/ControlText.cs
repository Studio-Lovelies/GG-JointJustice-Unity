using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class ControlText : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputActionReference;
        
        private TextMeshProUGUI _text;
        private Menu _menu;

        private void Awake()
        {
            _menu = GetComponentInParent<Menu>();
            _menu.OnSetInteractable.AddListener(isInteractable =>
            {
                if (isInteractable)
                {
                    OnEnable();
                }
            });
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _text.text = _inputActionReference.action.GetBindingDisplayString().Split('|')[0].ToUpper().Trim();
        }
    }
}
