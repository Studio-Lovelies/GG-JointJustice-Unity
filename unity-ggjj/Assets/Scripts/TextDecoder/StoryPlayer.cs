using System.ComponentModel;
using Ink.Runtime;

public class StoryPlayer
{
    private StoryPlayer _subStory;
    private readonly AppearingDialogueController _appearingDialogueController;
    private readonly DirectorActionDecoder _directorActionDecoder;
    private readonly ChoiceMenu _choiceMenu;

    private bool IsAtChoice => ActiveNarrativeScript.Story.currentChoices.Count > 0;
    private Story Story => ActiveNarrativeScript.Story;
    
    public NarrativeScript ActiveNarrativeScript { get; set; }
    public GameMode GameMode { get; set; } = GameMode.Dialogue;

    public StoryPlayer(AppearingDialogueController appearingDialogueController, DirectorActionDecoder directorActionDecoder, ChoiceMenu choiceMenu)
    {
        _appearingDialogueController = appearingDialogueController;
        _directorActionDecoder = directorActionDecoder;
        _directorActionDecoder.Decoder.StoryPlayer = this;
        _choiceMenu = choiceMenu;
    }

    public void Continue()
    {
        if (_subStory != null)
        {
            _subStory.Continue();
            return;
        }
        
        if (!Story.canContinue)
        {
            if (!IsAtChoice)
            {
                return;
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

    public void HandleChoice(int choiceIndex)
    {
        Story.ChooseChoiceIndex(choiceIndex);
        Continue();
    }

    public void StartSubStory(NarrativeScript narrativeScript)
    {
        _subStory = new StoryPlayer(_appearingDialogueController, _directorActionDecoder, _choiceMenu);
        _subStory.Continue();
    }
}