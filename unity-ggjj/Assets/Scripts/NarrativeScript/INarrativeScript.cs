using Ink.Runtime;
using UnityEngine;

public interface INarrativeScript
{
    TextAsset Script { get; }
    IObjectStorage ObjectStorage { get; }
    Story Story { get; }
}