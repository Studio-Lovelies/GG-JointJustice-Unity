using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum DialogueControllerMode
{
    Dialogue,
    CrossExamination
}

[System.Serializable]
public enum CrossExaminationChoice
{
    Continue,
    Press,
    Evidence
}

public class DialogueController : MonoBehaviour
{
    private const char ACTION_TOKEN = '&';

    [SerializeField] private TextAsset _narrativeScript;

    [SerializeField] private DialogueControllerMode _dialogueMode = DialogueControllerMode.Dialogue;

    [SerializeField] private FailureStoryList _failureList;

    [SerializeField] private DialogueController _dialogueControllerPrefab;

    [Header("Events")]

    [Tooltip("Attach a scene controller to this so it can cancel 'wait' actions on new lines.")]
    [SerializeField] private UnityEvent _onNewLine;

    [Tooltip("Attach a dialogue controller to this so it can display spoken lines")]
    [SerializeField] private UnityEvent<string> _onNewSpokenLine;

    [Tooltip("Attach an action decoder so it can deal with the actions")]
    [SerializeField] private UnityEvent<string> _onNewActionLine;

    [Tooltip("Event fired when the dialogue is over")]
    [SerializeField] private UnityEvent _onDialogueFinished;

    [Tooltip("Event fired when a choice is encountered in regular dialogue")]
    [SerializeField] private UnityEvent<List<Choice>> _onChoicePresented;

    [Tooltip("Event fired when the green text loop is entered or left")]
    [SerializeField] private UnityEvent<bool> _onCrossExaminationLoopActive;

    [Tooltip("This event is called when the _isBusy field is set.")]
    [SerializeField] private UnityEvent<bool> _onBusySet;

    private Story _inkStory;
    private bool _isBusy;
    private bool _isMenuOpen;
    private DialogueController _subStory; //TODO: Substory needs to remember state to come back to (probably?)

    private bool _isAtChoice; //Possibly small state machine to handle all input?

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    void Start()
    {
        if (_narrativeScript != null)
        {
            SetNarrativeScript(_narrativeScript); //TODO:Disable this, for debug only
        }
    }

    /// <summary>
    /// Initialized a sub story by hooking the events to the parent dialogue so everything propagated down correctly
    /// </summary>
    /// <param name="parent">Parent of this dialogue to hook everything in to</param>
    void SubStoryInit(DialogueController parent)
    {
        _onNewSpokenLine.AddListener(parent.OnSubStorySpokenLine);
        _onNewActionLine.AddListener(parent.OnSubStoryActionLine);
        _onDialogueFinished.AddListener(parent.OnSubStoryFinished);
        _onChoicePresented.AddListener(parent.OnSubStoryChoicesPresented);
    }

    /// <summary>
    /// Used to start a new narrative script
    /// </summary>
    /// <param name="narrativeScript">JSON file to switch to</param>
    public void SetNarrativeScript(TextAsset narrativeScript)
    {
        _inkStory = new Story(narrativeScript.text);
    }

    /// <summary>
    /// Call externally on internally to continue the story.
    /// </summary>
    public void OnContinueStory()
    {
        if (_isBusy || _isMenuOpen) //Doesn't need to be handled in the sub story
        {
            Debug.Log($"Tried to continue while {(_isBusy ? "busy" : "menu is open")}");
            return;
        }

        if (_subStory != null)
        {
            _subStory.OnContinueStory();
            return;
        }

        switch (_dialogueMode)
        {
            case DialogueControllerMode.Dialogue:
                HandleNextLineDialogue();
                break;
            case DialogueControllerMode.CrossExamination:
                HandleNextLineCrossExamination();
                break;
            default:
                Debug.LogError("Unhandled dialogue type");
                break;
        }
    }

    /// <summary>
    /// Called externally to press the witness. Makes sure we're in cross examination mode before calling handle choice.
    /// </summary>
    public void OnPressWitness()
    {
        if (_dialogueMode != DialogueControllerMode.CrossExamination)
        {
            return;
        }
        _onCrossExaminationLoopActive.Invoke(false);
        HandleChoice(1);
    }

    /// <summary>
    /// If the system is at a choice, this picks the choice and continues the story
    /// </summary>
    /// <param name="choice">The index of the choice to be picked (0 based)</param>
    public void HandleChoice(int choice)
    {
        if (!_isAtChoice || _isBusy || _isMenuOpen)
            return;

        if (choice > _inkStory.currentChoices.Count)
        {
            Debug.LogError("choice index out of range");
        }
        else
        {
            _inkStory.ChooseChoiceIndex(choice);
            _isAtChoice = false;
            OnContinueStory();
        }        
    }

    /// <summary>
    /// Makes sure the system can't continue when a menu is open.
    /// Should be set by a menu's opening and closing events.
    /// </summary>
    /// <param name="isMenuOpen">Whether is open (true) or not (false).</param>
    public void SetMenuOpen(bool isMenuOpen)
    {
        _isMenuOpen = isMenuOpen;
    }

    /// <summary>
    /// Actually handles the evidence being presented by seeing if it is one of the current choices and then progressing the story appropriately. Makes sure we're in cross examination mode before continuing.
    /// May spawn a random failure sub story.
    /// </summary>
    /// <param name="presentedObject">Name of the object you want to present to the court</param>
    public void HandlePresenting(ICourtRecordObject presentedObject)
    {
        if (_subStory)
        {
            _subStory.HandlePresenting(presentedObject);
            return;
        }

        if (!_isAtChoice)
        {
            return;
        }

        if (_dialogueMode != DialogueControllerMode.CrossExamination)
        {
            return;
        }

        _onCrossExaminationLoopActive.Invoke(false);

        List<Choice> choiceList = _inkStory.currentChoices;

        if (choiceList.Count <= 2)
        {
            //Deal with bad consequences, spawn sub story and continue that
            StartSubStory(_failureList.GetRandomFailurestate());
            return;
        }

        int evidenceFoundAt = -1;
        for (int i = 2; i < choiceList.Count; i++)
        {
            if (choiceList[i].text == presentedObject.InstanceName)
            {
                evidenceFoundAt = i;
                break;
            }
        }
        if (evidenceFoundAt != -1)
        {
            HandleChoice(evidenceFoundAt);
            return;
        }

        //Deal with bad consequences, spawn sub story and continue that
        StartSubStory(_failureList.GetRandomFailurestate());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="choiceList"></param>
    private void HandleCrossExaminationLoop(List<Choice> choiceList)
    {
        _isAtChoice = true;
        if (_dialogueMode == DialogueControllerMode.CrossExamination)
        {
            _onCrossExaminationLoopActive.Invoke(true);
        }
        _onChoicePresented.Invoke(choiceList);
    }

    /// <summary>
    /// Handles the next line of dialogue in regular dialogue mode.
    /// </summary>
    private void HandleNextLineDialogue()
    {
        if (_isAtChoice) // Make sure we don't continue unless we're not at a choice
        {
            return;
        }

        if (!_inkStory.canContinue)
        {
            List<Choice> choiceList = _inkStory.currentChoices;
            if (choiceList.Count <= 0)
            {
                _onDialogueFinished.Invoke();
                return;
            }

            // If we're at the end of the current ink story but have dialogue options available,
            // we're inside a cross examination statement loop
            HandleCrossExaminationLoop(choiceList);
            return;
        }

        string currentLine = _inkStory.Continue();
        if (currentLine.Length == 0) //0 should never happen
        {
            Debug.LogError("Line-length was 0, should never happen");
            return;
        }

        if (currentLine.Length == 1) //inky reports a line with a comment back as a line with \n so this is used to automatically continue in that case
        {
            HandleNextLineDialogue();
            return;
        }

        if (IsAction(currentLine))
        {
            _onNewActionLine.Invoke(currentLine);
        }
        else
        {
            if (currentLine == "\n")
            {
                HandleNextLineDialogue();
                return;
            }

            _onNewSpokenLine.Invoke(currentLine);
        }
    }

    /// <summary>
    /// Handles the next line of dialogue in cross examination mode.
    /// </summary>
    private void HandleNextLineCrossExamination()
    {
        if (_isAtChoice)
        {
            HandleChoice(0); //Handle regular continue
            return;
        }

        if (_inkStory.canContinue)
        {
            string currentLine = _inkStory.Continue();
            if (currentLine.Length == 0) //0 should never happen
            {
                Debug.LogError("Line-length was 0, should never happen");
                return;
            }
            
            if (currentLine.Length == 1) //inky reports a line with a comment back as a line with \n so this is used to automatically continue in that case
            {
                HandleNextLineCrossExamination();
                return;
            }

            _onNewLine.Invoke();

            if (IsAction(currentLine))
            {
                _onNewActionLine.Invoke(currentLine);
            }
            else
            {
                _onNewSpokenLine.Invoke(currentLine);
            }
        }
        else
        {
            List<Choice> choiceList = _inkStory.currentChoices;

            if (choiceList.Count > 0)
            {
                _isAtChoice = true;
                if (_dialogueMode == DialogueControllerMode.CrossExamination)
                {
                    _onCrossExaminationLoopActive.Invoke(true);
                }
                _onChoicePresented.Invoke(choiceList);
            }
            else
            {
                _onDialogueFinished.Invoke();
            }
        }

        HandleCrossExaminationLoop(choiceList);
    }

    /// <summary>
    /// Makes sure the system can't continue to the next line
    /// </summary>
    /// <param name="busy">Sets the busy flag</param>
    public void SetBusy(bool busy)
    {
        _isBusy = busy;
        _onBusySet.Invoke(_isBusy);
    }

    /// <summary>
    /// Checks whether a certain line is an action line or not
    /// </summary>
    /// <param name="line">Line to check</param>
    /// <returns>Whether the line is an action or not</returns>
    private static bool IsAction(string line)
    {
        return line[0] == ACTION_TOKEN;
    }

    //Sub story stuff

    /// <summary>
    /// Starts a new sub story based on the provided inky dialogue script. The sub story always goes before the main story. When the sub story is finished, the main story continues on the line after the one it left off on.
    /// </summary>
    /// <param name="subStory">Inky dialogue script to be set as the sub story</param>
    public void StartSubStory(TextAsset subStory)
    {
        _subStory = Instantiate(_dialogueControllerPrefab); //Returns the DialogueController component attached to the instantiated gameobject
        _subStory.SubStoryInit(this); //RECURSION
        _subStory.SetNarrativeScript(subStory);
        _subStory.OnContinueStory();
    }

    /// <summary>
    /// Callback for a sub story so it can propagate downwards
    /// </summary>
    /// <param name="spokenLine">Spoken line to be shown</param>
    public void OnSubStorySpokenLine(string spokenLine)
    {
        _onNewSpokenLine.Invoke(spokenLine);
    }

    /// <summary>
    /// Callback for a sub story so it can propagate downwards
    /// </summary>
    /// <param name="actionLine">Action line to be handled</param>
    public void OnSubStoryActionLine(string actionLine)
    {
        _onNewActionLine.Invoke(actionLine);
    }

    /// <summary>
    /// Callback for a sub story so it can propagate downwards
    /// </summary>
    /// <param name="choices">Choices to be presented</param>
    public void OnSubStoryChoicesPresented(List<Choice> choices)
    {
        _onChoicePresented.Invoke(choices);
    }

    /// <summary>
    /// Callback for when the sub story is finished to we can destroy the gameobject it is contained in and continue this story.
    /// </summary>
    public void OnSubStoryFinished()
    {
        Destroy(_subStory.gameObject);
        _subStory = null;
        OnContinueStory();
    }
}
