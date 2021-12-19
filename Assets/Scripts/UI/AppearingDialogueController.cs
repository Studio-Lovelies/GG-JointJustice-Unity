using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AppearingDialogueController : MonoBehaviour, IAppearingDialogueController
{
    [Tooltip("Drag a DirectorActionDecoder component here.")]
    [SerializeField] private DirectorActionDecoder _directorActionDecoder;
    
    [Tooltip("Drag a TextMeshProUGUI component here.")]
    [SerializeField] private TextMeshProUGUI _textBox;

    [Tooltip("Drag the speech panel game object here")]
    [SerializeField] private GameObject _speechPanel;
    
    [Tooltip("The number of characters that will appear in one second.")]
    [SerializeField] private float _charactersPerSecond;

    [field: Tooltip("Set the default delay for punctuation characters here.")]
    [field: SerializeField] public float DefaultPunctuationDelay { private get; set; }

    [Tooltip("Add punctuation characters and their delay values here.")]
    [SerializeField] private Pair<char, float>[] _punctuationDelay;

    [Header("Events")]
    [SerializeField] private UnityEvent _onLineEnd;
    [SerializeField] private UnityEvent _onAutoSkip;
    [SerializeField] private UnityEvent _onLetterAppear;
    
    private float _characterDelay;
    private TMP_TextInfo _textInfo;

    public float CharactersPerSecond
    {
        set
        {
            _charactersPerSecond = value;
            _characterDelay = 1f / value;
        }
    }

    public float SpeedMultiplier { get; set; } = 1;
    public bool SkippingDisabled { get; set; }
    public bool ContinueDialogue { get; set; }
    public bool AutoSkip { get; set; }
    public bool AppearInstantly { get; set; }

    public bool TextBoxHidden
    {
        set => _speechPanel.gameObject.SetActive(!value);
    }

    private void Awake()
    {
        _textInfo = _textBox.textInfo;
        _directorActionDecoder.Decoder.AppearingDialogueController = this;
        CharactersPerSecond = _charactersPerSecond;
    }

    /// <summary>
    /// Call this method to start printing text to the dialogue box
    /// </summary>
    /// <param name="text">The text to print.</param>
    public void PrintText(string text)
    {
        text = text.TrimEnd('\n');
        TextBoxHidden = false;

        if (ContinueDialogue)
        {
            _textBox.text = $"{_textBox.text} {text}";
            _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
            ContinueDialogue = false;
        }
        else
        {
            _textBox.text = text;
            _textBox.maxVisibleCharacters = 0;
        }
        
        _textBox.ForceMeshUpdate();
        
        if (AppearInstantly)
        {
            _textBox.maxVisibleCharacters = Int32.MaxValue;
            AppearInstantly = false;
            return;
        }
        
        StartCoroutine(PrintTextCoroutine());
    }

    /// <summary>
    /// Coroutine to print text to the dialogue box over time
    /// </summary>
    private IEnumerator PrintTextCoroutine()
    {
        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            _textBox.maxVisibleCharacters++;
            _onLetterAppear.Invoke();
            char currentCharacter = _textInfo.characterInfo[_textInfo.characterCount - 1].character;
            float speedMultiplier = SkippingDisabled ? 1 : SpeedMultiplier;
            yield return new WaitForSeconds(GetDelay(currentCharacter) / speedMultiplier);
        }
        _onLineEnd.Invoke();

        if (AutoSkip)
        {
            _onAutoSkip.Invoke();
        }
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
            return pair.Item1 == '\0' ? DefaultPunctuationDelay: pair.Item2;
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
