using System.Collections;
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

    [SerializeField] private GameObject _dialogueControllerPrefab;

    [Header("Events")]

    [Tooltip("Attach a scene controller to this so it can cancel 'wait' actions on new lines.")]
    [SerializeField] private UnityEvent _onNewLine; // TODO make this into a custom event

    [Tooltip("Attach a dialogue controller to this so it can display spoken lines")]
    [SerializeField] private NewSpokenLineEvent _onNewSpokenLine;

    [Tooltip("Attach an action decoder so it can deal with the actions")]
    [SerializeField] private NewActionLineEvent _onNewActionLine;

    [Tooltip("Event fired when the dialogue is over")]
    [SerializeField] private UnityEvent _onDialogueFinished;

    [Tooltip("Event fired when a choice is encountered in regular dialogue")]
    [SerializeField] private UnityEvent<List<Choice>> _onChoicePresented;
    
    [Tooltip("This event is called when the _isBusy field is set.")]
    [SerializeField] private UnityEvent<bool> _onBusySet;

    private Story _inkStory;
    bool _isBusy = false;
    private bool _isMenuOpen;
    private DialogueController _subStory; //TODO: Substory needs to remember state to come back to (probably?)

    private bool _isAtChoice = false; //Possibly small state machine to handle all input?

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

    void SubStoryInit(DialogueController parent)
    {
        _onNewSpokenLine.AddListener(parent.OnSubStorySpokenLine);
        _onNewActionLine.AddListener(parent.OnSubStoryActionLine);
        _onDialogueFinished.AddListener(parent.OnSubStoryFinished);
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
        if (_isBusy || _isMenuOpen)
        {
            Debug.LogWarning($"Tried to continue while {(_isBusy ? "busy" : "menu is open")}");
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

    public void HandleChoice(int choice)
    {
        if (!_isAtChoice || _isBusy)
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

    public void HandleEvidencePresented(string evidence)
    {
        List<Choice> choiceList = _inkStory.currentChoices;

        if (choiceList.Count <= 2)
        {
            //No evidence possible for a good direction, kill off.
        }
        else
        {
            int evidenceFoundAt = -1;
            for (int i = 2; i < choiceList.Count; i++)
            {
                //if choiceList[i] == evidence
                //Do the thing
                //Can't be implemented yet cause evidence isn't implemented yet.
            }
            if (evidenceFoundAt != -1)
            {
                HandleChoice(evidenceFoundAt);
            }
            else
            {
                //Deal with bad consequences, spawn sub story and continue that
                HandleChoice(0); //0 is continue, for debug purposes
            }
        }
    }

    private void HandleNextLineDialogue()
    {
        if (_isAtChoice) //Make sure we don't continue unless we're not at a choice
            return;

        if (_inkStory.canContinue)
        {
            string currentLine = _inkStory.Continue();

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
                _onChoicePresented.Invoke(choiceList);
            }
            else
            {
                Debug.Log("Done with story " + gameObject.transform.name);
                _onDialogueFinished.Invoke();
            }
        }
    }

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
            //Story has ended
            _onDialogueFinished.Invoke();
        }

        if (!_inkStory.canContinue) //At choice, set up choice sequence (or at end, but we'll deal with that on the next space press)
        {
            List<Choice> choiceList = _inkStory.currentChoices;

            if (choiceList.Count > 0)
            {
                _isAtChoice = true;
            }
        }
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
    private bool IsAction(string line)
    {
        return line[0] == ACTION_TOKEN;
    }




    //Sub story stuff
    public void StartSubStory(TextAsset subStory)
    {
        GameObject obj = GameObject.Instantiate(_dialogueControllerPrefab);
        _subStory = obj.GetComponent<DialogueController>();
        _subStory.SubStoryInit(this); //RECURSION
        _subStory.SetNarrativeScript(subStory);
        _subStory.OnContinueStory();
    }

    public void OnSubStorySpokenLine(string spokenLine)
    {
        _onNewSpokenLine.Invoke(spokenLine);
    }

    public void OnSubStoryActionLine(string actionLine)
    {
        _onNewActionLine.Invoke(actionLine);
    }

    public void OnSubStoryFinished()
    {
        Destroy(_subStory.gameObject);
        _subStory = null;
        OnContinueStory();
    }
}
