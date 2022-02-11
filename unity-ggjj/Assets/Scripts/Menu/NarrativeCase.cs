using System;
using UnityEngine;

[Serializable]
public struct NarrativeCase
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite PreviewImage { get; private set; }
    [field: SerializeField] public TextAsset[] Chapters { get; private set; }
}