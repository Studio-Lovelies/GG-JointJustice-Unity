using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class AppearingDialogController : MonoBehaviour
{
    [Header("Basic Values")]
    [SerializeField, Tooltip("Test string. Only for testing the tool in editor.")]
    private string _testString = "";
    [SerializeField, Tooltip("TextMeshPro-component all the dialog should appear in.")]
    private TextMeshProUGUI _controlledText = null;
    [SerializeField, Tooltip("Default waiting time for all letters, if no alterations have been made in dialog."), Min(0f)]
    private float _defaultAppearTime = 0;

    [Header("Events")]
    [SerializeField, Tooltip("Events that should happen after completing a dialog.")]
    private UnityEvent _dialogDoneEvent = new UnityEvent();
    ///<summary>
    ///Events that should happen after completing a dialog. You have to manually add and remove listeners, and they will happen every time dialog is complete.
    ///</summary>
    public UnityEvent DialogDoneEvent { get { return _dialogDoneEvent; } }

    private UnityEvent _temporaryDialogDoneEvent = new UnityEvent();

    private float _currentAppearTime = 0;
    private string _currentDialog = "";
    private int _currentLetterNum = 0;
    private float _timer = 0;
    private bool _writingDialog = false;
    private bool _initialSetupDone = false;

    ///<summary>
    ///Awake is called automatically in the game when object is created, before Start-function.
    ///</summary>
    private void Awake()
    {
        InitialSetup();

        //If starting the scene in application, will 
        if (Application.isEditor)
            StartCoroutine(StartDialog(_testString));
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

        //If the textbox is using autosize, lets take if off or else the text will keep changing size when more dialog appears. 
        _controlledText.autoSizeTextContainer = false;
        _currentAppearTime = _defaultAppearTime;
    }

    ///<summary>
    ///Restart all values to default when starting to write new dialog.
    ///</summary>
    ///<params name = "dialog">String containing the dialog to write on screen</params>
    private void StartDialogSetup(string dialog)
    {
        _currentDialog = dialog;
        _currentLetterNum = 0;
        _writingDialog = true;
        _controlledText.text = _currentDialog;
        _controlledText.maxVisibleCharacters = 0;
    }

    ///<summary>
    /// Displays dialog one letter at a time.
    ///</summary>
    /// <param name="dialog">The dialog that should be showed to player.</param>
    /// <param name="dialogDoneActions">All the actions that should be called after the dialog has written everything. These actions happen only once, and will be removed afterwards.</param>
    public IEnumerator StartDialog(string dialog, params UnityAction[] dialogDoneActions)
    {
        foreach (UnityAction ua in dialogDoneActions)
        {
            _temporaryDialogDoneEvent.AddListener(ua);
        }

        yield return StartDialog(dialog);

        _temporaryDialogDoneEvent?.Invoke();
        _temporaryDialogDoneEvent.RemoveAllListeners();
    }

    ///<summary>
    /// Displays dialog one letter at a time.
    ///</summary>
    /// <param name="dialog">The dialog that should be showed to player.</param>
    public IEnumerator StartDialog(string dialog)
    {
        StartDialogSetup(dialog);

        while (_writingDialog)
        {
            WriteDialog();
            yield return null;
        }
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

        //If the end of dialog is reached, make appropriate measures
        if (_currentDialog.Length == _currentLetterNum)
        {
            EndDialog();
            return;
        }

        _currentLetterNum++;
    }
}
