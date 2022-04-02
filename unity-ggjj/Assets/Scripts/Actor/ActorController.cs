using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] private NarrativeGameState _narrativeGameState;

    [Tooltip("Attach the NameBox here")]
    [SerializeField] private NameBox _nameBox;

    private readonly Dictionary<ActorData, Actor> _actorDataToActor = new Dictionary<ActorData, Actor>();
    private Actor _activeActor;
    private BGScene _activeScene;
    private Actor _currentSpeakingActor;
    private SpeakingType _currentSpeakingType = SpeakingType.Speaking;

    public bool Animating { get; set; }
    public ActorData CurrentSpeakingActorData { get; private set; }

    /// <summary>
    /// Connect to an event that exposes the active scene when it changes.
    /// </summary>
    /// <param name="newScene">New bg-scene that got set</param>
    public void OnSceneChanged(BGScene newScene)
    {
        _activeScene = newScene;
    }

    /// <summary>
    /// Set the target GameObject to be considered the active actor to manipulate when actions are triggered in  the dialogue script
    /// </summary>
    /// <param name="actor">Actor MonoBehaviour attached to the GameObject to be set as the active actor</param>
    public void SetActiveActorObject(Actor actor)
    {
        _activeActor = actor;
        _currentSpeakingType = SpeakingType.Speaking;
        if (actor != null)
        {
            actor.AttachController(this);
        }
    }


    /// <summary>
    /// Retrieves actor data from the actor dictionary and uses it set the active actor.
    /// Gives an exception if the actor is not found.
    /// </summary>
    /// <param name="actor">The name of the actor as it appears in the dictionary.
    /// Actors use the same name as their ActorData object.</param>
    public void SetActiveActor(string actor)
    {
        var targetActorData = FindActorDataInInventory(actor);
        if (_activeActor != null)
        {
            _activeActor.ActorData = targetActorData;
            SetActorInLookupTable(targetActorData, _activeActor);
        }
    }

    /// <summary>
    /// Sets actor to the lookup table so we can go from ActorData -> Actor
    /// </summary>
    /// <param name="key">ActorData that corresponds to the actor</param>
    /// <param name="value">Actor that corresponds to the actorData</param>
    private void SetActorInLookupTable(ActorData key, Actor value)
    {
        _actorDataToActor[key] = value;
    }

    /// <summary>
    /// Find ActorData in ActorInventory.
    /// </summary>
    /// <param name="actorName"></param>
    /// <returns></returns>
    private ActorData FindActorDataInInventory(string actorName)
    {
        try
        {
            return _narrativeGameState.ObjectStorage.GetObject<ActorData>(actorName);
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Actor {actorName} was not found in the actor dictionary");
        }
    }

    /// <summary>
    /// Find Actor based on ActorData in ActorInventory
    /// </summary>
    /// <param name="actorName">Name of the ActorData object to find the actor by</param>
    /// <returns>Instance of <see cref="Actor"/> the actor is currently assigned to</returns>
    private Actor FindActorInInventory(string actorName)
    {
        var actorData = FindActorDataInInventory(actorName);
        if (actorData == null || !_actorDataToActor.ContainsKey(actorData))
        {
            throw new KeyNotFoundException($"No actor with the name '{actorName}' could be found\r\nMake sure both an `Actor` ScriptableObject with that name exists and it's assigned to a slot in the currently active scene before calling this method");
        }
        return _actorDataToActor[actorData];
    }

    /// <summary>
    /// Sets the pose of the active actor.
    /// In working this is mostly the same as PlayEmotion without calling OnAnimationStarted so the system can continue without waiting for the animation to end.
    /// </summary>
    /// <param name="pose">Name of the pose to execute</param>
    /// <param name="actorName">Optional name of another actor to run this animation on (defaults to <see cref="_activeActor"/> if not set)</param>
    public void SetPose(string pose, string actorName = null)
    {
        if (!string.IsNullOrEmpty(actorName) && FindActorInInventory(actorName) != _activeActor)
        {
            var actor = FindActorInInventory(actorName);
            actor.PlayAnimation(pose);
            return;
        }

        if (_activeActor == null)
        {
            Debug.LogError("Actor has not been assigned");
            return;
        }

        _activeActor.PlayAnimation(pose);
    }

    /// <summary>
    /// Plays the animation of an actor by playing the specified animation on the actor's animator.
    /// Flags the system as busy so it waits for the animation to end.
    /// </summary>
    /// <param name="emotion">The emotion to play.</param>
    /// <param name="actorName">Optional name of another actor to run this animation on (defaults to <see cref="_activeActor"/> if not set)</param>
    public void PlayEmotion(string emotion, string actorName = null)
    {
        if (string.IsNullOrEmpty(actorName) || FindActorInInventory(actorName) == _activeActor)
        {
            if (_activeActor == null)
            {
                Debug.LogError("Actor has not been assigned");
                OnAnimationDone();
                return;
            }
            OnAnimationStarted();
            _activeActor.PlayAnimation(emotion);
        }
        else
        {
            var actor = FindActorInInventory(actorName);
            actor.PlayAnimation(emotion);
        }
    }

    /// <summary>
    /// Sets the active speaker to the Narrator (which has no visible name / NameBox)
    /// </summary>
    public void SetActiveSpeakerToNarrator()
    {
        _currentSpeakingActor = null;
        _nameBox.SetSpeakerToNarrator();
    }

    /// <summary>
    /// Sets the active speaker in the scene, changing the name shown.
    /// </summary>
    /// <param name="actorName">Target actor. This gets the correct name and color from the list of existing actors.</param>
    /// <param name="speakingType">SpeakingType of the speaker.</param>
    public void SetActiveSpeaker(string actorName, SpeakingType speakingType)
    {
        try
        {
            ActorData actorData = _narrativeGameState.ObjectStorage.GetObject<ActorData>(actorName);
            _nameBox.SetSpeaker(actorData, speakingType);
            CurrentSpeakingActorData = actorData;
            _currentSpeakingActor = _actorDataToActor.ContainsKey(actorData) ? _actorDataToActor[actorData] : null;
            _currentSpeakingType = speakingType;
        }
        catch (KeyNotFoundException)
        {
            _currentSpeakingActor = null;
            throw new KeyNotFoundException($"Actor {actorName} was not found in actor dictionary");
        }
    }

    /// <summary>
    /// Makes the active actor speak animation run if the active actor is the speaking actor.
    /// </summary>
    public void StartTalking()
    {
        if (_activeActor == null || _currentSpeakingType == SpeakingType.Thinking || _currentSpeakingActor == null)
        {
            return;
        }

        _currentSpeakingActor.SetTalking(true);
    }

    /// <summary>
    /// Makes the active actor speak animation stop.
    /// </summary>
    public void StopTalking()
    {
        if (_activeActor == null)
        {
            return;
        }
        _activeActor.SetTalking(false);
    }

    /// <summary>
    /// Called by attached animations when the animation is done
    /// </summary>
    public void OnAnimationDone()
    {
        Animating = false;
        _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Waiting = false;
        _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Continue();
    }

    /// <summary>
    /// Sets an actor inside a slot in the scene, if the active bg-scene has support for slots.
    /// </summary>
    /// <param name="slotName">Name of an actor slot of the currently active scene</param>
    /// <param name="newActorName"></param>
    public void AssignActorToSlot(string slotName, string newActorName)
    {
        if (_activeScene == null)
        {
            throw new NotSupportedException("Can't assign actor to slot: No active scene");
        }

        var actorData = _narrativeGameState.ObjectStorage.GetObject<ActorData>(newActorName);
        var actorAtSlot = _activeScene.GetActorAtSlot(slotName);
        actorAtSlot.ActorData = actorData;
        SetActorInLookupTable(actorData, actorAtSlot);
    }

    /// <summary>
    /// Change the visibility of the sprite of the specified actor
    /// </summary>
    /// <param name="shouldShow">Whether to show (`true`) or hide (`false`) the actor</param>
    /// <param name="actorName">Name of the actor to change the visibility of</param>
    /// <exception cref="KeyNotFoundException">Thrown, if no actor with the specified name exists in this scene</exception>
    /// <exception cref="System.NullReferenceException">Thrown, if no Renderer exists on the actor with the specified name</exception>
    public void SetVisibility(bool shouldShow, ActorAssetName actorName)
    {
        (actorName == null ? _activeActor : FindActorInInventory(actorName)).Renderer.enabled = shouldShow;
    }

    /// <summary>
    /// Handles what happens when an animation starts
    /// </summary>
    private void OnAnimationStarted()
    {
        Animating = true;
        _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Waiting = true;
        _narrativeGameState.AppearingDialogueController.TextBoxHidden = true;
    }
}
