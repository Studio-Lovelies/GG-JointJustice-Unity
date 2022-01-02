using System;
using UnityEngine;

[Serializable]
public struct Dialogue
{
    /// <summary>
    /// Initialise values on construction.
    /// </summary>
    /// <param name="narrativeScript">An Ink narrative script</param>
    public Dialogue(TextAsset narrativeScript)
    {
        NarrativeScript = narrativeScript;
    }

    [field: Tooltip("Drag an Ink narrative script here.")]
    [field: SerializeField] public TextAsset NarrativeScript { get; private set; }
}