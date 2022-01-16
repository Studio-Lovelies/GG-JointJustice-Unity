using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class ShoutTests
{
    private const int VARIANTS_LENGTH = 8;
    
    private readonly Pair<Sprite, AudioClip>[] _shoutVariants = new Pair<Sprite, AudioClip>[VARIANTS_LENGTH];

    private static int SpecialVariantsLength => VARIANTS_LENGTH - Shout.NormalShoutNames.Length;
    
    [SetUp]
    public void SetUp()
    {
        for (int i = 0; i < VARIANTS_LENGTH - Shout.NormalShoutNames.Length; i++)
        {
            _shoutVariants[i] = new Pair<Sprite, AudioClip>
            {
                Item1 = Sprite.Create(new Texture2D(0, 0), new Rect(), new Vector2(0, 0)),
                Item2 = AudioClip.Create(i.ToString(), 1, 1, 1000, false)
            };
            _shoutVariants[i].Item1.name = i.ToString();
        }

        for (int i = 0; i < Shout.NormalShoutNames.Length; i++)
        {
            _shoutVariants[i + SpecialVariantsLength] = new Pair<Sprite, AudioClip>
            {
                Item1 = Sprite.Create(new Texture2D(0, 0), new Rect(), new Vector2(0, 0)),
                Item2 = AudioClip.Create(Shout.NormalShoutNames[i], 1, 1, 1000, false)
            };
            _shoutVariants[i + SpecialVariantsLength].Item1.name = Shout.NormalShoutNames[i];
        }
    }
    
    [Test]
    public void ShoutsCanBeAccessedByName()
    {
        for (int i = 0; i < SpecialVariantsLength; i++)
        {
            var shout = new Shout(i.ToString(), _shoutVariants, false, 0);
            Assert.AreEqual(i.ToString(), shout.Sprite.name);
            Assert.AreEqual(i.ToString(), shout.AudioClip.name);
        }

        foreach (var shoutName in Shout.NormalShoutNames)
        {
            var shout = new Shout(shoutName, _shoutVariants, false, 0);
            Assert.AreEqual(shoutName, shout.Sprite.name);
            Assert.AreEqual(shoutName, shout.AudioClip.name);
        }
    }

    [Test]
    public void RandomVariantsCanBePlayed()
    {
        var shout = new Shout("", _shoutVariants, true, 1);
        Assert.IsTrue(_shoutVariants.Any(variant => shout.Sprite.name == variant.Item1.name && Shout.NormalShoutNames.All(normalShout => variant.Item1.name != normalShout)));
    }

    [Test]
    public void CannotPlaySpecialVariantWhenNoSpecialVariantsAvailable()
    {
        foreach (var shoutName in Shout.NormalShoutNames)
        {
            var shout = new Shout(shoutName, _shoutVariants.Where(variant => Shout.NormalShoutNames.Any(normalShoutName => normalShoutName == variant.Item1.name)).ToArray(), true, 1);
            Assert.AreEqual(shoutName, shout.Sprite.name);
        }
    }
}