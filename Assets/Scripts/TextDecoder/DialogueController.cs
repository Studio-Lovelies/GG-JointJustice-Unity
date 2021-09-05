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

    [Tooltip("This event is called when the _isBusy field is set.")]
    [SerializeField] private UnityEvent<bool> _onBusySet;

    private Story _inkStory;
    bool _isBusy = false;
    private bool _isMenuOpen;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    void Start()
    {
        SetNarrativeScript(_narrativeScript); //TODO:Disable this, for debug only
    }

    /// <summary>
    /// Used to start a new narrative script
    /// </summary>
    /// <param name="narrativeScript">JSON file to switch to</param>
    public void SetNarrativeScript(TextAsset narrativeScript)
    {
        _inkStory = new Story(_narrativeScript.text);
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

        OnNextLine();
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
    /// Makes sure the system can't continue when a menu is open.
    /// Should be set by a menu's opening and closing events.
    /// </summary>
    /// <param name="isMenuOpen">Whether is open (true) or not (false).</param>
    public void SetMenuOpen(bool isMenuOpen)
    {
        _isMenuOpen = isMenuOpen;
    }

    /// <summary>
    /// Reads the next line and sets up everything relevant to it, including handling actions and setting up choices
    /// </summary>
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

    /// <summary>
    /// Checks whether a certain line is an action line or not
    /// </summary>
    /// <param name="line">Line to check</param>
    /// <returns>Whether the line is an action or not</returns>
    private bool IsAction(string line)
    {
        return line[0] == ACTION_TOKEN;
        //TODO: Check if line is action
    }

    
}
