using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DirectorActionDecoder : MonoBehaviour
{
    private const char ACTION_SIDE_SEPARATOR = ':';
    private const char ACTION_PARAMETER_SEPARATOR = ',';

    private IActorController _actorController;
    private ISceneController _sceneController;
    private IAudioController _audioController;
    private IEvidenceController _evidenceController;

    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    /// <summary>
    /// Called whenever a new action is executed (encountered and then forwarded here) in the script
    /// </summary>
    /// <param name="line">The full line in the script containing the action and parameters</param>
    public void OnNewActionLine(string line)
    {
        //Split into action and parameter
        string[] actionAndParam = line.Substring(1, line.Length - 2).Split(ACTION_SIDE_SEPARATOR); 

        if (actionAndParam.Length != 2)
        {
            Debug.LogError("Invalid action with line: " + line);
            _onActionDone.Invoke();
            return;
        }

        string action = actionAndParam[0];
        string parameters = actionAndParam[1];

        StopAllCoroutines();
        StartCoroutine(PerformActions(action, parameters));
    }

    private IEnumerator PerformActions(string action, string parameters)
    {
        switch(action)
        {
            //Actor controller
            case "ACTOR": SetActor(parameters); break;
            case "SHOWACTOR": SetActorVisibility(parameters); break;
            case "SPEAK": SetSpeaker(parameters); break;
            case "EMOTION": SetEmotion(parameters); break;
            //Audio controller
            case "PLAYSFX": PlaySFX(parameters); break;
            case "PLAYSONG": SetBGMusic(parameters); break;
            //Scene controller
            case "FADE_OUT": FadeOutScene(parameters); break;
            case "FADE_IN": FadeInScene(parameters); break;
            case "CAMERA_PAN": PanCamera(parameters); break;
            case "CAMERA_SET": SetCameraPosition(parameters); break;
            case "SHAKESCREEN": ShakeScreen(parameters); break;
            case "SCENE": SetScene(parameters); break;
            case "WAIT_FOR_ANIMATION": yield return StartCoroutine(WaitForAnimation()); break;
            case "WAIT": yield return StartCoroutine(Wait(parameters)); break;
            case "SHOW_ITEM": ShowItem(parameters); break;
            //Evidence controller
            case "ADD_EVIDENCE": AddEvidence(parameters); break;
            case "REMOVE_EVIDENCE": RemoveEvidence(parameters); break;
            case "ADD_RECORD": AddToCourtRecord(parameters); break;
            //Default
            default: Debug.LogError("Unknown action: " + action); break;
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
    private void SetActorVisibility(string showActor)
    {
        if (!HasActorController())
            return;

        if (bool.TryParse(showActor, out var shouldShow))
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
            Debug.LogError("Invalid parameter " + showActor + " for function SHOWACTOR");
        }
    }

    /// <summary>
    /// Set the speaker for the current and following lines, until a new speaker is set
    /// </summary>
    /// <param name="actor">Actor to make the speaker</param>
    private void SetSpeaker(string actor)
    {
        if (!HasActorController())
            return;

        _actorController.SetActiveSpeaker(actor);
    }

    /// <summary>
    /// Set the emotion of the current actor
    /// </summary>
    /// <param name="emotion">Emotion to display for the current actor</param>
    private void SetEmotion(string emotion)
    {
        if (!HasActorController())
            return;

        _actorController.SetEmotion(emotion);
    }
    #endregion

    #region SceneController
    /// <summary>
    /// Fades the scene in from black
    /// </summary>
    /// <param name="seconds">Amount of seconds the fade-in should take as a float</param>
    private void FadeInScene(string seconds)
    {
        if (!HasSceneController())
            return;

        if(float.TryParse(seconds, out var timeInSeconds))
        {
            _sceneController.FadeIn(timeInSeconds);
        }
        else
        {
            Debug.LogError("Invalid parameter " + seconds + " for function FADE_IN");
        }
    }

    /// <summary>
    /// Fades the scene to black
    /// </summary>
    /// <param name="seconds">Amount of seconds the fade-out should take as a float</param>
    private void FadeOutScene(string seconds)
    {
        if (!HasSceneController())
            return;

        if (float.TryParse(seconds, out var timeInSeconds))
        {
            _sceneController.FadeOut(timeInSeconds);
        }
        else
        {
            Debug.LogError("Invalid parameter " + seconds + " for function FADE_OUT");
        }
    }

    /// <summary>
    /// Shakes the screen
    /// </summary>
    /// <param name="intensity">Max displacement of the screen as a float</param>
    private void ShakeScreen(string intensity)
    {
        if (!HasSceneController())
            return;

        if (float.TryParse(intensity, out var intensityNumerical))
        {
            _sceneController.ShakeScreen(intensityNumerical);
        }
        else
        {
            Debug.LogError("Invalid parameter " + intensity + " for function SHAKESCREEN");
        }
    }

    /// <summary>
    /// Sets the scene (background, character location on screen, any props (probably prefabs))
    /// </summary>
    /// <param name="sceneName">Scene to change to</param>
    private void SetScene(string sceneName)
    {
        if (!HasSceneController())
            return;

        _sceneController.SetScene(sceneName);
    }

    /// <summary>
    /// Sets the camera position
    /// </summary>
    /// <param name="position">New camera coordinates in the "int x,int y" format</param>
    private void SetCameraPosition(string position)
    {
        if (!HasSceneController())
            return;

        string[] parameters = position.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length != 2)
        {
            Debug.LogError("Invalid amount of parameters for function CAMERA_SET");
            return;
        }

        if (int.TryParse(parameters[0], out var x) 
            && int.TryParse(parameters[1], out var y))
        {
            _sceneController.SetCameraPos(new Vector2Int(x,y));
        }
        else
        {
            Debug.LogError("Invalid paramater " + position + " for function CAMERA_SET");
        }

    }

    /// <summary>
    /// Pan the camera to a certain x,y position
    /// </summary>
    /// <param name="durationAndPosition">Duration the pan should take and the target coordinates in the "float seconds, int x, int y" format</param>
    private void PanCamera(string durationAndPosition)
    {
        if (!HasSceneController())
            return;

        string[] parameters = durationAndPosition.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length != 3)
        {
            Debug.LogError("Invalid amount of parameters for function CAMERA_PAN");
            return;
        }

        if (float.TryParse(parameters[0], out var duration) 
            && int.TryParse(parameters[1], out var x) 
            && int.TryParse(parameters[2], out var y))
        {
            _sceneController.PanCamera(duration, new Vector2Int(x, y));
        }
        else
        {
            Debug.LogError("Invalid paramater " + durationAndPosition + " for function CAMERA_PAN");
        }

    }

    /// <summary>
    /// Shows an item on the middle, left, or right side of the screen.
    /// </summary>
    /// <param name="itemNameAndPosition">Which item to show and where to show it, in the "string item, itemPosition pos" format</param>
    private void ShowItem(string itemNameAndPosition)
    {
        if (!HasSceneController())
            return;

        string[] parameters = itemNameAndPosition.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length != 2)
        {
            Debug.LogError("Invalid number of parameters for function SHOW_ITEM");
            return;
        }

        itemDisplayPosition pos;
        if (System.Enum.TryParse<itemDisplayPosition>(parameters[1], out pos))
        {
            _sceneController.ShowItem(parameters[0], pos);
        }
        else
        {
            Debug.LogError("Invalid parameter " + parameters[1] + " for function CAMERA_PAN");
        }
    }

    private IEnumerator WaitForAnimation()
    {
        _actorController.Animating = true;
        while (_actorController.Animating)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Waits seconds before automatically continuing.
    /// </summary>
    /// <param name="seconds">Amount of seconds to wait</param>
    private IEnumerator Wait(string seconds)
    {
        if (!HasSceneController())
            yield break;

        if (float.TryParse(seconds, out var secondsFloat))
        {
            yield return new WaitForSeconds(secondsFloat);
        }
        else
        {
            Debug.LogError("Invalid parameter " + seconds + " for function WAIT");
        }
    }

    #endregion
    
    #region AudioController
    /// <summary>
    /// Plays a sound effect
    /// </summary>
    /// <param name="sfx">Name of the sound effect</param>
    void PlaySFX(string sfx)
    {
        if (!HasAudioController())
            return;

        _audioController.PlaySFX(sfx);
    }

    /// <summary>
    /// Sets the background music
    /// </summary>
    /// <param name="songName">Name of the new song</param>
    private void SetBGMusic(string songName)
    {
        if (!HasAudioController())
            return;

        _audioController.PlaySong(songName);
    }
    #endregion

    #region EvidenceController
    void AddEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        _evidenceController.AddEvidence(evidence);
    }

    private void RemoveEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        _evidenceController.RemoveEvidence(evidence);
    }

    private void AddToCourtRecord(string actor)
    {
        if (!HasEvidenceController())
            return;

        _evidenceController.AddToCourtRecord(actor);
    }
    #endregion

    #region ControllerStuff
    /// <summary>
    /// Attach a new IActorController to the decoder
    /// </summary>
    /// <param name="newController">New action controller to be added</param>
    public void SetActorController(IActorController newController)
    {
        _actorController = newController;
    }

    /// <summary>
    /// Checks if the decoder has an actor controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an actor controller is connected</returns>
    private bool HasActorController()
    {
        if (_actorController == null)
        {
            Debug.LogError("No actor controller attached to the action decoder");
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
    /// Checks if the decoder has a scene controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether a scene controller is connected</returns>
    private bool HasSceneController()
    {
        if (_sceneController == null)
        {
            Debug.LogError("No scene controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Attach a new IAudioController to the decoder
    /// </summary>
    /// <param name="newController">New audio controller to be added</param>
    public void SetAudioController(IAudioController newController)
    {
        _audioController = newController;
    }

    /// <summary>
    /// Checks if the decoder has an audio controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an audio controller is connected</returns>
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
    /// Attach a new IEvidenceController to the decoder
    /// </summary>
    /// <param name="newController">New evidence controller to be added</param>
    public void SetEvidenceController(IEvidenceController newController)
    {
        _evidenceController = newController;
    }

    /// <summary>
    /// Checks if the decoder has an evidence controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an evidence controller is connected</returns>
    private bool HasEvidenceController()
    {
        if (_evidenceController == null)
        {
            Debug.LogError("No evidence controller attached to the action decoder");
            return false;
        }
        return true;
    }
    #endregion
}
