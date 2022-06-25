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
        while (AudioSource.volume != targetVolume)
        {
            AudioSource.volume = Mathf.MoveTowards(AudioSource.volume, targetVolume, Time.deltaTime / time);
            yield return null;
        }
    }
}
