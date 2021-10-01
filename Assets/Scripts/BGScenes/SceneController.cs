using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SceneController : MonoBehaviour, ISceneController
{
    [Tooltip("Pixels per unit of the basic ")]
    [SerializeField] private int _pixelsPerUnit = 100;

    [Tooltip("List of BG scenes in the unity scene, needs to be dragged here for every scene")]
    [SerializeField] private BGSceneList _sceneList;
    
    [Tooltip("Drag a FadeToImageController here.")]
    [SerializeField] private ImageFader _imageFader;

    [Tooltip("Drag an ItemDisplay component here.")]
    [SerializeField] private ItemDisplay _itemDisplay;

    [Tooltip("Drag an EvidenceInventory component here")]
    [SerializeField] private EvidenceInventory _evidenceInventory;
    
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] private DirectorActionDecoder _directorActionDecoder;

    [Header("Events")]
    [Tooltip("This event is called when a wait action is started.")]
    [SerializeField] private UnityEvent _onWaitStart;

    [Tooltip("This event is called when a wait action is finished.")]
    [SerializeField] private UnityEvent _onWaitComplete;

    [Tooltip("Event that gets called when the actor displayed on screen changes")]
    [SerializeField] private UnityEvent<Actor> _onActorChanged;

    [Tooltip("Event that gets called when the active bg-scene changes")]
    [SerializeField] private UnityEvent<BGScene> _onSceneChanged;

    private Coroutine _waitCoroutine;

    private BGScene _activeScene;
    
    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Start()
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

    /// <summary>
    /// Fades an image from opaque to transparent. Used to fade in to a scene from a black screen.
    /// </summary>
    /// <param name="seconds">The number of seconds it should take to fade.</param>
    public void FadeIn(float seconds)
    {
        if (_imageFader == null)
        {
            Debug.LogError($"Could not begin fade in. {name} does not have a FadeToImageTransition component attached.");
            return;
        }
        
        _onWaitStart.Invoke();
        _imageFader.StartFade(1, 0, seconds, _onWaitComplete);
    }

    /// <summary>
    /// Fades an image from transparent to opaque. Used to fade out from a scene to a black screen.
    /// </summary>
    /// <param name="seconds">The number of seconds it should take to fade.</param>
    public void FadeOut(float seconds)
    {
        if (_imageFader == null)
        {
            Debug.LogError($"Could not begin fade out. {name} does not have a FadeToImageTransition component attached.");
            return;
        }
        
        _onWaitStart.Invoke();
        _imageFader.StartFade(0, 1, seconds, _onWaitComplete);
    }

    /// <summary>
    /// Pans the camera position to the target position in pixels
    /// </summary>
    /// <param name="position">Target position in pixels</param>
    /// <param name="seconds">Time for the pan to take in seconds</param>
    public void PanCamera(float seconds, Vector2Int position)
    {
        StartCoroutine(PanToPosition(PixelPositionToUnitPosition(position), seconds));
    }

    /// <summary>
    /// Pans the camera position to the target position
    /// </summary>
    /// <param name="targetPos">Target position</param>
    /// <param name="timeInSeconds">Time for the pan to take in seconds</param>
    /// <returns>IEnumerator stuff for coroutine</returns>
    private IEnumerator PanToPosition(Vector2 targetPos, float timeInSeconds)
    {
        Vector2 startPos = _activeScene.transform.position;
        float percentagePassed = 0f;
        while (percentagePassed < 1)
        {
            percentagePassed += Time.deltaTime / timeInSeconds;
            _activeScene.transform.position = Vector2.Lerp(startPos, targetPos, percentagePassed);
            yield return null;
        }
    }

    /// <summary>
    /// Sets the new bg-scene based on the provided name and changes the active actor using an event.
    /// </summary>
    /// <param name="background">Target bg-scene</param>
    public void SetScene(string background)
    {
        _activeScene = _sceneList.SetScene(background);

        if (_activeScene != null)
        {
            _onActorChanged.Invoke(_activeScene.ActiveActor);
            _onSceneChanged.Invoke(_activeScene);
        }
    }

    /// <summary>
    /// Sets the camera position to the target position in pixels
    /// </summary>
    /// <param name="position">Target pixel position, top left</param>
    public void SetCameraPos(Vector2Int position)
    {
        _activeScene.transform.position = PixelPositionToUnitPosition(position);
    }

    public void ShakeScreen(float intensity)
    {
        Debug.LogWarning("ShakeScreen not implemented");
    }

    /// <summary>
    /// Gets an item from the evidence dictionary and shows it on the screen at a specified position.
    /// </summary>
    /// <param name="item">The name of the item to show.</param>
    /// <param name="position">The position of the item's image on the screen (left, middle, right).</param>
    public void ShowItem(string item, itemDisplayPosition position)
    {
        if (_evidenceInventory == null)
        {
            Debug.LogError($"Cannot show item, no EvidenceInventory component assigned to {name}.", gameObject);
            return;
        }

        if (_itemDisplay == null)
        {
            Debug.LogError($"Cannot show item, no ItemDisplay component assigned to {name}.", gameObject);
        }

        Evidence evidence = _evidenceInventory.GetObjectFromAvailableObjects(item);
        _itemDisplay.ShowItem(evidence.Icon, position);
    }

    /// <summary>
    /// Hides the item currently being displayed on the screen.
    /// </summary>
    public void HideItem()
    {
        if (_itemDisplay == null)
        {
            Debug.LogError($"Cannot hide item, no ItemDisplay component assigned to {name}.", gameObject);
        }
        
        _itemDisplay.HideItem();
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
    /// Pans to the position of the specified slot index, if the bg-scene has support for actor slots.
    /// </summary>
    /// <param name="oneBasedSlotIndex">Target slot index, 1 based.</param>
    /// <param name="seconds">Time in seconds the pan should take</param>
    public void PanToActorSlot(int oneBasedSlotIndex, float seconds)
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

        _activeScene.SetActiveActorSlot(oneBasedSlotIndex);
        _onActorChanged.Invoke(_activeScene.ActiveActor);
        PanCamera(seconds, _activeScene.GetTargetPosition());
    }

    /// <summary>
    /// Jump cuts to the target sub position, if the bg-scene has sub positions.
    /// </summary>
    /// <param name="oneBasedSlotIndex">Target actor slot, 1 based.</param>
    public void JumpToActorSlot(int oneBasedSlotIndex)
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

        _activeScene.SetActiveActorSlot(oneBasedSlotIndex);
        _onActorChanged.Invoke(_activeScene.ActiveActor);
        SetCameraPos(_activeScene.GetTargetPosition());
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
        _onWaitStart.Invoke();
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

    /// <summary>
    /// Turns pixel position into a unity position that unity can use based on the configured pixels per unit. Also inverts any value given because the camera has inverse movements because of the implementation.
    /// </summary>
    /// <param name="pixelPosition">Pixel position to turn into unit position</param>
    /// <returns>Unit position from pixel position</returns>
    public Vector2 PixelPositionToUnitPosition(Vector2Int pixelPosition)
    {
        return new Vector2((float)(pixelPosition.x * -1) / _pixelsPerUnit, (float)(pixelPosition.y * -1) / _pixelsPerUnit);
    }
}
