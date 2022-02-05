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

    [Tooltip("Drag an AudioController here.")]
    [SerializeField] private AudioController _audioController;
    
    [Tooltip("Drag a NameBox component here.")]
    [SerializeField] private NameBox _namebox;
    
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

    [Tooltip("Add an AudioClip for the default dialogue chirp here")]
    [SerializeField] private AudioClip _defaultDialogueChirpSfx;
    
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
            _onLineEnd.Invoke();
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
            var currentCharacterInfo = _textInfo.characterInfo[_textBox.maxVisibleCharacters - 1];
            float speedMultiplier = SkippingDisabled ? 1 : SpeedMultiplier;
            PlayDialogueChirp(_namebox.CurrentActorDialogueChirp, currentCharacterInfo);
            yield return new WaitForSeconds(GetDelay(currentCharacterInfo) / speedMultiplier);
        }
        _onLineEnd.Invoke();

        if (AutoSkip)
        {
            _onAutoSkip.Invoke();
        }

        PrintingText = false;
    }

    /// <summary>
    /// Play dialogue chirp sound effect for current actor, if it exists
    /// </summary>
    /// <param name="currentActorChirp">Speaker actor's dialogue chirp</param>
    /// <param name="characterInfo">CharacterInfo for the character to play chirp on (skipped if punctuation or ignored)</param>
    private void PlayDialogueChirp(AudioClip currentActorChirp, TMP_CharacterInfo characterInfo)
    {
        if (CharShouldBeTreatedAsPunctuation(characterInfo))
        {
            return;
        }
        
        var resultChirp = currentActorChirp;
        if (resultChirp == null)
        {
            resultChirp = _defaultDialogueChirpSfx;
        }
        _audioController.PlaySfx(resultChirp);
    }

    /// <summary>
    /// Return a time to wait for a given character.
    /// If character is not found in the punctuation array
    /// (i.e. the default null character is returned from FirstOrDefault)
    /// then the default punctuation delay is returned.
    /// </summary>
    /// <param name="characterInfo">Character info for the character to get the delay for</param>
    /// <returns>The time to wait.</returns>
    public float GetDelay(TMP_CharacterInfo characterInfo)
    {
        var character = characterInfo.character;
        
        if (!CharShouldBeTreatedAsPunctuation(characterInfo))
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
    
    /// <param name="characterInfo">Char to be tested</param>
    /// <returns>True if character is ignorable</returns>
    private bool CharShouldBeTreatedAsPunctuation(TMP_CharacterInfo characterInfo)
    {
        return char.IsPunctuation(characterInfo.character) && 
               !_ignoredCharacters.Contains(characterInfo.character) &&
               !_textInfo.linkInfo.Where(info => info.GetLinkID() == "character").Any(linkInfo => _textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex].Equals(characterInfo));
    }
}
 
/// <summary>
/// Tuple that can be serialized
/// Stores two items with different types.
/// </summary>
[Serializable]
public struct Pair<T, S>
{
    [field: SerializeField] public T Item1 { get; set; }
    [field: SerializeField] public S Item2 { get; set; }
}
