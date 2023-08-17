using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AppearingDialogueController : MonoBehaviour, IAppearingDialogueController
{
    [SerializeField] private NarrativeGameState _narrativeGameState;

    [Tooltip("Drag a NameBox component here.")]
    [SerializeField] private NameBox _namebox;
    
    [Tooltip("Drag a TextMeshProUGUI component here.")]
    [SerializeField] private TextMeshProUGUI _textBox;

    [Tooltip("Drag the speech panel game object here")]
    [SerializeField] private GameObject _speechPanel;
    
    [field: Tooltip("The time in seconds after a character appears and before the next >non-punctuation< character appears.")]
    [field: SerializeField] public float CharacterDelay { get; set; }

    [field: Tooltip("The time in seconds after a character appears and before the next >punctuation< character appears.")]
    [field: SerializeField] public float DefaultPunctuationDelay { get; set; }

    [field: Tooltip("The time in seconds after a character appears and before the next character appears when the player is requesting a text speedup.")]
    [field: SerializeField] public float SpeedupDelay { get; set; }

    [Tooltip("List of characters and a custom delay in seconds after a character appears and before >that specific< character appears.")]
    [SerializeField] private Pair<char, float>[] _specialDelays;

    [Tooltip("Add characters that should be treated like regular characters here.")]
    [SerializeField] private char[] _ignoredCharacters;

    [Tooltip("Add an AudioClip for the default dialogue chirp here")]
    [SerializeField] private AudioClip _defaultDialogueChirpSfx;
    
    [Range(1, 10)]
    [Tooltip("Specify how often a chirp should play here")]
    [SerializeField] private int _chirpEveryNthLetter = 1;

    [Range(0, 1)]
    [Tooltip("Relative volume (0=0% to 1=100%) of the chirp sound when the text-box is sped up")]
    [SerializeField] private float _chirpVolumeDuringSpeedup = 0.18f;

    [SerializeField] private GameObject _continueArrow;

    private TMP_TextInfo _textInfo;
    private Coroutine _printCoroutine;
    private int _chirpIndex;

    private bool _speedupText = false;
    public bool SpeedupText { 
        get => _speedupText;
        set {
            _speedupText = value;
            _narrativeGameState?.AudioController?.SetDialogueChirpVolume(value ? _chirpVolumeDuringSpeedup : 1.0f);
        }
    }
    
    public bool SkippingDisabled { get; set; }
    public bool ContinueDialogue { get; set; }
    public bool AutoSkip { get; set; }
    public bool AppearInstantly { get; set; }
    public int MaxVisibleCharacters => _textBox.maxVisibleCharacters;
    public string Text => _textBox.GetParsedText();
    public bool IsPrintingText { get; private set; }

    public bool TextBoxHidden
    {
        set => _speechPanel.gameObject.SetActive(!value);
    }

    private void Awake()
    {
        _textInfo = _textBox.textInfo;
    }

    /// <summary>
    /// Call this method to start printing text to the dialogue box
    /// </summary>
    /// <param name="text">The text to print.</param>
    public void PrintText(string text)
    {
        StopPrintingText();
        text = text.TrimEnd('\n');
        TextBoxHidden = false;
        _continueArrow.SetActive(false);

        _narrativeGameState.ActorController.StartTalking();
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
            EndLine();
            return;
        }
        
        _printCoroutine = StartCoroutine(PrintTextCoroutine(startingIndex));
    }

    /// <summary>
    /// Stops printing text by stopping _printCoroutine and setting PrintingText to false
    /// </summary>
    public void StopPrintingText()
    {
        if (_printCoroutine == null)
        {
            return;
        }
        
        StopCoroutine(_printCoroutine);
        _printCoroutine = null;
        IsPrintingText = false;
    }

    /// <summary>
    /// Coroutine to print text to the dialogue box over time
    /// </summary>
    private IEnumerator PrintTextCoroutine(int startingIndex)
    {
        IsPrintingText = true;
        for (int i = startingIndex; i < _textInfo.characterCount; i++)
        {
            _textBox.maxVisibleCharacters++;
            var currentCharacterInfo = _textInfo.characterInfo[_textBox.maxVisibleCharacters - 1];
            TryPlayDialogueChirp(_namebox.CurrentActorDialogueChirp, currentCharacterInfo);
            yield return new WaitForSeconds(GetDelay(currentCharacterInfo));
        }
        EndLine();

        if (AutoSkip)
        {
            _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Continue();
        }
    }

    /// <summary>
    /// Increment the chirp index and try to play dialogue chirp sound effect for current actor
    /// - If no chirp is specified, we will play the default chirp.
    /// - If the chirp index is not evenly divided by `_chirpEveryNthLetter` then we play no sound.
    /// - If the character is treated as punctuation, the chirp index is reset
    /// </summary>
    /// <param name="currentActorChirp">Speaker actor's dialogue chirp</param>
    /// <param name="characterInfo">CharacterInfo for the character to play chirp on (skipped if punctuation or ignored)</param>
    private void TryPlayDialogueChirp(AudioClip currentActorChirp, TMP_CharacterInfo characterInfo)
    {
        if (CharShouldBeTreatedAsPunctuation(characterInfo))
        {
            _chirpIndex = 0;
            return;
        }
        
        var resultChirp = currentActorChirp;
        if (resultChirp == null)
        {
            resultChirp = _defaultDialogueChirpSfx;
        }

        if (_chirpIndex % _chirpEveryNthLetter == 0)
        {
            _narrativeGameState.AudioController.PlayDialogueChirp(resultChirp);
        }
        
        _chirpIndex++;
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
        if (!SkippingDisabled)
        {
            if (SpeedupText)
            {
                return SpeedupDelay;
            }
        }

        var character = characterInfo.character;
        
        if (!CharShouldBeTreatedAsPunctuation(characterInfo))
        {
            return CharacterDelay;
        }

        if (_textBox.maxVisibleCharacters == _textInfo.characterCount)
        {
            return 0;
        }
        
        var pair = _specialDelays.FirstOrDefault(charFloatPair => charFloatPair.Item1 == character);
        return (pair.Item1 == '\0' ? DefaultPunctuationDelay : pair.Item2);
    }
    
    /// <param name="characterInfo">Char to be tested</param>
    /// <returns>True if character is ignorable</returns>
    private bool CharShouldBeTreatedAsPunctuation(TMP_CharacterInfo characterInfo)
    {
        return char.IsPunctuation(characterInfo.character) && 
               !_ignoredCharacters.Contains(characterInfo.character) &&
               !_textInfo.linkInfo.Where(info => info.GetLinkID() == "character").Any(linkInfo => _textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex].Equals(characterInfo));
    }

    /// <summary>
    /// Handles what happens when a line ends
    /// </summary>
    private void EndLine()
    {
        _narrativeGameState.ActorController.StopTalking();
        _continueArrow.SetActive(true);
        IsPrintingText = false;
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
