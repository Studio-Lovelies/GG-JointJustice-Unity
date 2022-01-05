using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using UnityEngine;
using Object = Ink.Runtime.Object;

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
        
        List<string> lines = new List<string>();
        ReadContent(story.mainContentContainer.content, lines);
        ReadContent(story.mainContentContainer.namedOnlyContent?.Values.ToList(), lines);

        foreach (var line in lines.Where(line => line[0] == '&'))
        {
            Debug.Log(line);
        }
        
        var actions = new HashSet<string>(lines.Where(line => line[0] == '&'));
        foreach (var action in actions)
        {
            actionDecoder.OnNewActionLine(action);
        }
    }

    private static void ReadContent(List<Object> content, List<string> lines)
    {
        if (content == null)
        {
            return;
        }

        foreach (var obj in content)
        {
            switch (obj)
            {
                case Container container:
                    ReadContent(container.content, lines);
                    break;
                case StringValue value:
                    lines.Add(value.ToString());
                    break;
            }
        }
    }
}
