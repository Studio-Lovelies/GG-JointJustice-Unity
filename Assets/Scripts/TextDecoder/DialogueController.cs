using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    private const char ACTION_TOKEN = '&';


    [SerializeField] private TextAsset _narrativeScript;

    [Header("Events")]

    [Tooltip("Attach a dialogue controller to this so it can display spoken lines")]
    [SerializeField] private NewSpokenLineEvent _onNewSpokenLine;

    [Tooltip("Attach a action decoder so it can deal with the actions")]
    [SerializeField] private NewActionLineEvent _onNewActionLine;
    bool _writingDialog = false;

    private Story _inkStory;

    // Start is called before the first frame update
    void Start()
    {
        SetNarrativeScript(_narrativeScript); //TODO:Disable this, for debug only
    }

    private void Update()
    {
        if (!_writingDialog && Input.GetKeyDown(KeyCode.Space)) //TODO: This is debug, remove
        {
            OnNextLine();
            _writingDialog = true;
        }
    }

    public void SetNarrativeScript(TextAsset narrativeScript)
    {
        _inkStory = new Story(_narrativeScript.text);
    }


    public void OnNextLine()
    {
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

        List<Choice> choiceList = _inkStory.currentChoices;

        if (choiceList.Count > 0)
        {
            //Choices present
        }
        else
        {
            //Empty
        }

    }

    private bool IsAction(string line)
    {
        return line[0] == ACTION_TOKEN;
        //TODO: Check if line is action
    }

    ///<summary>
    ///When the dialog has been written, makes the code be able to continue to next dialog
    ///</summary>
    public void StopWritingDialog()
    {
        _writingDialog = false;
    }
}
