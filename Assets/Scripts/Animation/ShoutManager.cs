using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShoutManager
{
    private static readonly string[] NormalShoutNames = {
        "Objection", "HoldIt", "TakeThat"
    };
    private readonly Pair<Sprite, AudioClip>[] _shoutVariants;
    private readonly float _randomShoutChance;

    public ShoutManager(Pair<Sprite, AudioClip>[] shoutVariants, float randomShoutChance)
    {
        _shoutVariants = shoutVariants;
        _randomShoutChance = randomShoutChance;
    }

    /// <summary>
    /// Returns the data required to play a specified Shout
    /// Randomly returns a variant Shout instead
    /// </summary>
    /// <param name="shoutName">The name of the shout to get</param>
    /// <returns>The Pair containing the Sprite and AudioClip for the specified shout.</returns>
    public Pair<Sprite, AudioClip> GetShout(string shoutName)
    {
        return ShouldReturnRandomVariant()
            ? GetRandomVariant()
            : _shoutVariants.First(pair => pair.Item1.name == shoutName);
    }

    /// <summary>
    /// Checks if there are shout variants in the that can be played.
    /// Randomly decides if one of these shouts should be played.
    /// </summary>
    /// <returns>Whether a random variant should be played (true) or not (false)</returns>
    private bool ShouldReturnRandomVariant()
    {
        return _shoutVariants.Any(pair => NormalShoutNames.Any(name => name != pair.Item1.name))
            && Random.Range(0f, 1f) < _randomShoutChance;
    }

    /// <summary>
    /// Gets a random variant from the array of shouts
    /// </summary>
    /// <returns>The random variant generated.</returns>
    private Pair<Sprite, AudioClip> GetRandomVariant()
    {
        var variants = _shoutVariants.Where(pair => NormalShoutNames.Any(name => name != pair.Item1.name)).ToArray();
        return variants[Random.Range(0, variants.Length)];
    }
}