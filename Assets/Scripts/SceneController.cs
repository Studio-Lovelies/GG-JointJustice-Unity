using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneController : MonoBehaviour, ISceneController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("This event is called when a wait action is finished. Subscribe OnNextLine method to this.")]
    [SerializeField] private UnityEvent _onWaitComplete;
    
    private Coroutine _waitCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Scene Controller doesn't have a action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.SetSceneController(this);
        }

    }

    public void FadeIn(float seconds)
    {
        Debug.LogWarning("FadeIn not implemented");
    }

    public void FadeOut(float seconds)
    {
        Debug.LogWarning("FadeOut not implemented");
    }

    public void PanCamera(float seconds, Vector2Int position)
    {
        Debug.LogWarning("PanCamera not implemented");
    }

    public void SetScene(string background)
    {
        Debug.LogWarning("SetBackground not implemented");
    }

    public void SetCameraPos(Vector2Int position)
    {
        Debug.LogWarning("SetCameraPos not implemented");
    }

    public void ShakeScreen(float intensity)
    {
        Debug.LogWarning("ShakeScreen not implemented");
    }

    public void ShowItem(string item, itemDisplayPosition position)
    {
        Debug.LogWarning("ShowItem not implemented");
    }
    
    public void ShowActor()
    {
        Debug.LogWarning("ShowActor not implemented");
    }
    
    public void HideActor()
    {
        Debug.LogWarning("HideActor not implemented");
    }

    /// <summary>
    /// Starts and caches a coroutine to wait for a specific amount of time.
    /// Called by DirectorActionController when a WAIT action is read.
    /// </summary>
    /// <param name="seconds"></param>
    public void Wait(float seconds)
    {
        _waitCoroutine = StartCoroutine(WaitCoroutine(seconds));
    }

    /// <summary>
    /// Coroutine to handle waiting for a specific amount of time.
    /// </summary>
    /// <param name="seconds">The time to wait in seconds.</param>
    private IEnumerator WaitCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _onWaitComplete?.Invoke();
    }

    /// <summary>
    /// Cancels a wait coroutine. Used to stop the the coroutine from
    /// continuing if more actions are read before waiting is complete.
    /// Subscribe this to DialogueController's onNewActionLine and onNewSpokenLine events.
    /// Make sure the event is ABOVE DirectorActionController in the subscribed events list.
    /// </summary>
    public void CancelWaitCoroutine()
    {
        if (_waitCoroutine == null)
            return;
        
        StopCoroutine(_waitCoroutine);
        _waitCoroutine = null;
    }
}
