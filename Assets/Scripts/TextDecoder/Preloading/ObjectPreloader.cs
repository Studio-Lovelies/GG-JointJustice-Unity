using System;
using UnityEngine;

/// <summary>
/// Assigned to an ActionDecoder in place of the usual controller interfaces.
/// Action decoder calls the methods which are used to load any required objects.
/// Objects are then stored in the assign ObjectStorage object.
/// </summary>
public class ObjectPreloader : ActionDecoderBase
{
    private readonly IObjectLoader _objectLoader = new ResourceLoader();
    private readonly ObjectStorage _objectStorage;

    public ObjectPreloader(ObjectStorage objectStorage)
    {
        _objectStorage = objectStorage;
    }

    #region Actions
    protected override void ADD_EVIDENCE(AssetName evidenceName)
    {
        LoadEvidence(evidenceName);
    }

    protected override void ADD_RECORD(AssetName actorName)
    {
        LoadActor(actorName);
    }

    protected override void PLAY_SFX(AssetName sfx)
    {
        LoadObject($"Audio/SFX/{sfx}");
    }

    protected override void PLAY_SONG(AssetName songName)
    {
        LoadObject($"Audio/Music/{songName}");
    }

    protected override void SCENE(AssetName sceneName)
    {
        LoadScene(sceneName);
    }

    protected override void SHOW_ITEM(AssetName itemName, ItemDisplayPosition itemPos)
    {
        LoadEvidence(itemName);
    }

    protected override void ACTOR(AssetName actorName)
    {
        LoadActor(actorName);
    }

    protected override void SPEAK(AssetName actorName)
    {
        LoadActor(actorName);
    }

    protected override void SPEAK_UNKNOWN(AssetName actorName)
    {
        LoadActor(actorName);
    }

    protected override void THINK(AssetName actorName)
    {
        LoadActor(actorName);
    }

    protected override void SET_ACTOR_POSITION(int oneBasedSlotIndex, AssetName actorName)
    {
        LoadActor(actorName);
    }
    #endregion
    
    private void LoadActor(string actorName)
    {
        LoadObject($"Actors/{actorName}");
    }
    
    private void LoadEvidence(string evidenceName)
    {
        LoadObject($"Evidence/{evidenceName}");
    }
    
    private void LoadScene(string sceneName)
    {
    }

    /// <summary>
    /// Uses the assigned IObjectLoader to load an object and add it to the object storage
    /// </summary>
    /// <param name="path">The path to the object to load</param>
    private void LoadObject(string path)
    {
        try
        {
            _objectStorage.Add(_objectLoader.Load(path));
        }
        catch (NullReferenceException exception)
        {
            Debug.LogWarning($"{exception.GetType().Name}: Object at path {path} could not be loaded");
        }
    }
}