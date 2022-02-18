using UnityEngine;

public interface IAudioController
{
    void PlaySfx(AudioClip sfx);
    void PlaySong(AudioClip song);
    void StopSong();
}
