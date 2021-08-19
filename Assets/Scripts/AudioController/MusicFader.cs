using UnityEngine;

public class MusicFader
{
    public float NormalizedVolume { get; private set; }

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
