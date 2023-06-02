using UnityEngine;
using UnityEngine.Audio;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSlider _sliderPrefab;

    /// <summary>
    /// Instantiates audio sliders for all mixer groups in an AudioMixer
    /// IMPORTANT: When adding new mixer groups to an AudioMixer remember
    /// to expose their volume parameter and give it a name in the AudioMixer window
    /// </summary>
    private void Start()
    {
        var firstSelectable = true;
        foreach (var audioMixerGroup in _audioMixer.FindMatchingGroups(string.Empty))
        {
            var audioSlider = Instantiate(_sliderPrefab, transform);
            if (firstSelectable)
            {
                audioSlider.Select();
                firstSelectable = false;
            }
            audioSlider.Text = audioMixerGroup.name;
            audioSlider.AudioMixerGroup = audioMixerGroup;
            audioSlider.UpdateSliderValue();
            audioSlider.ActivateOnValueChangedSound();
        }
    }
}
