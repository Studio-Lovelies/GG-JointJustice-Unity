using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Object = Ink.Runtime.Object;

public static class ScriptReader
{
    /// <summary>
    /// On construction, creates an action code and assigns an ObjectPreloader to its controller properties.
    /// Gets all lines from an Ink story, extracts all of the action lines (using a
    /// hash set to remove duplicate lines). Calls the ActionDecoder's OnNewActionLine method
    /// for each action extracted. ActionDecoder then calls the actions
    /// on the ObjectPreloader to preload any required assets.
    /// </summary>
    /// <param name="story">The Ink story to read</param>
    /// <param name="objectPreloader">An ObjectPreloader object</param>
    public static void ReadScript(Story story, IObjectPreloader objectPreloader)
    {
        var actionDecoder = new ActionDecoder
        {
            ActorController = objectPreloader,
            EvidenceController = objectPreloader,
            SceneController = objectPreloader,
            AudioController = objectPreloader,
            AppearingDialogueController = objectPreloader
        };

        var lines = new List<string>();
        ReadContent(story.mainContentContainer.content, lines);
        ReadContent(story.mainContentContainer.namedOnlyContent?.Values.ToList(), lines);

        var actions = new HashSet<string>(lines.Where(line => line[0] == '&'));
        foreach (var action in actions)
        {
            actionDecoder.OnNewActionLine(action);
        }
    }

    /// <summary>
    /// Reads the content of an Ink file.
    /// Ink files exist as containers within containers.
    /// Reads the content of containers until a StringValue
    /// is found, which is then added to a provided list.
    /// </summary>
    /// <param name="content">The Ink container content to read</param>
    /// <param name="lines">A list to add read lines to</param>
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
