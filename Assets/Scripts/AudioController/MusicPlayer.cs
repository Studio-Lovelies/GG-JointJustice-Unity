public class MusicPlayer
{
    public float NormalizedVolume { get; private set; }

    public bool FadeInCurrentMusicTrack(float deltaTime)
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
    }

    public bool FadeOutCurrentMusicTrack(float deltaTime)
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
    }
}
