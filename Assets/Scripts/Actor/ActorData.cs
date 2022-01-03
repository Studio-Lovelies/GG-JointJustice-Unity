using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor Data", menuName = "Actors/Actor Data")]
public class ActorData : ScriptableObject, ICourtRecordObject
{
    private enum AgeCertainty {
        Known,
        Guess,
        Unknown
    }

    [field: SerializeField, Tooltip("The actor's sprite that will appear in the profiles menu.")]
    public Sprite Profile { get; private set; }

    [field: SerializeField, Tooltip("Name displayed when referring to this actor or when they are speaking.")]
    public string DisplayName { get; private set; }

    [field: SerializeField, Tooltip("The age of the actor displayed in the profiles menu.")]
    public int Age { get; private set; }

    [SerializeField, Tooltip("Whether this age is accurate (Known), maybe accurate (Guess), or not accurate (Unknown)")]
    private AgeCertainty _ageCertainty;
    
    [field: SerializeField, Tooltip("Color for the background of the name when speaking.")]
    public Color DisplayColor { get; private set; }
    
    [field: SerializeField, TextArea, Tooltip("A description of the actor that will appear in the profiles menu.")]
    public string Bio { get; private set; }

    [field: SerializeField, Tooltip("The animator controllers that this actor uses.")]
    public RuntimeAnimatorController AnimatorController { get; private set; }

    [field: SerializeField, Tooltip("Drag sprites for any shout variations this actor has here.")]
    public Pair<Sprite, AudioClip>[] ShoutVariants { get; private set; }
    
    [field: SerializeField, Tooltip("The sound that plays when this actor talks.")]
    public AudioClip DialogueChirp { get; private set; }

    public string InstanceName => name;
    public string CourtRecordName => GenerateNameWithAge();
    public Sprite Icon => Profile;
    public string Description => Bio;

    /// <summary>
    /// Generates a string with the actor's age and age, used in the evidence menu.
    /// Handles what should be written when an actors name is unknown, or uncertain.
    /// </summary>
    /// <returns>The name and age of the actor as a string.</returns>
    private string GenerateNameWithAge()
    {
        string age = _ageCertainty switch {
            AgeCertainty.Known => Age.ToString(),
            AgeCertainty.Guess => $"Maybe {Age}?",
            AgeCertainty.Unknown => "???",
            _ => throw new ArgumentOutOfRangeException()
        };

        return $"{DisplayName} (Age: {age})";
    }
}
