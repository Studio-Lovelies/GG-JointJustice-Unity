using UnityEngine;

namespace UI
{
    public class ControlsButtonContainer : MonoBehaviour
    {
        private ControlButton[] _controlButtons;

        private void Awake()
        {
            _controlButtons = GetComponentsInChildren<ControlButton>();
        }

        public void DeselectControlButtons(ControlButton callingControlButton)
        {
            foreach (var controlButton in _controlButtons)
            {
                if (controlButton != callingControlButton)
                {
                    controlButton.CancelRebind();
                }
            }
        }
    }
}
