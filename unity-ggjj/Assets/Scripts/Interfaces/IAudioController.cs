using UnityEngine;

public interface IAudioController
{
    void PlaySfx(AudioClip sfx);
    void PlayStaticSong(AudioClip song, float transitionTime);
    void PlayDynamicSong(DynamicMusicData dynamicMusicData, string variantName, float transitionTime);
    void StopSong();
    void FadeOutSong(float time);
}
