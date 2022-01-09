using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Object = Ink.Runtime.Object;
using UnityEngine;

[Serializable]
public class NarrativeScript
{
    [field: Tooltip("Drag an Ink narrative script here.")]
    [field: SerializeField] public TextAsset Script { get; private set; }
    
    [field: Tooltip("The dialogue mode the narrative script will use (dialogue or cross examination).")]
    [field: SerializeField] public DialogueControllerMode Type { get; private set; }

    private ObjectStorage _objectStorage = new ObjectStorage();

    public string Name => Script.name;
    public Story Story { get; private set; }
    public IObjectStorage ObjectStorage => _objectStorage;

    /// <summary>
    /// Initialise values on construction.
    /// </summary>
    /// <param name="script">An Ink narrative script</param>
    /// <param name="type">The type of script (dialogue or cross examination)</param>
    /// <param name="objectPreloader">An optional object preloader that can be injected (used for testing)</param>
    public NarrativeScript(TextAsset script, DialogueControllerMode type, IObjectPreloader objectPreloader = null)
    {
        Script = script;
        Type = type;
        Initialize(objectPreloader);
    }

    /// <summary>
    /// Initialises script values that cannot be set in the Unity inspector
    /// and begins script reading and object preloading.
    /// </summary>
    /// <param name="objectPreloader">An optional object preloader that can be injected (used for testing)</param>
    public void Initialize(IObjectPreloader objectPreloader = null)
    {
        _objectStorage = new ObjectStorage();
        Story = new Story(Script.text);
        ReadScript(Story, objectPreloader ?? new ObjectPreloader(_objectStorage));
    }
    
    /// <summary>
    /// Creates an action decoder and assigns an ObjectPreloader to its controller properties.
    /// Gets all lines from an Ink story, extracts all of the action lines (using a
    /// hash set to remove duplicate lines). Calls the ActionDecoder's OnNewActionLine method
    /// for each action extracted. ActionDecoder then calls the actions
    /// on the ObjectPreloader to preload any required assets.
    /// </summary>
    /// <param name="story">The Ink story to read</param>
    /// <param name="objectPreloader">An ObjectPreloader object</param>
    private static void ReadScript(Story story, IObjectPreloader objectPreloader)
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