using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class DialogueController : MonoBehaviour
{
    private const char ACTION_TOKEN = '&';


    [SerializeField] private TextAsset _narrativeScript;

    [Header("Events")]

    [Tooltip("Attach a scene controller to this so it can cancel 'wait' actions on new lines.")]
    [SerializeField] private UnityEvent _onNewLine; // TODO make this into a custom event
    
    [Tooltip("Attach a dialogue controller to this so it can display spoken lines")]
    [SerializeField] private NewSpokenLineEvent _onNewSpokenLine;

    [Tooltip("Attach a action decoder so it can deal with the actions")]
    [SerializeField] private NewActionLineEvent _onNewActionLine;

    private Story _inkStory;
    bool _isBusy = false;

    // Start is called before the first frame update
    void Start()
    {
        SetNarrativeScript(_narrativeScript); //TODO:Disable this, for debug only
    }

    public void SetNarrativeScript(TextAsset narrativeScript)
    {
        _inkStory = new Story(_narrativeScript.text);
    }

    public void OnContinueStory()
    {
        if (_isBusy)
        {
            Debug.LogWarning("Tried to continue while busy");
            return;
        }

        OnNextLine();
    }

    public void SetBusy(bool busy)
    {
        _isBusy = busy;
    }

    private void OnNextLine()
    {
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
                Debug.Log("End");
            }
        }
    }

    private bool IsAction(string line)
    {
        return line[0] == ACTION_TOKEN;
        //TODO: Check if line is action
    }

    
}
