using System.Linq;
using UnityEngine;

public class Shout
{
    public static string[] NormalShoutNames { get; } =
    {
        "Objection", "HoldIt", "TakeThat"
    };

    public Sprite Sprite { get; }
    public AudioClip AudioClip { get; }
    
    public Shout(string shoutName, Pair<Sprite, AudioClip>[] variants, bool allowRandomShouts, float randomShoutChance)
    {
        var specialVariants = GetSpecialVariants(variants);
        var pair = ShouldPlayRandomVariant(allowRandomShouts, specialVariants, randomShoutChance)
            ? GetRandomSpecialVariant(specialVariants)
            : GetVariant(variants, shoutName);
        Sprite = pair.Item1;
        AudioClip = pair.Item2;
    }

    /// <summary>
    /// Determines whether a random shout should be played, depending on
    /// if random shouts are allowed, if there are random shouts that
    /// can be played, and if a randomly generated number is less than randomShoutChance.
    /// </summary>
    /// <param name="allowRandomShouts">Whether random shouts should be allowed (true) or not (false).</param>
    /// <param name="specialVariants">An array of special shout variants to get random shouts from.</param>
    /// <param name="randomShoutChance">The chance to play a random shout.</param>
    /// <returns>Whether a random shout should be played (true) or not (false).</returns>
    private static bool ShouldPlayRandomVariant(bool allowRandomShouts, Pair<Sprite, AudioClip>[] specialVariants, float randomShoutChance)
    {
        return allowRandomShouts && specialVariants.Length > 0 && Random.Range(0f, 1f) < randomShoutChance;
    }
    
    /// <summary>
    /// Returns an array of all the shout variants that are not in the NormalShoutNames array
    /// </summary>
    ///  /// <param name="variants">The array to check for special variants.</param>
    /// <returns>The array of special variants created.</returns>
    private static Pair<Sprite, AudioClip>[] GetSpecialVariants(Pair<Sprite, AudioClip>[] variants)
    {
        return variants.Where(pair => NormalShoutNames.All(normalShoutName => normalShoutName  != pair.Item1.name)).ToArray();
    }

    /// <summary>
    /// Randomly generates a special shout variant.
    /// </summary>
    /// <param name="specialVariants">The array to get shout variants from.</param>
    /// <returns>The random variant generated.</returns>
    private static Pair<Sprite, AudioClip> GetRandomSpecialVariant(Pair<Sprite, AudioClip>[] specialVariants)
    {
        return specialVariants[Random.Range(0, specialVariants.Length)];
    }

    /// <summary>
    /// Gets a shout variant by name.
    /// </summary>
    /// <param name="variants">The array of possible shout variants.</param>
    /// <param name="shoutName">The name of the variant to get.</param>
    /// <returns>The shout variant with the name specified.</returns>
    private static Pair<Sprite, AudioClip> GetVariant(Pair<Sprite, AudioClip>[] variants, string shoutName)
    {
        return variants.First(pair => pair.Item1.name == shoutName);
    }
}