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
    
    [field: Tooltip("The time after one character appears and before the next character appears.")]
    [field: SerializeField] public float CharacterDelay { get; set; }

    [field: Tooltip("Set the default delay for punctuation characters here.")]
    [field: SerializeField] public float DefaultPunctuationDelay { get; set; }

    [Tooltip("Add punctuation characters and their delay values here.")]
    [SerializeField] private Pair<char, float>[] _punctuationDelay;

    [Tooltip("Add characters that should be treated like regular characters here.")]
    [SerializeField] private char[] _ignoredCharacters;

    [Header("Events")]
    [SerializeField] private UnityEvent _onLineEnd;
    [SerializeField] private UnityEvent _onAutoSkip;
    [SerializeField] private UnityEvent _onLetterAppear;

    private TMP_TextInfo _textInfo;
    private Coroutine _printCoroutine;

    public float SpeedMultiplier { get; set; } = 1;
    public bool SkippingDisabled { get; set; }
    public bool ContinueDialogue { get; set; }
    public bool AutoSkip { get; set; }
    public bool AppearInstantly { get; set; }
    public int MaxVisibleCharacters => _textBox.maxVisibleCharacters;
    public string Text => _textBox.GetParsedText();
    public bool PrintingText { get; private set; }

    public bool TextBoxHidden
    {
        set => _speechPanel.gameObject.SetActive(!value);
    }

    private void Awake()
    {
        _textInfo = _textBox.textInfo;
        _directorActionDecoder.Decoder.AppearingDialogueController = this;
    }

    /// <summary>
    /// Call this method to start printing text to the dialogue box
    /// </summary>
    /// <param name="text">The text to print.</param>
    public void PrintText(string text)
    {
        if (_printCoroutine != null)
        {
            StopCoroutine(_printCoroutine);
            _printCoroutine = null;
            PrintingText = false;
        }

        text = text.TrimEnd('\n');
        TextBoxHidden = false;

        int startingIndex = 0;
        if (ContinueDialogue)
        {
            startingIndex = _textInfo.characterCount;
            _textBox.text = $"{_textBox.text} {text}";
            _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
            ContinueDialogue = false;
            startingIndex -= Mathf.Abs(_textInfo.characterCount - startingIndex);
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
        
        _printCoroutine = StartCoroutine(PrintTextCoroutine(startingIndex));
    }

    /// <summary>
    /// Coroutine to print text to the dialogue box over time
    /// </summary>
    private IEnumerator PrintTextCoroutine(int startingIndex)
    {
        PrintingText = true;
        for (int i = startingIndex; i < _textInfo.characterCount; i++)
        {
            _textBox.maxVisibleCharacters++;
            _onLetterAppear.Invoke();
            char currentCharacter = _textInfo.characterInfo[_textBox.maxVisibleCharacters - 1].character;
            float speedMultiplier = SkippingDisabled ? 1 : SpeedMultiplier;
            if (currentCharacter == '\'')
            {
                Debug.Log(GetDelay(currentCharacter));
            }
            yield return new WaitForSeconds(GetDelay(currentCharacter) / speedMultiplier);
        }
        _onLineEnd.Invoke();

        if (AutoSkip)
        {
            _onAutoSkip.Invoke();
        }

        PrintingText = false;
    }

    /// <summary>
    /// Return a time to wait for a given character.
    /// If character is not found in the punctuation array
    /// (i.e. the default null character is returned from FirstOrDefault)
    /// then the default punctuation delay is returned.
    /// </summary>
    /// <param name="character">The character to get the wait time for.</param>
    /// <returns>The time to wait.</returns>
    public float GetDelay(char character)
    {
        if (!char.IsPunctuation(character) || _ignoredCharacters.Contains(character))
        {
            return CharacterDelay;
        }

        if (_textBox.maxVisibleCharacters == _textInfo.characterCount)
        {
            return 0;
        }
        
        var pair = _punctuationDelay.FirstOrDefault(charFloatPair => charFloatPair.Item1 == character);
        return pair.Item1 == '\0' ? DefaultPunctuationDelay : pair.Item2;
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
