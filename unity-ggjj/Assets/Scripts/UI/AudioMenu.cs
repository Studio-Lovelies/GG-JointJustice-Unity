using UnityEngine;
using UnityEngine.Audio;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSlider _sliderPrefab;

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
        }
    }
}
