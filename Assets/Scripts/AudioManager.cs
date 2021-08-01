using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // SINGLETON DEFINITION -----------------------------------------------------------------------------------------------
    // This crap lets the AudioManager exist throughout multiple scenes. Obviously necessary as we'll change scenes
    // multiple times.
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    // MY VARIABLES -------------------------------------------------------------------------------------------------------
    private AudioSource activeSong = null;

    // FUNCTIONS ----------------------------------------------------------------------------------------------------------
    public void PlaySFX(string sfxName, float volume = 1f)
    {
        AudioClip sfxToPlay = Resources.Load("Audio/SFX/" + sfxName) as AudioClip;

        AudioSource source = CreateNewSource(string.Format("SFX[{0}]", sfxName));
        source.clip = sfxToPlay;
        source.volume = volume;

        source.Play();

        Destroy(source.gameObject, sfxToPlay.length < 0.3f ? 0.3f : sfxToPlay.length);
    }

    public void PlaySong(string songName, float volume = 1f, bool loop = true)
    {
        AudioClip songToPlay = Resources.Load("Audio/Music/" + songName) as AudioClip;
        SetActiveSong(songToPlay, volume, loop);
        Play();
    }

    public void SetActiveSong(AudioClip song, float volume = 1f, bool loop = true)
    {
        AudioSource source = CreateNewSource(string.Format("SONG[{0}]", song.name));
        source.clip = song;
        source.volume = volume;
        source.loop = loop;

        this.activeSong = source;
    }

    public void Play()
    {
        this.activeSong.Play();
    }
    public void Stop()
    {
        this.activeSong.Stop();
    }
    public void Pause()
    {
        this.activeSong.Pause();
    }
    public void UnPause()
    {
        this.activeSong.UnPause();
    }

    Coroutine fadingOut = null;
    public void FadeOutActiveSong(float time)
    {
        this.fadingOut = StartCoroutine(FadeOut(this.activeSong, time));
    }
    private IEnumerator FadeOut(AudioSource audio, float time)
    {
        while (audio.volume > 0)
        {
            audio.volume -= 0.01f;
            yield return new WaitForSecondsRealtime(time / 100);
        }

        stopFadeOut();
    }
    private void stopFadeOut()
    {
        StopCoroutine(this.fadingOut);
        this.fadingOut = null;
    }

    public AudioSource CreateNewSource(string _name)
    {
        AudioSource newSource = new GameObject(_name).AddComponent<AudioSource>();
        newSource.transform.SetParent(instance.transform);
        return newSource;
    }
}
