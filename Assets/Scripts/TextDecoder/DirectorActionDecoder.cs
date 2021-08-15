using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DirectorActionDecoder : MonoBehaviour
{
    private const char ACTION_PARAMATER_SEPARATOR = ':';

    private IActorController _actorController;
    private ISceneController _sceneController;
    private IAudioController _audioController;

    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    public void OnNewActionLine(string line)
    {
        //Split into action and parameter
        string[] actionAndParam = line.Substring(1, line.Length - 2).Split(ACTION_PARAMATER_SEPARATOR); 

        if (actionAndParam.Length != 2)
        {
            Debug.LogError("Invalid action with line: " + line);
            _onActionDone.Invoke();
            return;
        }

        switch(actionAndParam[0])
        {
            case "ACTOR": SetActor(actionAndParam[1]); break;
            case "SHOWACTOR": ShowActor(actionAndParam[1]); break;
            case "SPEAK": SetSpeaker(actionAndParam[1]); break;
            case "EMOTION": SetEmotion(actionAndParam[1]); break;
            case "PLAYSFX": PlaySFX(actionAndParam[1]); break;
            case "PLAYSONG": SetBGMusic(actionAndParam[1]); break;
            default: Debug.LogError("Unknown action: " + actionAndParam[0]); break;
        }

        _onActionDone.Invoke(); //Called at the end when done
    }

    #region ActorController
    /// <summary>
    /// Sets the shown actor in the scene
    /// </summary>
    /// <param name="actor">Actor to be switched to</param>
    private void SetActor(string actor)
    {
        if (!HasActorController())
            return;

        _actorController.SetActiveActor(actor);
    }

    /// <summary>
    /// Shows or hides the actor based on the string parameter.
    /// </summary>
    /// <param name="showActor">Should contain true or false based on showing or hiding the actor respectively</param>
    private void ShowActor(string showActor)
    {
        if (!HasActorController())
            return;

        bool shouldShow;
        if (bool.TryParse(showActor, out shouldShow))
        {
            if(shouldShow)
            {
                _actorController.ShowActor();
            }
            else
            {
                _actorController.HideActor();
            }
        }
        else
        {
            Debug.LogError("Invalid paramater " + showActor + " for function SHOWACTOR");
        }
    }

    private void SetSpeaker(string actor)
    {
        if (!HasActorController())
            return;

        _actorController.SetActiveSpeaker(actor);
    }

    private void SetEmotion(string emotion)
    {
        if (!HasActorController())
            return;

        _actorController.SetEmotion(emotion);
    }
    #endregion

    #region SceneController

    #endregion

    #region AudioController
    void PlaySFX(string sfx)
    {
        if (!HasAudioController())
            return;

        _audioController.PlaySFX(sfx);
    }

    void SetBGMusic(string music)
    {
        if (!HasAudioController())
            return;

        _audioController.PlayBGSong(music);
    }
    #endregion

    /// <summary>
    /// Attach a new IActorController to the decoder
    /// </summary>
    /// <param name="newController">New action controller to be added</param>
    public void SetActorController(IActorController newController)
    {
        _actorController = newController;
    }

    private bool HasActorController()
    {
        if (_actorController == null)
        {
            Debug.LogError("No actor controller attached to the action decoder");
            return false;
        }
        return true;
    }

    private bool HasAudioController()
    {
        if (_audioController == null)
        {
            Debug.LogError("No audio controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Attach a new ISceneController to the decoder
    /// </summary>
    /// <param name="newController">New scene controller to be added</param>
    public void SetSceneController(ISceneController newController)
    {
        _sceneController = newController;
    }

    /// <summary>
    /// Attach a new IAudioController to the decoder
    /// </summary>
    /// <param name="newController">New audio controller to be added</param>
    public void SetAudioController(IAudioController newController)
    {
        _audioController = newController;
    }
}
