using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

public enum DialogueControllerMode
{
    Dialogue,
    CrossExamination
}

public class DialogueController : MonoBehaviour
{
    private const char ACTION_TOKEN = '&';


    [SerializeField] private TextAsset _narrativeScript;

    [SerializeField] private DialogueControllerMode _dialogueMode = DialogueControllerMode.Dialogue;

    [SerializeField] private GameObject _dialogueControllerPrefab;

    [Header("Events")]

    [Tooltip("Attach a dialogue controller to this so it can display spoken lines")]
    [SerializeField] private NewSpokenLineEvent _onNewSpokenLine;

    [Tooltip("Attach an action decoder so it can deal with the actions")]
    [SerializeField] private NewActionLineEvent _onNewActionLine;

    [Tooltip("Event fired when the dialogue is over")]
    [SerializeField] private UnityEvent _onDialogueFinished;
    
    [Tooltip("Event fired when the dialogue is over")]
    [SerializeField] private UnityEvent<List<Choice>> _onChoicePresented;

    private DialogueController _subStory;

    private Story _inkStory;

    private bool isFirst = false;

    // Start is called before the first frame update
    void Start()
    {
        if (_narrativeScript != null)
        {
            SetNarrativeScript(_narrativeScript); //TODO:Disable this, for debug only
            isFirst = true;
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
                Debug.Log(currentLine); //Temp to show lines being said
            }
        }
        else
        {
            List<Choice> choiceList = _inkStory.currentChoices;

            if (choiceList.Count > 0)
            {
                //Choices present
            }
            else
            {
                    Debug.Log("Done with story " + gameObject.transform.name);
                    _onDialogueFinished.Invoke();
            }
        }
    }

        

    private bool IsAction(string line)
    {
        return line[0] == ACTION_TOKEN;
        //TODO: Check if line is action
    }

    //Sub story stuff
    public void StartSubStory(TextAsset subStory)
    {
        GameObject obj = GameObject.Instantiate(_dialogueControllerPrefab);
        _subStory = obj.GetComponent<DialogueController>();
        _subStory.SubStoryInit(this);
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
