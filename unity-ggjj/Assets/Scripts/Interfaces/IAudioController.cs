using UnityEngine;

public interface IAudioController
{
    void PlaySfx(string sfx);
    void PlaySfx(AudioClip sfx);
    void PlaySong(AudioClip song);
    void StopSong();
}
