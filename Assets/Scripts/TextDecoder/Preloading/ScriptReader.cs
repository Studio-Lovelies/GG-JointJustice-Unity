using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;

public class ScriptReader
{
    private ObjectPreloader _objectPreloader = new ObjectPreloader();
    private ActionDecoder _actionDecoder;

    public ScriptReader(Story story)
    {
        _actionDecoder = new ActionDecoder
        {
            ActorController = _objectPreloader,
            EvidenceController = _objectPreloader,
            SceneController = _objectPreloader,
            AudioController = _objectPreloader,
            AppearingDialogueController = _objectPreloader
        };
        
        var actions = new HashSet<string>(story.BuildStringOfHierarchy().Split('"').Where(line => line[0] == '&'));
        foreach (var action in actions)
        {
            _actionDecoder.OnNewActionLine(action);
        }
    }
}
