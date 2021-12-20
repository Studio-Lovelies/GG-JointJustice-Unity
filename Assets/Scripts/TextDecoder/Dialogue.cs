using UnityEngine;

[System.Serializable]
public struct Dialogue
{
    /// <summary>
    /// Initialise values on construction.
    /// </summary>
    /// <param name="narrativeScript">An Ink narrative script</param>
    /// <param name="type">The type of script (dialogue or cross examination</param>
    public Dialogue(TextAsset narrativeScript, DialogueControllerMode type)
    {
        NarrativeScript = narrativeScript;
        ScriptType = type;
    }

    [field: Tooltip("Drag an Ink narrative script here.")]
    [field: SerializeField] public TextAsset NarrativeScript { get; private set; }
    
    [field: Tooltip("The dialogue mode the narrative script will use (dialogue or cross examination).")]
    [field: SerializeField] public DialogueControllerMode ScriptType { get; private set; }
}