using System.ComponentModel;
using System.Linq;
using Ink.Runtime;

public class StoryPlayer
{
    private readonly AppearingDialogueController _appearingDialogueController;
    private readonly DirectorActionDecoder _directorActionDecoder;
    private readonly ChoiceMenu _choiceMenu;
    private readonly NarrativeScriptPlaylist _narrativeScriptPlaylist;
    private NarrativeScript _activeNarrativeScript;
    private StoryPlayer _parent;
    private StoryPlayer _subStory;
    private GameMode _gameMode = GameMode.Dialogue;
    private bool _waiting = false;

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

            return IsAtChoice && GameMode == GameMode.CrossExamination;
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

    public StoryPlayer(NarrativeScriptPlaylist narrativeScriptPlaylist, AppearingDialogueController appearingDialogueController, DirectorActionDecoder directorActionDecoder, ChoiceMenu choiceMenu)
    {
        _narrativeScriptPlaylist = narrativeScriptPlaylist;
        _appearingDialogueController = appearingDialogueController;
        _directorActionDecoder = directorActionDecoder;
        _directorActionDecoder.Decoder.StoryPlayer = this;
        _choiceMenu = choiceMenu;
    }

    public void Continue()
    {
        if (_appearingDialogueController.PrintingText)
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
        if (_directorActionDecoder.IsAction(nextLine))
        {
            _directorActionDecoder.OnNewActionLine(nextLine);
        }
        else
        {
            _appearingDialogueController.PrintText(nextLine);
        }
    }

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

    public void HandleChoice(int choiceIndex)
    {
        Story.ChooseChoiceIndex(choiceIndex);
        Continue();
    }

    public void StartSubStory(NarrativeScript narrativeScript)
    {
        _subStory = new StoryPlayer(_narrativeScriptPlaylist, _appearingDialogueController, _directorActionDecoder, _choiceMenu)
        {
            ActiveNarrativeScript = narrativeScript,
            _parent = this
        };
        _subStory.Continue();
    }

    private void EndSubStory()
    {
        _subStory = null;
        Continue();
    }

    public void PresentEvidence(ICourtRecordObject courtRecordObject)
    {
        if (_subStory != null)
        {
            _subStory.PresentEvidence(courtRecordObject);
            return;
        }

        if (!IsAtChoice || GameMode != GameMode.CrossExamination)
        {
            return;
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