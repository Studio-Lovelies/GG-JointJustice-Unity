using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AppearingText : MonoBehaviour
{
    [Tooltip("Drag a TextMeshProUGUI component here.")]
    [SerializeField] private TextMeshProUGUI _textBox;

    [Tooltip("The number of characters that will appear in one second.")]
    [SerializeField] private float _charactersPerSecond;

    [Tooltip("Set the default delay for punctuation characters here.")]
    [SerializeField] private float _defaultPunctutationDelay;

    [Tooltip("Add punctuation characters and their delay values here.")]
    [SerializeField] private Pair<char, float>[] _punctuationDelay;

    [Header("Events")]
    [SerializeField] private UnityEvent _onLineEnd;
    
    private float _characterDelay;

    public float CharactersPerSecond
    {
        set
        {
            _charactersPerSecond = value;
            _characterDelay = 1f / value;
        }
    }

    private void Awake()
    {
        CharactersPerSecond = _charactersPerSecond;
    }

    /// <summary>
    /// Call this method to start printing text to the dialogue box
    /// </summary>
    /// <param name="text">The text to print.</param>
    public void PrintText(string text)
    {
        _textBox.text = text;
        _textBox.maxVisibleCharacters = 0;
        _textBox.ForceMeshUpdate();
        StartCoroutine(PrintTextCoroutine());
    }

    /// <summary>
    /// Coroutine to print text to the dialogue box over time
    /// </summary>
    private IEnumerator PrintTextCoroutine()
    {
        TMP_TextInfo textInfo = _textBox.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            _textBox.maxVisibleCharacters++;
            yield return new WaitForSeconds(GetDelay(textInfo.characterInfo[_textBox.maxVisibleCharacters - 1].character));
        }
        _onLineEnd.Invoke();
    }

    /// <summary>
    /// Return a time to wait for a given character.
    /// </summary>
    /// <param name="character">The character to get the wait time for.</param>
    /// <returns>The time to wait.</returns>
    private float GetDelay(char character)
    {
        if (char.IsPunctuation(character))
        {
            var pair = _punctuationDelay.FirstOrDefault(punctuation => punctuation.Item1 == character);
            return pair.Item1 == '\0' ? _defaultPunctutationDelay : pair.Item2;
        }

        return _characterDelay;
    }
}

/// <summary>
/// Tuple that can be serialized
/// Stores two items with different types.
/// </summary>
[Serializable]
internal struct Pair<T, S>
{
    [field: SerializeField] public T Item1 { get; set; }
    [field: SerializeField] public S Item2 { get; set; }
}
