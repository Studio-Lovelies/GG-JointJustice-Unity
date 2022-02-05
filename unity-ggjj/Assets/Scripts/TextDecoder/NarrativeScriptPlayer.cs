using Ink.Runtime;
using UnityEngine;

public class NarrativeScriptPlayer
{
    private NarrativeScriptPlayer _subStory;
    private AppearingDialogueController _appearingDialogueController;
    private DirectorActionDecoder _directorActionDecoder;

    private bool IsAtChoice => ActiveNarrativeScript.Story.currentChoices.Count > 0;
    private Story Story => ActiveNarrativeScript.Story;
    
    public NarrativeScript ActiveNarrativeScript { get; set; }
    
    public NarrativeScriptPlayer(AppearingDialogueController appearingDialogueController, DirectorActionDecoder directorActionDecoder)
    {
        _appearingDialogueController = appearingDialogueController;
        _directorActionDecoder = directorActionDecoder;
    }

    public void Continue()
    {
        if (!Story.canContinue)
        {
            if (IsAtChoice)
            {
                
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
}