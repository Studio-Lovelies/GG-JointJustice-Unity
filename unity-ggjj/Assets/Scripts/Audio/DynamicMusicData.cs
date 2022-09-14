using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dynamic Music Data", menuName = "Dynamic Music/Dynamic Music Data")]
public class DynamicMusicData : ScriptableObject
{
    [Serializable]
    public struct MusicVariant
    {
        public string name;
        public AudioClip file;
    }

    [SerializeField]
    private AudioClip primary;
    public AudioClip Primary => primary;

    [SerializeField]
    private MusicVariant[] secondaryVariants;

    public AudioClip GetClipForVariant(string requestedVariantName)
    {
        return secondaryVariants.First(variant => variant.name == requestedVariantName).file;
    }
}
