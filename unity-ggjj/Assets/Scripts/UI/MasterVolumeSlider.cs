using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MasterVolumeSlider : MonoBehaviour
    {
        private Slider _slider;
        
        public float Value
        {
            set => AudioListener.volume = value;
        }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            _slider.value = AudioListener.volume;
        }
    }
}
