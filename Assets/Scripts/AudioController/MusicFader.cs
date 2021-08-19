using UnityEngine;

public class MusicFader
{
    /// <summary>
    /// Float from [0..1] that represents the current volume of the music.
    /// </summary>
    public float NormalizedVolume { get; private set; }

    /// <summary>
    /// To be used in a coroutine to fade in the track
    /// </summary>
    /// <returns>WaitUntil object that returns control to the coroutine when it's finished</returns>
    public WaitUntil FadeIn()
    {
        return new WaitUntil(() =>
        {
            NormalizedVolume += Time.deltaTime;

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

    /// <summary>
    /// To be used in a coroutine to fade out the track
    /// </summary>
    /// <returns>WaitUntil object that returns control to the coroutine when it's finished</returns>
    public WaitUntil FadeOut()
    {
        return new WaitUntil(() =>
        {
            NormalizedVolume -= Time.deltaTime;

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
