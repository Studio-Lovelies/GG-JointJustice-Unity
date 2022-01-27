using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using TextDecoder.Parser;
using Object = Ink.Runtime.Object;
using UnityEngine;

[Serializable]
public class NarrativeScript
{
    [field: Tooltip("Drag an Ink narrative script here.")]
    [field: SerializeField] public TextAsset Script { get; private set; }

    private ObjectStorage _objectStorage = new ObjectStorage();

    public string Name => Script.name;
    public Story Story { get; private set; }
    public IObjectStorage ObjectStorage => _objectStorage;

    /// <summary>
    /// Initialise values on construction.
    /// </summary>
    /// <param name="script">An Ink narrative script</param>
    /// <param name="actionDecoder">An optional action decoder, used for testing</param>
    public NarrativeScript(TextAsset script, IActionDecoder actionDecoder = null)
    {
        Script = script;
        Initialize(actionDecoder);
    }

    /// <summary>
    /// Initializes script values that cannot be set in the Unity inspector
    /// and begins script reading and object preloading.
    /// </summary>
    /// <param name="actionDecoder">An optional action decoder, used for testing</param>
    public void Initialize(IActionDecoder actionDecoder = null)
    {
        _objectStorage = new ObjectStorage();
        Story = new Story(Script.text);
        ReadScript(Story, actionDecoder ?? new ObjectPreloader(_objectStorage));
    }

    /// <summary>
    /// Creates an action decoder and assigns an ObjectPreloader to its controller properties.
    /// Gets all lines from an Ink story, extracts all of the action lines (using a
    /// hash set to remove duplicate lines). Calls the ActionDecoder's OnNewActionLine method
    /// for each action extracted. ActionDecoder then calls the actions
    /// on the ObjectPreloader to preload any required assets.
    /// </summary>
    /// <param name="story">The Ink story to read</param>
    /// <param name="actionDecoder">An optional action decoder, used for testing</param>
    private void ReadScript(Story story, IActionDecoder actionDecoder)
    {
        var lines = new List<string>();
        
        ReadContent(story.mainContentContainer.content, lines);
        ReadContent(story.mainContentContainer.namedOnlyContent?.Values.ToList(), lines);

        var actions = new HashSet<string>(lines.Where(line => line[0] == '&'));
        foreach (var action in actions)
        {
            try
            {
                actionDecoder.OnNewActionLine(action);
            }
            catch (MethodNotFoundScriptParsingException)
            {
                // these types of exceptions are fine, as only actions
                // with resources need to be handled by the ObjectPreloader
            }
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