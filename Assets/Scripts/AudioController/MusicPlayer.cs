using UnityEngine;

public class MusicPlayer
{
    public bool FadeInCurrentMusicTrack(AudioSource source, float deltaTime, float maxVolume)
    {
        source.volume += deltaTime * maxVolume;

        if (source.volume >= maxVolume)
        {
            source.volume = maxVolume;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool FadeOutCurrentMusicTrack(AudioSource source, float deltaTime, float maxVolume)
    {
        source.volume -= deltaTime * maxVolume;

        if (source.volume <= 0)
        {
            source.volume = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
