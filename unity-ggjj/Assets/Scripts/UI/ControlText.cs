using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class ControlText : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputActionReference;
        
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _text.text = _inputActionReference.action.GetBindingDisplayString().Split('|')[0].ToUpper().Trim();
        }
    }
}
