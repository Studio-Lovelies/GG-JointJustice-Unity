using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioClip _onValueChangedSound;
    [SerializeField] private float _updateSoundDelay;
    
    private const int MIN_DECIBELS = -80;
    private const int MAX_DECIBELS = 20;
    private const int DECIBEL_LOG_BASE = 10;

    private float _lastUpdateSoundTime;
    private TextMeshProUGUI _label;
    private AudioMixerGroup _audioMixerGroup;
    private Slider _slider;
    private AudioSource _audioSource;

    private string VolumeParameterName => $"{_audioMixerGroup.name}Volume";

    public AudioMixerGroup AudioMixerGroup
    {
        get => _audioMixerGroup;
        set => _audioMixerGroup = value;
    }
    
    public string Text
    {
        get => _label.text;
        set => _label.text = value;
    }

    private void Awake()
    {
        _label = GetComponentInChildren<TextMeshProUGUI>();
        _slider = GetComponent<Slider>();
        _audioSource = GetComponent<AudioSource>();
        _slider.onValueChanged.AddListener(SetValue);
    }

    /// <summary>
    /// Updates the value of the slider UI component to be the same as the mixer group value
    /// </summary>
    public void UpdateSliderValue()
    {
        _audioMixerGroup.audioMixer.GetFloat(VolumeParameterName, out var value);
        _slider.value = CalculateLinearVolume(value);
    }

    /// <summary>
    /// Method used by the UI slider's OnValueChanged event to update the volume of the audio mixer
    /// </summary>
    /// <param name="value">The value to set the slider to. Takes a value from 0 to 1</param>
    private void SetValue(float value)
    {
        _audioMixerGroup.audioMixer.SetFloat(VolumeParameterName, CalculateVolumeInDecibels(value));
    }

    /// <summary>
    /// Boilerplate function to allow selecting of the UI slider
    /// </summary>
    public void Select()
    {
        _slider.Select();
    }

    /// <summary>
    /// Converts a value from -80 to 0 to a value from 0 to 1
    /// I.E converts from decibels used by AudioMixer to a linear value
    /// </summary>
    /// <param name="volumeInDecibels">The volume in decibels to convert. Takes a value from -80 to 0</param>
    private static float CalculateLinearVolume(float volumeInDecibels)
    {
        return Mathf.Pow(10, volumeInDecibels / MAX_DECIBELS);
    }

    /// <summary>
    /// Converts a value from 0 to 1 to a value from -80 to 0
    /// I.E converts from a linear value to decibels used by AudioMixer
    /// </summary>
    /// <param name="linearVolume">The value to convert to decibels. Takes a value from 0 to 1</param>
    private static float CalculateVolumeInDecibels(float linearVolume)
    {
        if (linearVolume == 0)
        {
            return MIN_DECIBELS;
        }
        return MAX_DECIBELS * Mathf.Log(linearVolume, DECIBEL_LOG_BASE);
    }

    /// <summary>
    /// Activates the sound played when changing the value of the slider
    /// This allows the value to be changed before activation without playing the sound
    /// Used when instantiating sliders and setting their initial values
    /// </summary>
    public void ActivateOnValueChangedSound()
    {
        _slider.onValueChanged.AddListener(_ => PlayUpdateSound());
    }

    /// <summary>
    /// Method used by the onValueChanged event of the UI slider to play a sound
    /// Only allows a sound to be played after a certain period of time to prevent audio spam
    /// </summary>
    private void PlayUpdateSound()
    {
        var unscaledTime = Time.unscaledTime;
        if (unscaledTime - _lastUpdateSoundTime < _updateSoundDelay)
        {
            return;
        }

        _audioSource.PlayOneShot(_onValueChangedSound);
        _lastUpdateSoundTime = unscaledTime;
    }
}
