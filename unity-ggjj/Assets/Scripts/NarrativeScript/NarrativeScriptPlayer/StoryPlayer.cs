using System;
using System.ComponentModel;
using System.Linq;
using Ink.Runtime;

public class StoryPlayer
{
    private readonly IAppearingDialogueController _appearingDialogueController;
    private readonly IActionDecoder _actionDecoder;
    private readonly IChoiceMenu _choiceMenu;
    private readonly INarrativeScriptPlaylist _narrativeScriptPlaylist;
    private NarrativeScript _activeNarrativeScript;
    private StoryPlayer _parent;
    private StoryPlayer _subStory;
    private GameMode _gameMode = GameMode.Dialogue;
    private bool _waiting;

    private Story Story => ActiveNarrativeScript.Story;
    private bool IsAtChoice => ActiveNarrativeScript.Story.currentChoices.Count > 0;

    public bool Waiting
    {
        get => HasSubStory ? _subStory.Waiting : _waiting;
        set
        {
            if (HasSubStory)
            {
                _subStory.Waiting = value;
            }

            _waiting = value;
        }
    }
    public bool HasSubStory => _subStory != null;

    public bool CanPressWitness
    {
        get
        {
            if (HasSubStory)
            {
                return _subStory.CanPressWitness;
            }

            return IsAtChoice && GameMode == GameMode.CrossExamination && !_appearingDialogueController.IsPrintingText;
        }
    }

    public NarrativeScript ActiveNarrativeScript
    {
        get => HasSubStory ? _subStory.ActiveNarrativeScript : _activeNarrativeScript;
        set => _activeNarrativeScript = value;
    }
    
    public GameMode GameMode
    {
        get => HasSubStory ? _subStory.GameMode : _gameMode;
        set
        {
            if (HasSubStory)
            {
                _subStory.GameMode = value;
            }
            else
            {
                _gameMode = value;
            }
        }
    }

    public StoryPlayer(INarrativeScriptPlaylist narrativeScriptPlaylist, IAppearingDialogueController appearingDialogueController, IActionDecoder actionDecoder, IChoiceMenu choiceMenu)
    {
        _narrativeScriptPlaylist = narrativeScriptPlaylist;
        _appearingDialogueController = appearingDialogueController;
        _actionDecoder = actionDecoder;
        _choiceMenu = choiceMenu;
    }

    /// <summary>
    /// Continues the Ink story.
    /// Does nothing if text is being printed.
    /// </summary>
    /// <param name="overridePrintingText"></param>
    public void Continue(bool overridePrintingText = false)
    {
        if (_appearingDialogueController.IsPrintingText && !overridePrintingText)
        {
            return;
        }
        
        if (_subStory != null)
        {
            _subStory.Continue();
            return;
        }
        
        if (HandleCannotContinue() || Waiting)
        {
            return;
        }
        
        var nextLine = Story.Continue();
        if (nextLine == string.Empty)
        {
            Continue();
        }
        
        if (_actionDecoder.IsAction(nextLine))
        {
            _actionDecoder.OnNewActionLine(nextLine);
        }
        else
        {
            _appearingDialogueController.PrintText(nextLine);
        }
    }

    /// <summary>
    /// Checks if a story can continue, and handles what happens
    /// it cannot, depending on the current GameMode
    /// </summary>
    /// <returns>If the story can continue (true) or (not)</returns>
    private bool HandleCannotContinue()
    {
        if (Story.canContinue)
        {
            return false;
        }
        
        if (!IsAtChoice)
        {
            _parent?.EndSubStory();
            return true;
        }

        switch (GameMode)
        {
            case GameMode.Dialogue:
                _choiceMenu.Initialise(Story.currentChoices);
                break;
            case GameMode.CrossExamination:
                HandleChoice(0);
                break;
            default:
                throw new InvalidEnumArgumentException($"{GameMode} is an invalid GameMode");
        }

        return true;
    }

    /// <summary>
    /// Selects a choice index in the Ink story and continues the story
    /// </summary>
    /// <param name="choiceIndex">The index of the choice to choose</param>
    public void HandleChoice(int choiceIndex)
    {
        Story.ChooseChoiceIndex(choiceIndex);
        Continue();
    }

    /// <summary>
    /// Starts a sub-story which will run in place of the parent story until it ends or is stopped
    /// </summary>
    /// <param name="narrativeScript"></param>
    public void StartSubStory(NarrativeScript narrativeScript)
    {
        _subStory = new StoryPlayer(_narrativeScriptPlaylist, _appearingDialogueController, _actionDecoder, _choiceMenu)
        {
            ActiveNarrativeScript = narrativeScript,
            _parent = this
        };
        _appearingDialogueController.StopPrintingText();
        _subStory.Continue(true);
    }

    /// <summary>
    /// Ends the current sub-story
    /// </summary>
    private void EndSubStory()
    {
        _subStory = null;
        Continue();
    }

    /// <summary>
    /// Handles presenting of evidence.
    /// Checks if evidence is required, then checks if the
    /// given evidence is correct depending on the Ink story
    /// </summary>
    /// <param name="courtRecordObject">The court record object that is required to present</param>
    public void PresentEvidence(ICourtRecordObject courtRecordObject)
    {
        if (_subStory != null)
        {
            _subStory.PresentEvidence(courtRecordObject);
            return;
        }

        if (!IsAtChoice)
        {
            throw new NotSupportedException("Cannot present evidence when not at choice");
        }

        if (GameMode != GameMode.CrossExamination)
        {
            throw new NotSupportedException("Can only present evidence during cross examination");
        }

        var currentChoices = Story.currentChoices;
        var choice = currentChoices.FirstOrDefault(choice => choice.text == courtRecordObject.InstanceName);
        if (choice != null)
        {
            HandleChoice(choice.index);
        }
        else
        {
            StartSubStory(_narrativeScriptPlaylist.GetRandomFailureScript());
        }
    }
}