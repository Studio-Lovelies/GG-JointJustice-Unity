using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private VolumeManager _volumeManager;

        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        
        private void OnEnable()
        {
            _slider.value = _volumeManager.Volume;
        }
    }
}
