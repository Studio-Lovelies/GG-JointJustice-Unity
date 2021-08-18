using System.Collections;
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
    private AudioSource _audioSource;
    private Coroutine _currentFadeCoroutine;
    [SerializeField] private bool _isTransitioningMusicTracks;

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
        AudioClip sfxToPlay = Resources.Load("Audio/SFX/" + soundEffectName) as AudioClip;
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

    private bool FadeInCurrentMusicTrack()
    {
        _audioSource.volume += Time.deltaTime * _maxVolume;

        if (_audioSource.volume >= _maxVolume)
        {
            _audioSource.volume = _maxVolume;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool FadeOutCurrentMusicTrack()
    {
        _audioSource.volume -= Time.deltaTime * _maxVolume;

        if (_audioSource.volume <= 0)
        {
            _audioSource.volume = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
