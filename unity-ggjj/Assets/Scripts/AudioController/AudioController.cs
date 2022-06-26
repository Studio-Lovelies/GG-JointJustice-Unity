using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour, IAudioController
{
    [SerializeField] private VolumeManager _musicVolumeManager;
    [SerializeField] private VolumeManager _sfxVolumeManager;

    [Tooltip("Drag an AudioClip here to be played on scene start")]
    [SerializeField] private AudioClip _defaultSong;
    
    private Coroutine _currentFadeCoroutine;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Awake()
    {
        PlaySong(_defaultSong, 0);
        _musicVolumeManager.AudioSource.Play();
    }

    /// <summary>
    /// Overload for PlaySong which allows songs to be played using a direct reference.
    /// </summary>
    /// <param name="song">The song to play.</param>
    /// <param name="transitionTime">The time taken to transition</param>
    public void PlaySong(AudioClip song, float transitionTime)
    {
        if (_currentFadeCoroutine != null)
        {
            StopCoroutine(_currentFadeCoroutine);
        }

        _currentFadeCoroutine = StartCoroutine(FadeToNewSong(song, transitionTime));
    }

    /// <summary>
    /// Play given audio clip immediately
    /// </summary>
    /// <param name="soundEffectClip">Clip to play</param>
    public void PlaySfx(AudioClip soundEffectClip)
    {
        _sfxVolumeManager.AudioSource.PlayOneShot(soundEffectClip);
    }

    /// <summary>
    /// Coroutine to fade to a new song.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    /// <param name="transitionTime">The time taken to transition</param>
    private IEnumerator FadeToNewSong(AudioClip song, float transitionTime)
    {
        if (IsCurrentlyPlayingMusic())
        {
            yield return FadeOutSongCoroutine(transitionTime / 2f);
        }

        SetCurrentTrack(song);
        yield return _musicVolumeManager.Fade(_musicVolumeManager.MaximumVolume, transitionTime / 2f);
    }

    /// <summary>
    /// Coroutine to found out a song over a given time
    /// </summary>
    /// <param name="time">The time taken to fade out</param>
    private IEnumerator FadeOutSongCoroutine(float time)
    {
        yield return _musicVolumeManager.Fade(0, time);
        StopSong();
    }

    /// <summary>
    /// If music is currently playing, stop it!
    /// </summary>
    public void StopSong()
    {
        if (_musicVolumeManager != null)
        {
            _musicVolumeManager.AudioSource.Stop();
        }
    }

    /// <summary>
    /// Fade out the currently playing song over a given time
    /// </summary>
    /// <param name="time">The time taken to fade out</param>
    public void FadeOutSong(float time)
    {
        StartCoroutine(FadeOutSongCoroutine(time));
    }

    /// <summary>
    /// Are we playing music?
    /// </summary>
    /// <returns>True if we're playing a music track with a volume above zero</returns>
    private bool IsCurrentlyPlayingMusic()
    {
        return _musicVolumeManager.AudioSource.clip != null && _musicVolumeManager.AudioSource.volume != 0;
    }

    /// <summary>
    /// Assigns the music player to a given song with 0 volume, intending to fade it in.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    private void SetCurrentTrack(AudioClip song)
    {
        _musicVolumeManager.AudioSource.clip = song;
        _musicVolumeManager.AudioSource.volume = 0f; // Always set volume to 0 BEFORE playing the audio source
        _musicVolumeManager.AudioSource.Play();
    }
}
