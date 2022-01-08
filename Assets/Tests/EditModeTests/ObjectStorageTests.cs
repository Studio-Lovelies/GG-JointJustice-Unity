using NUnit.Framework;

public class ObjectStorageTests
{
    private ObjectStorage _objectStorage;
    private ResourceLoader _resourceLoader = new ResourceLoader();

    [SetUp]
    public void SetUp()
    {
        _objectStorage = new ObjectStorage();
    }
    
    [Test]
    public void ObjectsCanBeAddedToObjectStorage()
    {
        Assert.AreEqual(0, _objectStorage.Count);
        _objectStorage.Add(_resourceLoader.Load("Actors/Arin"));
        Assert.AreEqual(1, _objectStorage.Count);
        _objectStorage.Add(_resourceLoader.Load("Actors/Dan"));
        Assert.AreEqual(2, _objectStorage.Count);
    }
    
    [Test]
    public void DuplicateObjectsCannotBeAddedToObjectStorage()
    {
        Assert.AreEqual(0, _objectStorage.Count);
        _objectStorage.Add(_resourceLoader.Load("Actors/Arin"));
        Assert.AreEqual(1, _objectStorage.Count);
        _objectStorage.Add(_resourceLoader.Load("Actors/Arin"));
        Assert.AreEqual(1, _objectStorage.Count);
    }
    
    [Test]
    public void ObjectsCanBeRetrievedFromObjectStorage()
    {
        const string ARIN = "Arin";
        const string BENT_COINS = "Bent_Coins";
        
        _objectStorage.Add(_resourceLoader.Load($"Actors/{ARIN}"));
        var storedActorData = _objectStorage.GetObject<ActorData>(ARIN);
        Assert.AreEqual(ARIN, storedActorData.name);
        
        _objectStorage.Add(_resourceLoader.Load($"Evidence/{BENT_COINS}"));
        var storedEvidence = _objectStorage.GetObject<Evidence>(BENT_COINS);
        Assert.AreEqual(BENT_COINS, storedEvidence.name);
    }
}
