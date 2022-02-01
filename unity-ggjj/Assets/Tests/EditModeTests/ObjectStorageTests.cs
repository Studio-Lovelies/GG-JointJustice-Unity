using NUnit.Framework;
using UnityEngine;

public class ObjectStorageTests
{
    private ObjectStorage _objectStorage;

    [SetUp]
    public void SetUp()
    {
        _objectStorage = new ObjectStorage();
    }
    
    [Test]
    public void ObjectsCanBeAddedToObjectStorage()
    {
        Assert.AreEqual(0, _objectStorage.Count);
        _objectStorage.Add(Resources.Load("Actors/Arin"));
        Assert.AreEqual(1, _objectStorage.Count);
        _objectStorage.Add(Resources.Load("Actors/Dan"));
        Assert.AreEqual(2, _objectStorage.Count);
    }
    
    [Test]
    public void DuplicateObjectsCannotBeAddedToObjectStorage()
    {
        Assert.AreEqual(0, _objectStorage.Count);
        _objectStorage.Add(Resources.Load("Actors/Arin"));
        Assert.AreEqual(1, _objectStorage.Count);
        _objectStorage.Add(Resources.Load("Actors/Arin"));
        Assert.AreEqual(1, _objectStorage.Count);
    }
    
    [Test]
    public void ObjectsCanBeRetrievedFromObjectStorage()
    {
        const string ARIN = "Arin";
        const string BENT_COINS = "BentCoins";
        
        _objectStorage.Add(Resources.Load($"Actors/{ARIN}"));
        var storedActorData = _objectStorage.GetObject<ActorData>(ARIN);
        Assert.AreEqual(ARIN, storedActorData.name);
        
        _objectStorage.Add(Resources.Load($"Evidence/{BENT_COINS}"));
        var storedEvidence = _objectStorage.GetObject<Evidence>(BENT_COINS);
        Assert.AreEqual(BENT_COINS, storedEvidence.name);
    }
}
