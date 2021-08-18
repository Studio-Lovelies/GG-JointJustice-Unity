using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour, IAudioController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;
    /// <summary>
    /// One day this will come from the "Settings," but for now it lives on a field
    /// </summary>
    [Tooltip("Maximum allowed volume by the game")]
    [Range(0f, 1f)]
    [SerializeField] private float _maxVolume;
    [SerializeField] private bool _isTransitioningMusicTracks;
    private Dictionary<string, AudioSource> _cachedAudioSources = new Dictionary<string, AudioSource>();
    private MusicPlayer _musicPlayer;
    private AudioSource _audioSource;
    private Coroutine _currentFadeCoroutine;

    // Start is called before the first frame update
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

        _audioSource = GetComponent<AudioSource>();
        _musicPlayer = new MusicPlayer();

        // DEBUG
        PlaySong("aBoyAndHisTrial");
        // /DEBUG
    }

    void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySong("aKissFromARose");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            PlaySong("investigationJoonyer");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlaySFX("chug");
        }
        // /DEBUG


        // Kinda clumsy but this way we can ensure the audioSource volume stays in sync with the settings volume.
        // If you change the volume mid-fade then it will land on the correct volume.
        // If you change the volume outside of a fade, the volume will jump to that exact value.
        if (!_isTransitioningMusicTracks)
        {
            _audioSource.volume = _maxVolume;
        }
    }

    public void PlaySong(string songName)
    {
        if (_currentFadeCoroutine != null)
        {
            StopCoroutine(_currentFadeCoroutine);
        }

        _currentFadeCoroutine = StartCoroutine(FadeToNewSong(songName));
    }

    public void PlaySFX(string soundEffectName)
    {
        AudioClip soundEffectClip = Resources.Load("Audio/SFX/" + soundEffectName) as AudioClip;
        if (!_cachedAudioSources.ContainsKey(soundEffectName))
        {
            var audioSourceGameObject = new GameObject(string.Format("SFX_{0}", soundEffectName));
            audioSourceGameObject.transform.SetParent(transform);
            var source = audioSourceGameObject.AddComponent<AudioSource>();
            _cachedAudioSources.Add(soundEffectName, source);
            source.clip = soundEffectClip;
        }

        _cachedAudioSources[soundEffectName].Play();
    }

    public IEnumerator FadeToNewSong(string songName)
    {
        _isTransitioningMusicTracks = true;
        if (IsCurrentlyPlayingSomething())
        {
            yield return new WaitUntil(FadeOutCurrentMusicTrack);
        }

        SetCurrentTrack(songName);
        yield return new WaitUntil(FadeInCurrentMusicTrack);

        _isTransitioningMusicTracks = false;
        yield return null;
    }

    private bool FadeOutCurrentMusicTrack()
    {
        return _musicPlayer.FadeOutCurrentMusicTrack(_audioSource, Time.deltaTime, _maxVolume);
    }

    private bool FadeInCurrentMusicTrack()
    {
        return _musicPlayer.FadeInCurrentMusicTrack(_audioSource, Time.deltaTime, _maxVolume);
    }

    private bool IsCurrentlyPlayingSomething()
    {
        return _audioSource.clip != null && _audioSource.volume != 0;
    }

    private void SetCurrentTrack(string songName)
    {
        _audioSource.clip = Resources.Load("Audio/Music/" + songName) as AudioClip;
        _audioSource.volume = 0f;
        _audioSource.Play();
    }


}
