using System;
using Ink.Runtime;
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
    public NarrativeScript(TextAsset script, DialogueControllerMode type)
    {
        Script = script;
        Type = type;
        Initialize();
    }

    public void Initialize()
    {
        _objectStorage = new ObjectStorage();
        Story = new Story(Script.text);
        ScriptReader.ReadScript(Story, new ObjectPreloader(_objectStorage));
    }
}