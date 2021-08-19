using UnityEngine;

public class MusicPlayer
{
    public float NormalizedVolume { get; private set; }

    public WaitUntil FadeInCurrentMusicTrack(float deltaTime)
    {
        return new WaitUntil(() =>
        {
            NormalizedVolume += deltaTime;

            if (NormalizedVolume >= 1f)
            {
                NormalizedVolume = 1f;
                return true;
            }
            else
            {
                return false;
            }
        });
    }

    public WaitUntil FadeOutCurrentMusicTrack(float deltaTime)
    {
        return new WaitUntil(() =>
        {
            NormalizedVolume -= deltaTime;

            if (NormalizedVolume <= 0)
            {
                NormalizedVolume = 0;
                return true;
            }
            else
            {
                return false;
            }
        });
    }
}
