using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private const int MIN_DECIBALS = -80;
    private const int MAX_DECIBALS = 20;
    
    private TextMeshProUGUI _label;
    private AudioMixerGroup _audioMixerGroup;
    private Slider _slider;

    private string VolumeParameterName => $"{_audioMixerGroup.name}Volume";

    public AudioMixerGroup AudioMixerGroup
    {
        get => _audioMixerGroup;
        set
        {
            _audioMixerGroup = value;
            UpdateValue();
        }
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
        _slider.onValueChanged.AddListener(SetValue);
    }

    private void UpdateValue()
    {
        _audioMixerGroup.audioMixer.GetFloat(VolumeParameterName, out var value);
        _slider.value = Mathf.InverseLerp(MIN_DECIBALS, MAX_DECIBALS, value);
    }

    private void SetValue(float value)
    {
        _audioMixerGroup.audioMixer.SetFloat(VolumeParameterName, Mathf.Lerp(MIN_DECIBALS, MAX_DECIBALS, value));
    }

    public void Select()
    {
        _slider.Select();
    }
}
