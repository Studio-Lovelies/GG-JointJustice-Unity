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

    private DialogueController _subStory; //TODO: Substory needs to remember state to come back to (probably?)

    private bool _isAtChoice = false; //Possibly small state machine to handle all input?

    private Story _inkStory;
    bool _dialgoueIsWriting = false;

    // Start is called before the first frame update
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

    public void SetNarrativeScript(TextAsset narrativeScript)
    {
        _inkStory = new Story(narrativeScript.text);
    }

    public void OnNextLine()
    {
        if (_subStory != null)
        {
            _subStory.OnNextLine();
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

    public void HandleChoice(int choice)
    {
        if (!_isAtChoice)
            return;

        if (choice > _inkStory.currentChoices.Count)
        {
            Debug.LogError("choice index out of range");
        }
        else
        {
            _inkStory.ChooseChoiceIndex(choice);
            _isAtChoice = false;
            OnNextLine();
        }
    }

    private void HandleNextLineCrossExamination()
    {
        if (_isAtChoice) //Make sure we don't continue unless we're not at a choice in the cross examination
            return;

        if(_dialgoueIsWriting)
        {
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

    private void HandleEvidencePresented(string evidence)
    {
        List<Choice> choiceList = _inkStory.currentChoices;

        if (choiceList.Count <= 2)
        {
            //No evidence possible for a good direction, kill off.
        }
        else
        {
            int evidenceFoundAt = -1;
            for(int i = 2; i < choiceList.Count; i++)
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
                //Deal with bad consequences
                HandleChoice(0); //0 is continue
            }
        }
    }
        

    private bool IsAction(string line)
    {
        return line[0] == ACTION_TOKEN;
    }

    public void SetDialogueIsWriting(bool writing)
    {
        _dialgoueIsWriting = writing;
    }

    //Sub story stuff
    public void StartSubStory(TextAsset subStory)
    {
        GameObject obj = GameObject.Instantiate(_dialogueControllerPrefab);
        _subStory = obj.GetComponent<DialogueController>();
        _subStory.SubStoryInit(this); //RECURSION
        _subStory.SetNarrativeScript(subStory);
        _subStory.OnNextLine();
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
        OnNextLine();
    }
}
