using UnityEngine;

namespace Input
{
    /// <summary>
    /// Handles input. Add events to this class use them to communicate with the Controls instance.
    /// </summary>
    public class InputModule : MonoBehaviour
    {
        [SerializeField] private Control[] _controls;

        public void OnEnable()
        {
            foreach (var control in _controls)
            {
                control.Enable();
            }
        }

        public void OnDisable()
        {
            foreach (var control in _controls)
            {
                control.Disable();
            }
        }
    }
}
