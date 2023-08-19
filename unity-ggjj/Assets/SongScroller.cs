using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongScroller : MonoBehaviour
{
    private const int CharacterMaximum = 27;
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private TextMeshProUGUI _textMeshProUGUI;

    public Animator _animator;
    private string _fullTrackName;

    public void ShowTrack(string fullTrackName)
    {
        _fullTrackName = fullTrackName;
        _textMeshProUGUI.text = GenerateSubstringString(0);
    }
    
    public void StartScrolling()
    {
        StartCoroutine(Scroll());
    }

    private string GenerateSubstringString(int offset)
    {
        var content = _fullTrackName.Substring(offset, Math.Min(CharacterMaximum, _fullTrackName.Length - offset));
        content = content.PadRight(CharacterMaximum, ' ');
        return content;
    }
    
    private IEnumerator Scroll()
    {
        var currentIndex = 0;
        do
        {
            _textMeshProUGUI.text = GenerateSubstringString(currentIndex);
            currentIndex += 1;
            yield return new WaitForSeconds(0.2f);
            Debug.Log("Scrolling: " + currentIndex + " / " + _textMeshProUGUI.text.Length + ": " + _textMeshProUGUI.text);
        } while (currentIndex < _fullTrackName.Length - (CharacterMaximum - 1));
        
        yield return new WaitForSeconds(1.5f);
        
        //
        // var startTime = Time.time;
        // var endTime = startTime + 5f;
        // var start = 0f;
        // // set end to 1f + normalized width of this objects calculated rect transform width
        // var end = 1f;
        // while (Time.time < endTime)
        // {
        //     var t = (Time.time - startTime) / (endTime - startTime);
        //     _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, end, t * t * (3f - 2f * t));
        //     yield return null;
        // }
        _animator.Play("SlideOut");
    }
}
