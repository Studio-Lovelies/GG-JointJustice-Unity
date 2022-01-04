using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;

public static class ScriptReader
{
    public static void ReadScript(Story story, ObjectPreloader objectStorage)
    {
        var actionDecoder = new ActionDecoder
        {
            ActorController = objectStorage,
            EvidenceController = objectStorage,
            SceneController = objectStorage,
            AudioController = objectStorage,
            AppearingDialogueController = objectStorage
        };
        
        var actions = new HashSet<string>(story.BuildStringOfHierarchy().Split('"').Where(line => line[0] == '&'));
        foreach (var action in actions)
        {
            actionDecoder.OnNewActionLine(action);
        }
    }
}
