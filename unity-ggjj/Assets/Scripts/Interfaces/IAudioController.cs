using UnityEngine;

public interface IAudioController
{
    void SetDialogueVolume(float volume);
    void PlaySfx(AudioClip sfx);
    void PlayDialogue(AudioClip sfx);
    void PlayStaticSong(AudioClip song, float transitionTime);
    void PlayDynamicSong(DynamicMusicData dynamicMusicData, string variantName, float transitionTime);
    void StopSong();
    void FadeOutSong(float time);
}
