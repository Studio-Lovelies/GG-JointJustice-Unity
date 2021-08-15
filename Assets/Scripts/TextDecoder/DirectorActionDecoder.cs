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

    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    /// <summary>
    /// Called whenever a new action is performed in the script
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

        switch(actionAndParam[0])
        {
            //Actor controller
            case "ACTOR": SetActor(parameters); break;
            case "SHOWACTOR": ShowActor(parameters); break;
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
            case "BACKGROUND": SetBackground(parameters); break;
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
    void FadeInScene(string seconds)
    {
        if (!HasSceneController())
            return;

        float timeInSeconds;

        if(float.TryParse(seconds, out timeInSeconds))
        {
            _sceneController.FadeIn(timeInSeconds);
        }
        else
        {
            Debug.LogError("Invalid paramater " + seconds + " for function FADE_IN");
        }
    }

    /// <summary>
    /// Fades the scene to black
    /// </summary>
    /// <param name="seconds">Amount of seconds the fade-out should take as a float</param>
    void FadeOutScene(string seconds)
    {
        if (!HasSceneController())
            return;

        float timeInSeconds;

        if (float.TryParse(seconds, out timeInSeconds))
        {
            _sceneController.FadeOut(timeInSeconds);
        }
        else
        {
            Debug.LogError("Invalid paramater " + seconds + " for function FADE_OUT");
        }
    }

    /// <summary>
    /// Shakes the screen
    /// </summary>
    /// <param name="intensity">Max displacement of the screen as a float</param>
    void ShakeScreen(string intensity)
    {
        if (!HasSceneController())
            return;

        float intensityNumerical;

        if (float.TryParse(intensity, out intensityNumerical))
        {
            _sceneController.ShakeScreen(intensityNumerical);
        }
        else
        {
            Debug.LogError("Invalid paramater " + intensity + " for function SHAKESCREEN");
        }
    }

    /// <summary>
    /// Sets the background scene
    /// </summary>
    /// <param name="background">Background scene to change to</param>
    void SetBackground(string background)
    {
        if (!HasSceneController())
            return;

        _sceneController.SetBackground(background);
    }

    /// <summary>
    /// Sets the camera position
    /// </summary>
    /// <param name="coordinates">New camera coordinates in the "int x,int y" format</param>
    void SetCameraPosition(string coordinates)
    {
        if (!HasSceneController())
            return;

        string[] parameters = coordinates.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length != 2)
        {
            Debug.LogError("Invalid amount of parameters for function CAMERA_SET");
            return;
        }

        int x;
        int y;

        if (int.TryParse(parameters[0], out x) 
            && int.TryParse(parameters[1], out y))
        {
            _sceneController.SetCameraPos(x, y);
        }
        else
        {
            Debug.LogError("Invalid paramater " + coordinates + " for function CAMERA_SET");
        }

    }

    /// <summary>
    /// Pan the camera to a certain x,y position
    /// </summary>
    /// <param name="stringParameters">Duration the pan should take and the target coordinates in the "float seconds, int x, int y" format</param>
    void PanCamera(string stringParameters)
    {
        if (!HasSceneController())
            return;

        string[] parameters = stringParameters.Split(ACTION_PARAMETER_SEPARATOR);

        if (parameters.Length != 3)
        {
            Debug.LogError("Invalid amount of parameters for function CAMERA_PAN");
            return;
        }

        float duration;
        int x;
        int y;

        if (float.TryParse(parameters[0], out duration) 
            && int.TryParse(parameters[1], out x) 
            && int.TryParse(parameters[2], out y))
        {
            _sceneController.PanCamera(duration, x, y);
        }
        else
        {
            Debug.LogError("Invalid paramater " + stringParameters + " for function CAMERA_PAN");
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
    /// <param name="music">Name of the new song</param>
    void SetBGMusic(string music)
    {
        if (!HasAudioController())
            return;

        _audioController.PlayBGSong(music);
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
    #endregion
}
