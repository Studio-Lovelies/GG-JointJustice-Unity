using System;
using Ink.Runtime;
using UnityEngine;

[System.Serializable]
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
    
    public GameMode ScriptMode
    {
        get
        {
            const string expectedFileStart = "&MODE:";
            var firstLine = new Story(NarrativeScript.text).Continue().Trim();
            var availableModes = ((GameMode[])Enum.GetValues(typeof(GameMode)));
            if (!firstLine.StartsWith("&MODE:"))
            {
                throw new NotSupportedException($"The first line of each .ink script needs to begin with either '{expectedFileStart}{string.Join($"','{expectedFileStart}", availableModes)}'\r\nLine: {firstLine}");
            }

            var modeParameter = firstLine.Substring(expectedFileStart.Length);
            foreach (GameMode mode in availableModes)
            {
                if (mode.ToString() == modeParameter)
                {
                    return mode;
                }
            }

            throw new NotSupportedException($"The mode '{modeParameter}' is not supported. (Currently supported values: '{expectedFileStart}{string.Join($"','{expectedFileStart}", availableModes)}')\r\n Line: {firstLine}");
        }
    }
}