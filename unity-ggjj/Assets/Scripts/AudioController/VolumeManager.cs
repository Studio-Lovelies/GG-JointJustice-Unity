using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeManager : MonoBehaviour
{
    public AudioSource AudioSource { get; private set; }
    public float Volume { get; set; } = 1;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
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
