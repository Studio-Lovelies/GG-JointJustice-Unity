using Ink.Runtime;

public class NarrativeScriptPlayer
{
    private NarrativeScriptPlayer _subStory;
    private readonly AppearingDialogueController _appearingDialogueController;
    private readonly DirectorActionDecoder _directorActionDecoder;
    private readonly ChoiceMenu _choiceMenu;

    private bool IsAtChoice => ActiveNarrativeScript.Story.currentChoices.Count > 0;
    private Story Story => ActiveNarrativeScript.Story;
    
    public NarrativeScript ActiveNarrativeScript { get; set; }
    
    public NarrativeScriptPlayer(AppearingDialogueController appearingDialogueController, DirectorActionDecoder directorActionDecoder, ChoiceMenu choiceMenu)
    {
        _appearingDialogueController = appearingDialogueController;
        _directorActionDecoder = directorActionDecoder;
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
            if (IsAtChoice)
            {
                _choiceMenu.Initialise(Story.currentChoices);
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
}