using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ActorController : MonoBehaviour, IActorController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] private DirectorActionDecoder _directorActionDecoder;

    [FormerlySerializedAs("_actorDictionary")]
    [Tooltip("Drag an ActorDictionary instance here, containing every required character")]
    [SerializeField] private ActorInventory _actorInventory;

    private Actor _activeActor;
    private BGScene _activeScene;

    [SerializeField] private UnityEvent<ActorData> _onNewSpeakingActor;
    [SerializeField] private UnityEvent _onAnimationStarted;
    [SerializeField] private UnityEvent _onAnimationComplete;

    public bool Animating { get; set; }

    private ActorData _currentSpeakingActor;
    private SpeakingType _currentSpeakingType = SpeakingType.Speaking;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Actor Controller doesn't have an action decoder to attach to");
            return;
        }

        _directorActionDecoder.SetActorController(this);
    }

    /// <summary>
    /// Connect to an event that exposes the active scene when it changes.
    /// </summary>
    /// <param name="newScene">New bg-scene that got set</param>
    public void OnSceneChanged(BGScene newScene)
    {
        _activeScene = newScene;
    }

    /// <summary>
    /// Set the target gameobject to be considered the active actor to manipulate when actions are triggered in  the dialogue script
    /// </summary>
    /// <param name="actor">Actor MonoBehaviour attached to the gameobject to be set as the active actor</param>
    public void SetActiveActorObject(Actor actor)
    {
        _activeActor = actor;
        if (actor != null)
            actor.AttachController(this);
    }

    /// <summary>
    /// Retrieves actor data from the actor dictionary and uses it set the active actor.
    /// Gives an exception if the actor is not found.
    /// </summary>
    /// <param name="actor">The name of the actor as it appears in the dictionary.
    /// Actors use the same name as their ActorData object.</param>
    public void SetActiveActor(string actor)
    {
        try
        { 
            if (_activeActor != null)
                _activeActor.SetActor(_actorInventory[actor]);
        }
        catch (KeyNotFoundException exception)
        {
            Debug.Log($"{exception.GetType().Name}: Actor {actor} was not found in actor dictionary");
        }
    }

    /// <summary>
    /// Sets the pose of the active actor.
    /// In working this is mostly the same as PlayEmotion without calling OnAnimationStarted so the system can continue without waiting for the animation to end.
    /// </summary>
    /// <param name="pose"></param>
    public void SetPose(string pose)
    {
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
    public void PlayEmotion(string emotion)
    {
        if (_activeActor == null)
        {
            Debug.LogError("Actor has not been assigned");
            _onAnimationComplete.Invoke();
            return;
        }
        _onAnimationStarted.Invoke();
        Animating = true;
        _activeActor.PlayAnimation(emotion);
    }

    /// <summary>
    /// Sets the active speaker in the scene, changing the name shown.
    /// </summary>
    /// <param name="actor">Target actor. This gets the correct name and color from the list of existing actors.</param>
    public void SetActiveSpeaker(string actor)
    {
        try
        {
            _currentSpeakingActor = _actorInventory[actor];
            _onNewSpeakingActor.Invoke(_currentSpeakingActor);
        }
        catch (KeyNotFoundException exception)
        {
            _currentSpeakingActor = null;
            Debug.Log($"{exception.GetType().Name}: Actor {actor} was not found in actor dictionary");
        }
    }

    /// <summary>
    /// Makes the active actor speak animation run if the active actor is the speaking actor.
    /// </summary>
    public void StartTalking()
    {
        if (_activeActor == null)
        {
            return;
        }

        if (_currentSpeakingType == SpeakingType.Thinking)
        {
            return;
        }

        if ( _activeActor.MatchesActorData(_currentSpeakingActor))
        {
            _activeActor.SetTalking(true);
        }
            
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
        if (Animating)
        {
            Animating = false;
            _onAnimationComplete.Invoke();
        }
        
    }

    /// <summary>
    /// Sets the speaking type of the sentences shown, so the actor can react appropriately.
    /// </summary>
    /// <param name="speakingType">Type of speaking the next sentence is gonna be.</param>
    public void SetSpeakingType(SpeakingType speakingType)
    {
        _currentSpeakingType = speakingType;
    }

    /// <summary>
    /// Sets an actor inside a slot in the scene, if the active bg-scene has support for slots.
    /// </summary>
    /// <param name="actor">Target actor</param>
    /// <param name="oneBasedSlotIndex">Target slot, 1 based</param>
    public void AssignActorToSlot(string actor, int oneBasedSlotIndex)
    {
        if (_activeScene == null)
        {
            Debug.LogError("Can't assign actor to slot: No active scene");
            return;
        }

        if (!_activeScene.SupportsActorSlots())
        {
            Debug.LogError("Can't assign actor to slot: This scene has no support for slots");
            return;
        }

        Actor tempActor = _activeScene.GetActorAtSlot(oneBasedSlotIndex);

        try
        {
            tempActor.SetActor(_actorInventory[actor]);
        }
        catch (KeyNotFoundException exception)
        {
            tempActor.SetActor(null);
            Debug.Log($"{exception.GetType().Name}: Actor {actor} was not found in actor dictionary");
        }
    }
}
