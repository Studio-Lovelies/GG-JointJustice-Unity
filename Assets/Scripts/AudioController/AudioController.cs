using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour, IAudioController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;
    /// <summary>
    /// One day this will come from the "Settings," but for now it lives on a field
    /// </summary>
    [Tooltip("Volume level set by player")]
    [Range(0f, 1f)]
    [SerializeField] private float _settingsMusicVolume = 1f;
    [Tooltip("Total duration of fade out + fade in")]
    [Range(0f, 4f)]
    [SerializeField] private float _transitionDuration = 2f;
    private bool _isTransitioningMusicTracks;
    private AudioSource _audioSource;
    private Coroutine _currentFadeCoroutine;
    private MusicFader _musicFader;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Audio Controller doesn't have an action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.SetAudioController(this);
        }

        _musicFader = new MusicFader();

        _audioSource = GetComponent<AudioSource>();
        Debug.Assert(_audioSource != null, "AudioController is missing AudioSource Component");
    }

    /// <summary>
    /// Called every rendered frame
    /// </summary>
    void Update()
    {
        _audioSource.volume = _musicFader.NormalizedVolume * _settingsMusicVolume;
    }

    /// <summary>
    /// Fades in the song with the desired name. Cancels out of a current fade if one is in progress.
    /// </summary>
    /// <param name="songName">Name of song asset, must be in `Resources/Audio/Music`</param>
    public void PlaySong(string songName)
    {
        if (_currentFadeCoroutine != null)
        {
            StopCoroutine(_currentFadeCoroutine);
        }

        _currentFadeCoroutine = StartCoroutine(FadeToNewSong(songName));
    }

    /// <summary>
    /// Plays sound effect of desired name.
    /// </summary>
    /// <param name="soundEffectName">Name of sound effect asset, must be in `Resources/Audio/SFX`</param>
    public void PlaySFX(string soundEffectName)
    {
        AudioClip soundEffectClip = Resources.Load("Audio/SFX/" + soundEffectName) as AudioClip;
        _audioSource.PlayOneShot(soundEffectClip);
    }

    /// <summary>
    /// Coroutine to fade to a new song.
    /// </summary>
    /// <param name="songName">Name of song asset, must be in `Resources/Audio/Music`</param>
    /// <returns></returns>
    public IEnumerator FadeToNewSong(string songName)
    {
        _isTransitioningMusicTracks = true;
        if (IsCurrentlyPlayingMusic())
        {
            yield return _musicFader.FadeOut(_transitionDuration / 2f);
        }

        SetCurrentTrack(songName);
        yield return _musicFader.FadeIn(_transitionDuration / 2f);

        _isTransitioningMusicTracks = false;
    }

    /// <summary>
    /// Are we playing music?
    /// </summary>
    /// <returns>True if we're playing a music track with a volume above zero</returns>
    private bool IsCurrentlyPlayingMusic()
    {
        return _audioSource.clip != null && _settingsMusicVolume != 0;
    }

    /// <summary>
    /// Assigns the music player to a given song with 0 volume, intending to fade it in.
    /// </summary>
    /// <param name="songName">Name of song asset, must be in `Resources/Audio/Music`</param>
    private void SetCurrentTrack(string songName)
    {
        _audioSource.clip = Resources.Load("Audio/Music/" + songName) as AudioClip;
        _audioSource.volume = 0f; // Always set volume to 0 BEFORE playing the audio source
        _audioSource.Play();
    }
}
