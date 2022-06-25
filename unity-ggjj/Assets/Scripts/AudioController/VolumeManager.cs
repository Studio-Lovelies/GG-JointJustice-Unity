using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeManager : MonoBehaviour
{
    private AudioSource _audioSource;
    
    public AudioSource AudioSource
    {
        get
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
            return _audioSource;
        }
    }

    public float Volume { get; set; } = 1;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        Volume = AudioSource.volume;
    }

    public IEnumerator Fade(float targetVolume, float time)
    {
        var completion = 0f;
        var startVolume = AudioSource.volume;
        var startTime = Time.time;
        while (completion < 1)
        {
            AudioSource.volume = Mathf.Lerp(startVolume, targetVolume, completion);
            completion = (Time.time - startTime) / time;
            yield return null;
        }
        AudioSource.volume = targetVolume;
    }
}
