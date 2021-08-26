using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class AppearingDialogController : MonoBehaviour
{
    [Header("Basic Values")]
    [SerializeField, Tooltip("TextMeshPro-component all the dialog should appear in.")]
    private TextMeshProUGUI _controlledText = null;
    [SerializeField, Tooltip("TextMeshPro-component the name of the speaker should appear in.")]
    private TextMeshProUGUI _nameText = null;
    [SerializeField, Tooltip("Image containing text background color")]
    private Image _nameBackgroundImage = null;
    [SerializeField, Tooltip("Default waiting time for all letters, if no alterations have been made in dialog."), Min(0f)]
    private float _defaultAppearTime = 0;
    [SerializeField, Tooltip("Default waiting time for punctuation, if no alterations have been made in dialog."), Min(0f)]
    private float _defaultPunctuationAppearTime = 0;
    [SerializeField, Tooltip("When player is giving correct input to the game, how much faster should the game go.")]
    private float _speedMultiplierFromPlayerInput = 2;

    [Header("Events")]
    [SerializeField, Tooltip("Events that should happen after completing a dialog.")]
    private UnityEvent _dialogDoneEvent = new UnityEvent();
    ///<summary>
    ///Events that should happen after completing a dialog. You have to manually add and remove listeners, and they will happen every time dialog is complete.
    ///</summary>
    public UnityEvent DialogDoneEvent { get { return _dialogDoneEvent; } }

    [SerializeField, Tooltip("Event is invoked every time letter appears")]
    private UnityEvent _onLetterAppear = new UnityEvent();

    private UnityEvent _temporaryDialogDoneEvent = new UnityEvent();

    private float _currentAppearTime = 0;
    private string _currentDialog = "";
    private int _currentLetterNum = 0;
    private float _timer = 0;
    private bool _writingDialog = false;
    private bool _initialSetupDone = false;
    private bool _playerIsGivingInput = false;
    private Dictionary<WaiterTypes, WaitInformation> _allWaiters = new Dictionary<WaiterTypes, WaitInformation>();
    private Dictionary<int, string> _allDialogCommands = new Dictionary<int, string>();
    private const char _commandCharacter = '@';
    private bool _disableTextSkipping = false;
    private bool _continueDialog = false;


    ///<summary>
    ///Awake is called automatically in the game when object is created, before Start-function.
    ///</summary>
    private void Awake()
    {
        InitialSetup();
    }

    ///<summary>
    ///Initial setup of the class that should happen only once.
    ///</summary>
    private void InitialSetup()
    {
        if (_initialSetupDone)
            return;

        _initialSetupDone = true;

        if (_controlledText == null)
        {
            Debug.LogError("Controlled text shouldn't be null. Please asssign it before continuing.");
            return;
        }

        //Setting up basic values for different waiters
        foreach (WaiterTypes wt in (WaiterTypes[])System.Enum.GetValues(typeof(WaiterTypes)))
        {
            bool useWithPunctuations = wt == WaiterTypes.punctuation ||
            wt == WaiterTypes.defaultPunctuation ||
            wt == WaiterTypes.letter;

            _allWaiters.Add(wt, new WaitInformation(_defaultAppearTime));
        }

        _allWaiters[WaiterTypes.letter].useWithEverything = true;
        _allWaiters[WaiterTypes.punctuation].useOnlyWithPunctuations = true;
        _allWaiters[WaiterTypes.defaultPunctuation].waitTime = _defaultPunctuationAppearTime;

        //If the textbox is using autosize, lets take if off or else the text will keep changing size when more dialog appears. 
        _controlledText.enableAutoSizing = false;
        _nameText.enableAutoSizing = false;
        _currentAppearTime = _defaultAppearTime;
    }

    ///<summary>
    ///Restart all values to default when starting to write new dialog.
    ///</summary>
    ///<param name = "dialog">String containing the dialog to write on screen</param>
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
    }

    ///<summary>
    /// Displays dialog one letter at a time.
    ///</summary>
    /// <param name="dialog">The dialog that should be showed to player.</param>
    /// <param name="dialogDoneActions">All the actions that should be called after the dialog has written everything. These actions happen only once, and will be removed afterwards.</param>
    public IEnumerator StartDialogCoroutine(string dialog, params UnityAction[] dialogDoneActions)
    {
        foreach (UnityAction ua in dialogDoneActions)
        {
            _temporaryDialogDoneEvent.AddListener(ua);
        }

        yield return StartDialogCoroutine(dialog);

        _temporaryDialogDoneEvent?.Invoke();
        _temporaryDialogDoneEvent.RemoveAllListeners();
    }

    ///<summary>
    /// Displays dialog one letter at a time.
    ///</summary>
    /// <param name="dialog">The dialog that should be showed to player.</param>
    public IEnumerator StartDialogCoroutine(string dialog)
    {
        StartDialogSetup(dialog);

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
        _dialogDoneEvent?.Invoke();
    }

    ///<summary>
    ///Is called every frame when dialog is written, and keeps track when to write the next letter.
    ///</summary>
    private void WriteDialog()
    {
        //Multiply the increasing time if player is giving input to make the text appear faster.
        if (_playerIsGivingInput && !_disableTextSkipping)
            _timer += Time.unscaledDeltaTime * _speedMultiplierFromPlayerInput;
        else
            _timer += Time.unscaledDeltaTime;

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
        _onLetterAppear?.Invoke();

        //If the end of dialog is reached, make appropriate measures
        if (_currentDialog.Length == _currentLetterNum)
        {
            EndDialog();
            return;
        }

        //Diabled prototype. Take out from comment to enable.
        //CheckCommands();

        _currentLetterNum++;

        WaiterTypes newWaiter = GetCurrentWaiter(_currentDialog[_currentLetterNum - 1]);

        if (newWaiter == WaiterTypes.letter)
            _allWaiters[WaiterTypes.letter].inUse = false;

        _currentAppearTime = _allWaiters[newWaiter].waitTime;

    }

    ///<summary>
    ///This should be called when player starts and stops giving wanted input to speed up or normalize the dialog appear time.
    ///</summary>
    ///<param name = "buttonIsDown">Is the button pressed down or up</param>
    public void PlayerInput(bool buttonIsDown)
    {
        _playerIsGivingInput = buttonIsDown;
    }

    ///<summary>
    ///Checks all different conditions to see which timer to use
    ///</summary>
    ///<param name = "characterToAppwar">What is the next character we are checking the timer for. Mainly used to see if its character or punctuation</param>
    ///<returns>Returns which kind of waiter is used for the next character</returns>
    private WaiterTypes GetCurrentWaiter(char characterToAppear)
    {
        bool isPunctuation = !char.IsLetterOrDigit(characterToAppear) && characterToAppear != ' ';

        foreach (WaiterTypes wt in _allWaiters.Keys)
        {
            if ((_allWaiters[wt].useWithEverything ||
            _allWaiters[wt].useOnlyWithPunctuations == isPunctuation) &&
            _allWaiters[wt].inUse)
                return wt;
        }

        return isPunctuation ? WaiterTypes.defaultPunctuation : WaiterTypes.defaultValue;
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
    public void ToggleDisableTextSkipping()
    {
        ToggleDisableTextSkipping(!_disableTextSkipping);
    }

    ///<summary>
    ///Toggle should the player be able to speed up the text.
    ///</summary>
    ///<param name = "b">The bool value that is set to disabling the speeding text</param>
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
        //String gathering the information converted to float.
        string toFloat = "";
        //The current letters position being examined.
        int currentLetter = 0;
        //If the string starts with either + or -, this value gotten from it will be used to increase or decreaset he current speed.
        bool increase = dialogText[0] == '+';
        bool decrease = dialogText[0] == '-';

        //If either increase or decrease were found, it will be cut from number conversion. 
        if (increase || decrease)
            dialogText = dialogText.Substring(1);

        //The code will check letter by letter if the number continues, or has decimal point/comma.
        while (currentLetter < dialogText.Length)
        {
            char currentCharacter = dialogText[currentLetter];

            if (char.IsDigit(currentCharacter) || currentCharacter == '.' || currentCharacter == ',')
                toFloat += currentCharacter;
            else
                break;

            currentLetter++;
        }

        //If the string containing all checked letters length is 0, then conversion to number is impossible and rest of the code is ignored.
        if (toFloat.Length == 0)
        {
            Debug.LogWarning("After given command a number was expected, but was not found. Please fix.");
            return false;
        }

        float f = float.Parse(toFloat);

        if (increase)
            f += previousTime;
        else if (decrease)
            f = previousTime - f;

        newAppearTime = f;
        return true;
    }

    ///<summary>
    ///Read the commands from dialog and seperates them. Prototype, currently not in use.
    ///</summary>
    ///<param name = "dialog">Current dialog to check the commands from</param>
    ///<returns>Dialog where commands have been separated</returns>
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
            //Check if the command character is found
            if (dialog[i] == _commandCharacter)
            {
                insideCommand = !insideCommand;

                if (insideCommand)
                {
                    startCommand = i;
                }
                else if (!insideCommand)
                {
                    _allDialogCommands.Add(textWithoutCommands.Length, currentCommand);
                    currentCommand = "";
                }
            }
            else if (insideCommand)
            {
                //While inside a command, add characters to command string
                currentCommand += dialog[i];
            }
            else
            {
                //While outside of a command, add characters to actual dialog string
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
            return;

        string currentCommand = _allDialogCommands[_currentLetterNum];
        int currentLetterNum = 0;
        WaiterTypes newType = WaiterTypes.defaultValue;

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
                newType = WaiterTypes.letter;
                break;
            //P for punctuation
            case 'p':
            case 'P':
                newType = WaiterTypes.punctuation;
                break;
            //D for dialog
            case 'd':
            case 'D':
                newType = WaiterTypes.dialog;
                break;
            //A for all
            case 'a':
            case 'A':
                newType = WaiterTypes.overall;
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


        if (newType != WaiterTypes.defaultValue)
        {
            SetTimerValue(newType, currentCommand.Substring(1));
        }
    }

    ///<summary>
    ///Sets timer value to given float.
    ///</summary>
    ///<param name = "waiterTypeToChange">What type of waiter should be changed</param>
    ///<param name = "valueToTurnFloat">string containing the numeric value of new appear time</param>
    public void SetTimerValue(WaiterTypes waiterTypeToChange, string valueToTurnFloat)
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
    ///<param name = "waiterTypeToChange">What type of waiter should be changed</param>
    ///<param name = "newAppearTime">float value that will be set as the waiters ne appear time.</param>
    public void SetTimerValue(WaiterTypes waiterTypeToChange, float newAppearTime)
    {
        _allWaiters[waiterTypeToChange].inUse = true;
        _allWaiters[waiterTypeToChange].waitTime = newAppearTime;
    }

    ///<summary>
    ///Should the next speaken dialog continue after this
    ///</summary>
    public void ContinueDialog()
    {
        _continueDialog = true;
    }


    ///<summary>
    ///Small class containing if the current WaitTimer is in use, and how long to wait if it is.
    ///</summary>
    private class WaitInformation
    {
        public WaitInformation() { }

        public WaitInformation(float waitTime)
        {
            this.waitTime = waitTime;
        }

        //Is this WaitInformation in use now.
        public bool inUse = false;
        //What is the current wait time of this information.
        public float waitTime = 0;
        //Is this information used only with punctuations.
        public bool useOnlyWithPunctuations = false;
        //Is this information used with everything.
        public bool useWithEverything = false;
    }
}
