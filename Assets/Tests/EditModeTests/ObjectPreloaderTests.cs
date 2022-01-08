using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPreloaderTests
{
    private ObjectStorage _objectStorage;
    private ObjectPreloader _objectPreloader;
    private ResourceLoader _resourceLoader = new ResourceLoader();

    [SetUp]
    public void SetUp()
    {
        _objectStorage = new ObjectStorage();
        _objectPreloader = new ObjectPreloader(_objectStorage);
    }
    
    [Test]
    public void ObjectPreloaderCanLoadActors()
    {
        ObjectPreloaderCanLoadObjects<ActorData>("Actors", actorName => _objectPreloader.SetActiveActor(actorName));
    }
    
    [Test]
    public void ObjectPreloaderCanLoadEvidence()
    {
        ObjectPreloaderCanLoadObjects<Evidence>("Evidence", evidenceName => _objectPreloader.AddEvidence(evidenceName));
    }
    
    [Test]
    public void ObjectPreloaderCanLoadSongs()
    {
        ObjectPreloaderCanLoadObjects<AudioClip>("Audio/Music", songName => _objectPreloader.PlaySong(songName));
    }
    
    [Test]
    public void ObjectPreloaderCanLoadSfx()
    {
        ObjectPreloaderCanLoadObjects<AudioClip>("Audio/SFX", sfxName => _objectPreloader.PlaySfx(sfxName));
    }

    /// <summary>
    /// Loads all objects at a given path, attempts to use them in an action
    /// then checks to see if they have been added to the object storage.
    /// </summary>
    /// <param name="path">The path to load objects from</param>
    /// <param name="action">The action to perform on the objects</param>
    /// <typeparam name="T">The type of object to load.</typeparam>
    private void ObjectPreloaderCanLoadObjects<T>(string path, Action<string> action) where T : Object
    {
        var objects = _resourceLoader.LoadAll(path);

        foreach (var o in objects)
        {
            var obj = (T)o;
            action(obj.name);
            var storedActor = _objectStorage.GetObject<T>(obj.name);
            Assert.AreEqual(storedActor, obj);
        }
    }
}