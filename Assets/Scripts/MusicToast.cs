using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the "Now Playing" thing that appears at the top of the screen when the music track changes.
/// 
/// It's called a "Toast" because it pops out like toast from a toaster.
/// </summary>
public class MusicToast : MonoBehaviour
{
    private Vector2 onPosition;
    [SerializeField]
    private Transform offTarget;
    [SerializeField]
    private TMPro.TextMeshProUGUI tmp;
    [SerializeField]
    private float animationDuration = 1f;
    [SerializeField]
    private float lingerTime = 3f;
    private float percent;
    private Vector2 offPosition;

    private Vector2 source;
    private Vector2 destination;
    private bool isAnimating;


    void Start()
    {
        // Assumes the toast starts in the On position
        this.onPosition = this.transform.position;
        // Assumes this field has been provided by the editor
        this.offPosition = this.offTarget.position;

        this.transform.position = this.offPosition;

        AudioManager.instance.SongStarted += DisplayToast;
    }

    private void DisplayToast(string songName)
    {
        StartCoroutine(DisplayCurrentSong(songName));
    }

    void Update()
    {
        if (this.isAnimating)
        {
            this.percent += Time.deltaTime / this.animationDuration;

            this.transform.position = this.source + (this.destination - this.source) * EaseFunction(this.percent);

            if (this.percent >= 1f)
            {
                this.isAnimating = false;
            }
        }
    }

    private bool PercentIsMax()
    {
        return this.percent >= 1f;
    }

    Func<bool> EaseIntoView()
    {
        this.percent = 0f;
        this.source = this.offPosition;
        this.destination = this.onPosition;
        this.isAnimating = true;

        return PercentIsMax;
    }

    Func<bool> EaseOutOfView()
    {
        this.percent = 0f;
        this.source = this.onPosition;
        this.destination = this.offPosition;
        this.isAnimating = true;

        return PercentIsMax;
    }

    IEnumerator DisplayCurrentSong(string songName)
    {
        this.tmp.text = songName;
        yield return new WaitUntil(EaseIntoView());
        yield return new WaitForSecondsRealtime(this.lingerTime);
        yield return new WaitUntil(EaseOutOfView());
    }


    /// <summary>
    /// Ease function that rebounds on its way in and on its way out.
    /// We can easily replace this, I just like this one.
    /// 
    /// Borrowed from https://easings.net/
    /// </summary>
    /// <param name="progress">Percent from [0,1]</param>
    /// <returns></returns>
    private static float EaseFunction(float progress)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return progress < 0.5
          ? ((float) Math.Pow(2 * progress, 2) * ((c2 + 1) * 2 * progress - c2)) / 2
          : ((float) Math.Pow(2 * progress - 2, 2) * ((c2 + 1) * (progress * 2 - 2) + c2) + 2) / 2;
    }
}
