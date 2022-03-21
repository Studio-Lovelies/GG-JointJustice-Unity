using UnityEngine;

public interface IAudioController
{
    void PlaySfx(AudioClip sfx);
    void PlaySong(AudioClip song, float transitionTime);
    void StopSong();
    void FadeSong(float time);
}
