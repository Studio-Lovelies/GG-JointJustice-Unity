using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AppearingDialogueController : MonoBehaviour, IAppearingDialogueController
{
    [Header("Basic Values")]

    [SerializeField, ReadOnly]
    private float _currentAppearTime = 0;
    [SerializeField, Tooltip("DirectionActionDecoder this script is connected to.")]
    DirectorActionDecoder _directorActionDecorder = null;
    [SerializeField, Tooltip("Drag the game object containing the text box here.")]
    private GameObject _textBoxGameObject;
    [SerializeField, Tooltip("TextMeshPro-component all the dialog should appear in.")]
    private TextMeshProUGUI _controlledText = null;
    [SerializeField, Tooltip("TextMeshPro-component the name of the speaker should appear in.")]
    private TextMeshProUGUI _nameText = null;
    [SerializeField, Tooltip("Image containing text background color")]
    private Image _nameBackgroundImage = null;
    [SerializeField, Tooltip("Default waiting time for all letters, if no alterations have been made in dialog."), Min(0f)]
    private float _defaultAppearTime = 0.01f;
    [SerializeField, Tooltip("Default waiting time for punctuation, if no alterations have been made in dialog."), Min(0f)]
    private float _defaultPunctuationAppearTime = 0.02f;
    [SerializeField, Tooltip("When player is giving correct input to the game, how much faster should the game go.")]
    private float _speedMultiplierFromPlayerInput = 2;

    [SerializeField, Tooltip("ActorData that is used to hide the spoken actor.")]
    private ActorData _actorDataHiddenActor;

    [Header("Events")]
    [SerializeField, Tooltip("Events that should happen before the dialog start.")]
    private UnityEvent _dialogueStartEvent = new UnityEvent();
    [SerializeField, Tooltip("Events that should happen after completing a dialog.")]
    private UnityEvent _dialogueEndEvent = new UnityEvent();

    [SerializeField, Tooltip("Event is invoked every time letter appears")]
    private UnityEvent _onLetterAppear = new UnityEvent();

    [SerializeField, Tooltip("Event is invoked when dialog ends while autoskip is still true. Should be made to start the next dialog immediatly")]
    private UnityEvent _onAutoSkip = new UnityEvent();
    private string _currentDialog = "";
    private int _currentLetterNum = 0;
    private float _timer = 0;
    private bool _writingDialog = false;
    private bool _initialSetupDone = false;
    private bool _speedupText = false;
    private Dictionary<WaiterType, WaitInformation> _allWaiters = new Dictionary<WaiterType, WaitInformation>();
    private Dictionary<int, string> _allDialogCommands = new Dictionary<int, string>();
    private const char _commandCharacter = '@';
    private bool _disableTextSkipping = false;
    private bool _continueDialog = false;
    private bool _autoSkipDialog = false;

    public bool PrintTextInstantly { get; set; }

    ///<summary>
    ///Awake is called automatically in the game when object is created, before Start-function.
    ///</summary>
    private void Awake()
    {
        InitialSetup();
    }

    ///<summary>
    ///Start is called automatically in the game when object is created, after Awake but before the first Update.
    ///</summary>
    private IEnumerator Start()
    {
        yield return null;
        _controlledText.enableAutoSizing = false;
        _nameText.enableAutoSizing = false;
    }

    ///<summary>
    ///Initial setup of the class that should happen only once.
    ///</summary>
    private void InitialSetup()
    {
        if (_initialSetupDone)
        {
            return;
        }

        _initialSetupDone = true;

        if (_directorActionDecorder == null)
        {
            Debug.LogError("DirectorActionDecoder has not been set. Please set it before continuing.");
            return;
        }

        _directorActionDecorder.Decoder.AppearingDialogueController = this;

        if (_controlledText == null)
        {
            Debug.LogError("Controlled text shouldn't be null. Please assign it before continuing.");
            return;
        }

        //Setting up basic values for different waiters
        foreach (WaiterType wt in (WaiterType[])System.Enum.GetValues(typeof(WaiterType)))
        {
            WaitInformation.UseCase useCase = WaitInformation.UseCase.OnlyWithLetters;

            if (wt == WaiterType.Letter)
            {
                useCase = WaitInformation.UseCase.WithEverything;
            }
            else if (wt == WaiterType.DefaultPunctuation || wt == WaiterType.Punctuation)
            {
                useCase = WaitInformation.UseCase.OnlyWithPunctuations;
            }

            _allWaiters.Add(wt, new WaitInformation(_defaultAppearTime, useCase));
        }

        _allWaiters[WaiterType.DefaultPunctuation].waitTime = _defaultPunctuationAppearTime;

        //If the textbox is using autosize, lets take if off or else the text will keep changing size when more dialog appears. 
        _currentAppearTime = _defaultAppearTime;
    }

    ///<summary>
    ///Restart all values to default when starting to write new dialog.
    ///</summary>
    ///<param name = "dialog">String containing the dialog to write on screen.</param>
    private void StartDialogSetup(string dialog)
    {
        //Diabled prototype. Take out from comment to enable.
        //_currentDialog = ReadCommands(dialog);

        //If we continue dialog, all values should already be correct. 
        if (_continueDialog)
        {
            _currentDialog += dialog;
            _continueDialog = false;
        }
        else
        {
            _currentDialog = dialog;
            _currentLetterNum = 0;
            _controlledText.maxVisibleCharacters = 0;
        }

        //Take out new lines
        _currentDialog = _currentDialog.Replace("\n", " ");

        _writingDialog = true;
        _controlledText.text = _currentDialog;

        _dialogueStartEvent.Invoke();
    }

    ///<summary>
    /// Displays dialog one letter at a time.
    ///</summary>
    /// <param name="dialog">The dialog that should be showed to player.</param>
    public IEnumerator StartDialogCoroutine(string dialog)
    {
        StartDialogSetup(dialog);

        if (PrintTextInstantly)
        {
            _controlledText.maxVisibleCharacters = _controlledText.text.Length;
            _dialogueEndEvent.Invoke();
            PrintTextInstantly = false;
            yield break;
        }

        while (_writingDialog)
        {
            WriteDialog();
            yield return null;
        }
    }

    ///<summary>
    /// Displays dialog one letter at a time.
    ///</summary>
    /// <param name="dialog">The dialog that should be showed to player.</param>
    public void StartDialog(string dialog)
    {
        if (!_textBoxGameObject.activeSelf)
        {
            _textBoxGameObject.SetActive(true);
        }

        StartCoroutine(StartDialogCoroutine(dialog));
    }


    ///<summary>
    /// Contains all the actions that should happen when all of the dialog has been showed.
    ///</summary>
    private void EndDialog()
    {
        //Disable the code from continuing writing
        _writingDialog = false;

        //Invoke all functions set to _dialogDoneEvent set either in editor or by code.
        _dialogueEndEvent.Invoke();

        if (_autoSkipDialog)
        {
            _onAutoSkip.Invoke();
        }
    }

    ///<summary>
    ///Is called every frame when dialog is written, and keeps track when to write the next letter.
    ///</summary>
    private void WriteDialog()
    {
        //Multiply the increasing time if player is giving input to make the text appear faster.
        if (_speedupText && !_disableTextSkipping)
        {
            _timer += Time.deltaTime * _speedMultiplierFromPlayerInput;
        }
        else
        {
            _timer += Time.deltaTime;
        }

        if (_timer >= _currentAppearTime)
        {
            NextLetter();
        }
    }

    ///<summary>
    /// Will check and calculate all needed things for the next letter, like any extra actions embedded to dialog that should happen.
    ///</summary>
    private void NextLetter()
    {
        _timer = 0;

        //Increase the maxVisibleCharacters to show the next letter.
        _controlledText.maxVisibleCharacters = _currentLetterNum;
        _onLetterAppear.Invoke();

        //If the end of dialog is reached, make appropriate measures.
        if (_currentDialog.Length == _currentLetterNum)
        {
            EndDialog();
            return;
        }

        //Diabled prototype. Take out from comment to enable.
        //CheckCommands();

        _currentLetterNum++;

        //Make sure the _currentLetterNum isn't going to be too small.
        if (_currentLetterNum >= 2)
        {
            //_currentLetterNum-2 is to make the text wait longer AFTER the punctuation has been written, rather it taking long time for the punctuation itself to appear.
            WaiterType newWaiter = GetCurrentWaiter(_currentDialog[_currentLetterNum - 2]);

            if (newWaiter == WaiterType.Letter)
            {
                _allWaiters[WaiterType.Letter].inUse = false;
            }

            _currentAppearTime = _allWaiters[newWaiter].waitTime;
        }
        else
        {
            _currentAppearTime = 0;
        }
    }

    ///<summary>
    ///This should be called when player starts and stops giving wanted input to speed up or normalize the dialog appear time.
    ///</summary>
    ///<param name = "buttonIsDown">Is the button pressed down or up.</param>
    public void SpeedupText(bool speedUp)
    {
        _speedupText = speedUp;
    }

    ///<summary>
    ///Checks all different conditions to see which timer to use.
    ///</summary>
    ///<param name = "characterToAppear">What is the next character we are checking the timer for. Mainly used to see if its character or punctuation.</param>
    ///<returns>Returns which kind of waiter is used for the next character.</returns>
    private WaiterType GetCurrentWaiter(char characterToAppear)
    {
        bool isPunctuation = characterToAppear == '.' || characterToAppear == '?' || characterToAppear == '!';
        foreach (WaiterType wt in _allWaiters.Keys)
        {
            if (_allWaiters[wt].inUse)
            {
                if ((isPunctuation && _allWaiters[wt].useCase == WaitInformation.UseCase.OnlyWithPunctuations) ||
                (!isPunctuation && _allWaiters[wt].useCase == WaitInformation.UseCase.OnlyWithLetters) ||
                _allWaiters[wt].useCase == WaitInformation.UseCase.WithEverything)
                {
                    return wt;
                }
            }
        }

        return isPunctuation ? WaiterType.DefaultPunctuation : WaiterType.DefaultValue;
    }

    ///<summary>
    ///Clears all waiters, so the default is used.
    ///</summary>
    public void ClearAllWaiters()
    {
        foreach (WaitInformation wi in _allWaiters.Values)
        {
            wi.inUse = false;
        }
        _disableTextSkipping = false;
    }

    ///<summary>
    ///Toggle should the player be able to speed up the text.
    ///</summary>
    ///<param name = "b">The bool value that is set to disabling the speeding text.</param>
    public void ToggleDisableTextSkipping(bool disabled)
    {
        _disableTextSkipping = disabled;
    }

    ///<summary>
    ///Checks how long the number continues from the start for string.
    ///</summary>
    /// <param name="dialogText">The dialog we check the number from.</param>
    /// <param name="previousTime">How fast the dialog was going previously, in case the number should increase or decrease the current speed.</param>
    /// <param name="newAppearTime">Gives out the new float containing new appear time rule.</param>
    /// <returns>Returns true if numeric value was retrieved, or false it there was an error or the command didn't contain numeric value.</returns>
    static private bool GetFloatFromString(string dialogText, float previousTime, out float newAppearTime)
    {
        newAppearTime = 0;
        //If the string starts with either + or -, this value gotten from it will be used to increase or decreaset he current speed.
        bool increase = dialogText[0] == '+';
        bool decrease = dialogText[0] == '-';

        //If either increase or decrease were found, it will be cut from number conversion. 
        if (increase || decrease)
            dialogText = dialogText.Substring(1);

        if (!float.TryParse(dialogText, NumberStyles.Any, CultureInfo.InvariantCulture, out float numericValue))
        {
            Debug.LogWarning("After given command a number was expected, but was not found. Please fix.");
            return false;
        }

        if (increase)
            numericValue += previousTime;
        else if (decrease)
            numericValue = previousTime - numericValue;

        newAppearTime = numericValue;
        return true;
    }

    ///<summary>
    ///Read the commands from dialog and separates them. Prototype, currently not in use.
    ///</summary>
    ///<param name = "dialog">Current dialog to check the commands from.</param>
    ///<returns>Dialog where commands have been separated.</returns>
    private string ReadCommands(string dialog)
    {
        //Clear past commands
        _allDialogCommands.Clear();
        bool insideCommand = false;
        string currentCommand = "";
        string textWithoutCommands = "";
        int startCommand = 0;

        for (int i = 0; i < dialog.Length; i++)
        {
            //Check if the command character is found.
            if (dialog[i] == _commandCharacter)
            {
                insideCommand = !insideCommand;

                if (insideCommand)
                {
                    startCommand = i;
                }
                else
                {
                    _allDialogCommands.Add(textWithoutCommands.Length, currentCommand);
                    currentCommand = "";
                }
            }
            else if (insideCommand)
            {
                //While inside a command, add characters to command string.
                currentCommand += dialog[i];
            }
            else
            {
                //While outside of a command, add characters to actual dialog string.
                textWithoutCommands += dialog[i];
            }
        }

        //If the dialog stops while we are still inside the command,it means it wasn't a command.
        if (insideCommand)
        {
            //Add the character from the mistaken command-character to the end to the actual dialog.
            textWithoutCommands += dialog.Substring(startCommand);
        }

        return textWithoutCommands;
    }

    ///<summary>
    ///Checks the commands for current dialog. Prototype, currently not in use.
    ///</summary>
    private void CheckCommands()
    {
        if (!_allDialogCommands.ContainsKey(_currentLetterNum))
        {
            return;
        }

        string currentCommand = _allDialogCommands[_currentLetterNum];
        int currentLetterNum = 0;
        WaiterType newType = WaiterType.DefaultValue;

        //Checking what the next letter is to know what action to take.
        switch (currentCommand[currentLetterNum])
        {
            //S for Speeding
            case 's':
            case 'S':
                _disableTextSkipping = true;
                break;
            //L for letter
            case 'l':
            case 'L':
                newType = WaiterType.Letter;
                break;
            //P for punctuation
            case 'p':
            case 'P':
                newType = WaiterType.Punctuation;
                break;
            //D for dialog
            case 'd':
            case 'D':
                newType = WaiterType.Dialog;
                break;
            //A for all
            case 'a':
            case 'A':
                newType = WaiterType.Overall;
                break;
            //C for clear
            case 'c':
            case 'C':
                ClearAllWaiters();
                break;
            default:
                Debug.Log("Command not recognized.");
                return;
        }

        if (newType != WaiterType.DefaultValue)
        {
            SetTimerValue(newType, currentCommand.Substring(1));
        }
    }

    ///<summary>
    ///Sets timer value to given float.
    ///</summary>
    ///<param name = "waiterTypeToChange">What type of waiter should be changed.</param>
    ///<param name = "valueToTurnFloat">string containing the numeric value of new appear time.</param>
    public void SetTimerValue(WaiterType waiterTypeToChange, string valueToTurnFloat)
    {
        //Check that there was a number after command we could use as new appear time.
        if (GetFloatFromString(valueToTurnFloat, _allWaiters[waiterTypeToChange].waitTime, out float newAppearTime))
        {
            SetTimerValue(waiterTypeToChange, newAppearTime);
        }
        else
        {
            Debug.LogError("Was expecting numeric value, but command didn't contain it.");
        }
    }


    ///<summary>
    ///Sets timer value to given float.
    ///</summary>
    ///<param name = "waiterTypeToChange">What type of waiter should be changed.</param>
    ///<param name = "newAppearTime">float value that will be set as the waiters ne appear time.</param>
    public void SetTimerValue(WaiterType waiterTypeToChange, float newAppearTime)
    {
        _allWaiters[waiterTypeToChange].inUse = true;
        _allWaiters[waiterTypeToChange].waitTime = newAppearTime;
    }

    ///<summary>
    ///Should the next speaken dialog continue after this.
    ///</summary>
    public void ContinueDialog()
    {
        _continueDialog = true;
    }

    ///<summary>
    ///Should the next speaken dialog continue after this.
    ///</summary>
    ///<param name = "skip">Boolean telling if autoskip is on or off.</param>
    public void AutoSkipDialog(bool skip)
    {
        _autoSkipDialog = skip;
    }

    /// <summary>
    /// Sets the speaker name and bg color based on provided actor data.
    /// </summary>
    /// <param name="actor">Actor data to base the changes on.</param>
    public void SetActiveSpeaker(ActorData actor)
    {
        if (actor == _actorDataHiddenActor)
        {
            _nameBackgroundImage.gameObject.SetActive(false);
            return;
        }

        _nameBackgroundImage.gameObject.SetActive(true);
        ChangeSpeakerName(actor.DisplayName);
        ChangeNameBGColor(actor.DisplayColor);
    }

    ///<summary>
    ///Changes the name of the speaker.
    ///</summary>
    ///<param name = "newName">The name of the new speaker.</param>
    private void ChangeSpeakerName(string newName)
    {
        _nameText.text = newName;
    }

    ///<summary>
    ///Changes the names background color to wanted.
    ///</summary>
    ///<param name = "newColor">The new color of the background image.</param>
    private void ChangeNameBGColor(Color newColor)
    {
        _nameBackgroundImage.color = newColor;
    }

    /// <summary>
    /// Hides the dialogue textbox.
    /// </summary>
    public void HideTextbox()
    {
        _textBoxGameObject.SetActive(false);
    }

    ///<summary>
    ///Small class containing if the current WaitTimer is in use, and how long to wait if it is.
    ///</summary>
    private class WaitInformation
    {
        public enum UseCase
        {
            OnlyWithLetters,
            OnlyWithPunctuations,
            WithEverything
        }

        public WaitInformation(float waitTime, UseCase useCase)
        {
            this.waitTime = waitTime;
            this.useCase = useCase;
        }

        //Is this WaitInformation in use now.
        public bool inUse = false;
        //What is the current wait time of this information.
        public float waitTime = 0;
        public UseCase useCase { get; private set; }
    }
}
