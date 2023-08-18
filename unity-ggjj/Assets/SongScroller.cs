using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongScroller : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _scrollRect;

    public Animator _animator;

    public void StartScrolling()
    {
        StartCoroutine(Scroll());
    }

    // scroll to the right for 5 seconds and lerp with ease-in-out
    private IEnumerator Scroll()
    {
        var startTime = Time.time;
        var endTime = startTime + 5f;
        var start = 0f;
        // set end to 1f + normalized width of this objects calculated rect transform width
        var end = 1f;
        while (Time.time < endTime)
        {
            var t = (Time.time - startTime) / (endTime - startTime);
            _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, end, t * t * (3f - 2f * t));
            yield return null;
        }
        _animator.Play("SlideOut");
    }
}
