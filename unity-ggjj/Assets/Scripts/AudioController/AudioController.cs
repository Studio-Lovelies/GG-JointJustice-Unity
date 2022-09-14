using System.Collections;
using PlasticGui.WorkspaceWindow;
using UnityEngine;

public class AudioController : MonoBehaviour, IAudioController
{
    [SerializeField] private VolumeManager _musicPrimaryVolumeManager;
    [SerializeField] private VolumeManager _musicSecondaryAVolumeManager;
    [SerializeField] private VolumeManager _musicSecondaryBVolumeManager;

    private bool _isSecondaryAActive = true;
    private VolumeManager ActiveMusicSecondaryManager => _isSecondaryAActive ? _musicSecondaryAVolumeManager : _musicSecondaryBVolumeManager;
    private VolumeManager InactiveSecondaryManager => !_isSecondaryAActive ? _musicSecondaryAVolumeManager : _musicSecondaryBVolumeManager;

    public void ToggleActiveMusicSecondaryManager()
    {
        _isSecondaryAActive = !_isSecondaryAActive;
    }

    [SerializeField] private VolumeManager _sfxVolumeManager;

    [Tooltip("Drag an AudioClip here to be played on scene start")]
    [SerializeField] private AudioClip _defaultSong;
    
    private Coroutine _currentFadeCoroutine;
    /// <summary>

    /// Called when the object is initialized
    /// </summary>
    private void Awake()
    {
        PlayStaticSong(_defaultSong, 0);
        _musicPrimaryVolumeManager.AudioSource.Play();
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
    /// Coroutine to found out a song over a given time
    /// </summary>
    /// <param name="time">The time taken to fade out</param>
    private IEnumerator FadeOutSongCoroutine(float time)
    {
        StartCoroutine(_musicSecondaryAVolumeManager.Fade(0, time));
        StartCoroutine(_musicSecondaryBVolumeManager.Fade(0, time));
        yield return _musicPrimaryVolumeManager.Fade(0, time);
        StopSong();
    }

    /// <summary>
    /// If music is currently playing, stop it!
    /// </summary>
    public void StopSong()
    {
        _musicPrimaryVolumeManager?.AudioSource.Stop();
        _musicSecondaryAVolumeManager?.AudioSource.Stop();
        _musicSecondaryBVolumeManager?.AudioSource.Stop();
        _isSecondaryAActive = true;
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
        return _musicPrimaryVolumeManager.AudioSource.clip != null && _musicPrimaryVolumeManager.AudioSource.volume != 0;
    }

    #region Static songs
    /// <summary>
    /// Overload for PlayStaticSong which allows songs to be played using a direct reference.
    /// </summary>
    /// <param name="song">The song to play.</param>
    /// <param name="transitionTime">The time taken to transition</param>
    public void PlayStaticSong(AudioClip song, float transitionTime)
    {
        if (_currentFadeCoroutine != null)
        {
            StopCoroutine(_currentFadeCoroutine);
        }

        _currentFadeCoroutine = StartCoroutine(FadeToNewStaticSong(song, transitionTime));
    }

    /// <summary>
    /// Coroutine to fade to a new song.
    /// </summary>
    /// <param name="song">The song to fade to</param>
    /// <param name="transitionTime">The time taken to transition</param>
    private IEnumerator FadeToNewStaticSong(AudioClip song, float transitionTime)
    {
        if (IsCurrentlyPlayingMusic())
        {
            yield return FadeOutSongCoroutine(transitionTime / 2f);
        }

        _musicPrimaryVolumeManager.AudioSource.clip = song;
        _musicPrimaryVolumeManager.AudioSource.volume = 0f; // Always set volume to 0 BEFORE playing the audio source
        _musicPrimaryVolumeManager.AudioSource.Play();
        yield return _musicPrimaryVolumeManager.Fade(_musicPrimaryVolumeManager.MaximumVolume, transitionTime / 2f);
    }
    #endregion

    #region Dynamic songs
    /// <summary>
    /// Overload for PlayStaticSong which allows songs to be played using a direct reference.
    /// </summary>
    /// <param name="dynamicMusicData">DynamicMusicData object containing all data of the dynamic song to play.</param>
    /// <param name="variantName">Name of the variant of the DynamicMusicData object to play.</param>
    /// <param name="transitionTime">The time taken to transition</param>
    public void PlayDynamicSong(DynamicMusicData dynamicMusicData, string variantName, float transitionTime)
    {
        if (_currentFadeCoroutine != null)
        {
            StopCoroutine(_currentFadeCoroutine);
        }

        _currentFadeCoroutine = StartCoroutine(FadeToNewDynamicSong(dynamicMusicData.Primary, dynamicMusicData.GetClipForVariant(variantName), transitionTime));
    }

    /// <summary>
    /// Are we playing dynamic music?
    /// </summary>
    /// <returns>True if we're playing a music track with a volume above zero in the active music secondary manager</returns>
    private bool IsCurrentlyPlayingDynamicMusic()
    {
        return ActiveMusicSecondaryManager.AudioSource.clip != null && ActiveMusicSecondaryManager.AudioSource.volume != 0;
    }

    /// <summary>
    /// Coroutine to fade to a new dynamic song.
    /// </summary>
    /// <param name="primaryClip">The song to fade to inside the music primary</param>
    /// <param name="secondaryClip">The song to fade to inside the music secondary</param>
    /// <param name="transitionTime">The time taken to transition</param>
    private IEnumerator FadeToNewDynamicSong(AudioClip primaryClip, AudioClip secondaryClip, float transitionTime)
    {
        if (IsCurrentlyPlayingMusic())
        {
            if (IsCurrentlyPlayingDynamicMusic())
            {
                yield return FadeBetweenDynamicVariant(secondaryClip, transitionTime);
                yield break;
            }

            yield return FadeOutSongCoroutine(transitionTime / 2f);
        }

        _musicPrimaryVolumeManager.AudioSource.clip = primaryClip;
        ActiveMusicSecondaryManager.AudioSource.clip = secondaryClip;

        _musicPrimaryVolumeManager.AudioSource.volume = 0f; // Always set volume to 0 BEFORE playing the audio source
        ActiveMusicSecondaryManager.AudioSource.volume = 0f; // Always set volume to 0 BEFORE playing the audio source

        _musicPrimaryVolumeManager.AudioSource.Play();
        ActiveMusicSecondaryManager.AudioSource.Play();

        StartCoroutine(_musicPrimaryVolumeManager.Fade(_musicPrimaryVolumeManager.MaximumVolume, transitionTime / 2f));
        yield return ActiveMusicSecondaryManager.Fade(ActiveMusicSecondaryManager.MaximumVolume, transitionTime / 2f);
    }

    private IEnumerator FadeBetweenDynamicVariant(AudioClip secondaryClip, float transitionTime)
    {
        InactiveSecondaryManager.AudioSource.clip = secondaryClip;
        InactiveSecondaryManager.AudioSource.volume = 0f;
        InactiveSecondaryManager.AudioSource.timeSamples = ActiveMusicSecondaryManager.AudioSource.timeSamples;
        InactiveSecondaryManager.AudioSource.Play();

        StartCoroutine(InactiveSecondaryManager.Fade(ActiveMusicSecondaryManager.MaximumVolume, transitionTime));
        yield return ActiveMusicSecondaryManager.Fade(0, transitionTime);
        ToggleActiveMusicSecondaryManager();
    }

    #endregion
}
