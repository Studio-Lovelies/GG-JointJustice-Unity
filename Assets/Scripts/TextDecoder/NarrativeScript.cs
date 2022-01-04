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
    
    private ScriptReader _scriptReader;

    public string Name => Script.name;
    public Story Story { get; private set; }
    
    /// <summary>
    /// Initialise values on construction.
    /// </summary>
    /// <param name="script">An Ink narrative script</param>
    /// <param name="type">The type of script (dialogue or cross examination)</param>
    public NarrativeScript(TextAsset script, DialogueControllerMode type)
    {
        Script = script;
        Type = type;
    }

    public void Initialize()
    {
        Story = new Story(Script.text);
        _scriptReader = new ScriptReader(Story);
    }
}