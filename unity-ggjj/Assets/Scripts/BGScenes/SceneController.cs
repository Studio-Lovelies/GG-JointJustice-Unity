using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneController : MonoBehaviour, ISceneController
{
    [FormerlySerializedAs("_game")] [SerializeField] private NarrativeGameState _narrativeGameState;
    
    [Tooltip("Pixels per unit of the basic ")]
    [SerializeField] private int _pixelsPerUnit = 100;

    [Tooltip("List of BG scenes in the unity scene, needs to be dragged here for every scene")]
    [SerializeField] private BGSceneList _sceneList;

    [Tooltip("Drag a FadeToImageController here.")]
    [SerializeField] private ImageFader _imageFader;

    [Tooltip("Drag an ItemDisplay component here.")]
    [SerializeField] private ItemDisplay _itemDisplay;

    [Tooltip("Drag the AnimatableObject that plays fullscreen animations here.")]
    [SerializeField] private Animatable _fullscreenAnimationPlayer;

    [Tooltip("Attach the screenshaker object here")]
    [SerializeField] private ObjectShaker _objectShaker;

    [Tooltip("Drag a Shout component here.")]
    [SerializeField] private ShoutPlayer _shoutPlayer;

    [Tooltip("Drag a SceneLoader object here.")]
    [SerializeField] private SceneLoader _sceneLoader;

    [Tooltip("Drag the witness testimony sign here.")]
    [SerializeField] private GameObject _witnessTestimonySign;

    private Coroutine _panToPositionCoroutine;
    private BGScene _activeScene;

    public bool WitnessTestimonyActive
    {
        set => _witnessTestimonySign.SetActive(value);
    }

    /// <summary>
    /// Fades an image from opaque to transparent. Used to fade in to a scene from a black screen.
    /// </summary>
    /// <param name="seconds">The number of seconds it should take to fade.</param>
    public void FadeIn(float seconds)
    {
        if (_imageFader == null)
        {
            Debug.LogError(
                $"Could not begin fade in. {name} does not have a FadeToImageTransition component attached.");
            return;
        }

        _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Waiting = true;
        _imageFader.StartFade(1, 0, seconds, () => _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.SetWaitingToFalseAndContinue());
    }

    /// <summary>
    /// Fades an image from transparent to opaque. Used to fade out from a scene to a black screen.
    /// </summary>
    /// <param name="seconds">The number of seconds it should take to fade.</param>
    public void FadeOut(float seconds)
    {
        if (_imageFader == null)
        {
            Debug.LogError(
                $"Could not begin fade out. {name} does not have a FadeToImageTransition component attached.");
            return;
        }

        _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Waiting = true;
        _imageFader.StartFade(0, 1, seconds, () => _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.SetWaitingToFalseAndContinue());
    }

    /// <summary>
    /// Pans the camera position to the target position in pixels
    /// </summary>
    /// <param name="position">Target position in pixels</param>
    /// <param name="seconds">Time for the pan to take in seconds</param>
    /// <param name="isBlocking">Whether the script should continue after the pan has completed (true) or immediately (false)</param>
    public void PanCamera(float seconds, Vector2Int position, bool isBlocking = false)
    {
        _panToPositionCoroutine =
            StartCoroutine(PanToPosition(PixelPositionToUnitPosition(position), seconds, isBlocking));

        if (!isBlocking)
        {
            _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.SetWaitingToFalseAndContinue();
        }
    }

    /// <summary>
    /// Pans the camera position to the target position
    /// </summary>
    /// <param name="targetPosition">Target position</param>
    /// <param name="timeInSeconds">Time for the pan to take in seconds</param>
    /// <param name="isBlocking">Whether the script should continue after the pan has completed (true) or immediately (false)</param>
    /// <returns>IEnumerator stuff for coroutine</returns>
    private IEnumerator PanToPosition(Vector2 targetPosition, float timeInSeconds, bool isBlocking)
    {
        var startPosition = _activeScene.transform.position;
        var percentagePassed = 0f;
        while (percentagePassed < 1)
        {
            percentagePassed += Time.deltaTime / timeInSeconds;
            _activeScene.transform.position = Vector2.Lerp(startPosition, targetPosition, percentagePassed);
            yield return null;
        }

        _panToPositionCoroutine = null;

        if (isBlocking)
        {
            _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.SetWaitingToFalseAndContinue();
        }
    }

    /// <summary>
    /// Sets the new bg-scene based on the provided name and changes the active actor using an event.
    /// </summary>
    /// <param name="background">Target bg-scene</param>
    public void SetScene(string background)
    {
        _activeScene = _sceneList.SetScene(new SceneAssetName(background));

        if (_panToPositionCoroutine != null)
        {
            StopCoroutine(_panToPositionCoroutine);
        }

        // it's possible to have scenes without actors (where they are part of the background sprites)
        if (_activeScene.CurrentActorSlot != null)
        {
            _narrativeGameState.ActorController.SetActiveActorObject(_activeScene.CurrentActorSlot.AttachedActor);
        }

        _narrativeGameState.ActorController.OnSceneChanged(_activeScene);
    }

    /// <summary>
    /// Sets the camera position to the target position in pixels
    /// </summary>
    /// <param name="position">Target pixel position, top left</param>
    public void SetCameraPos(Vector2Int position)
    {
        _activeScene.transform.position = PixelPositionToUnitPosition(position);
    }

    /// <summary>
    /// Calls shake method on the assigned screen shaker.
    /// </summary>
    /// <param name="intensity">The intensity of the shake.</param>
    /// <param name="duration">The duration of the shake.</param>
    /// <param name="isBlocking">Whether the system waits for the shake to complete before continuing.</param>
    public void ShakeScreen(float intensity, float duration, bool isBlocking)
    {
        _objectShaker.Shake(intensity * 10f, intensity / 10f, duration, isBlocking);

        if (!isBlocking)
        {
            _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.SetWaitingToFalseAndContinue();
        }
    }

    /// <summary>
    /// Gets an item from the evidence dictionary and shows it on the screen at a specified position.
    /// </summary>
    /// <param name="item">The name of the item to show.</param>
    /// <param name="position">The position of the item's image on the screen (left, middle, right).</param>
    public void ShowItem(ICourtRecordObject item, ItemDisplayPosition position)
    {
        if (_itemDisplay == null)
        {
            Debug.LogError($"Cannot show item, no ItemDisplay component assigned to {name}.", gameObject);
        }
        
        _itemDisplay.ShowItem(item.Icon, position);
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

    /// <summary>
    /// Plays a fullscreen animation e.g. Ross' galaxy brain or the gavel hit animations.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    public void PlayAnimation(string animationName)
    {
        if (!HasFullScreenAnimationPlayer())
            return;

        _fullscreenAnimationPlayer.PlayAnimation(animationName);
    }

    /// <summary>
    /// Method to check if an AnimatableObject has been assigned to _fullscreenAnimationPlayer.
    /// </summary>
    private bool HasFullScreenAnimationPlayer()
    {
        if (_fullscreenAnimationPlayer == null)
        {
            Debug.LogError($"Fullscreen Animation Player has not been set on {name}.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Pans to the position of the specified slot index, if the bg-scene has support for actor slots.
    /// </summary>
    /// <param name="slotName">Name of an actor slot in the currently active scene</param>
    /// <param name="seconds">Time in seconds the pan should take</param>
    public void PanToActorSlot(string slotName, float seconds)
    {
        if (_activeScene == null)
        {
            Debug.LogError("Can't assign actor to slot: No active scene");
            return;
        }

        _activeScene.SetActiveActorSlot(slotName);
        _narrativeGameState.ActorController.SetActiveActorObject(_activeScene.CurrentActorSlot.AttachedActor);
        PanCamera(seconds, _activeScene.CurrentActorSlot.Position);
    }

    /// <summary>
    /// Jump cuts to the target sub position, if the bg-scene has sub positions.
    /// </summary>
    /// <param name="slotName">Name of an actor slot in the currently active scene</param>
    public void JumpToActorSlot(string slotName)
    {
        if (_activeScene == null)
        {
            Debug.LogError("Can't assign actor to slot: No active scene");
            return;
        }

        _activeScene.SetActiveActorSlot(slotName);
        _narrativeGameState.ActorController.SetActiveActorObject(_activeScene.CurrentActorSlot.AttachedActor);
        SetCameraPos(_activeScene.CurrentActorSlot.Position);
    }

    /// <summary>
    /// Starts and caches a coroutine to wait for a specific amount of time.
    /// Called by DirectorActionController when a WAIT action is read.
    /// </summary>
    /// <param name="seconds"></param>
    public void Wait(float seconds)
    {
        StartCoroutine(WaitCoroutine(seconds));
    }

    /// <summary>
    /// Coroutine to handle waiting for a specific amount of time.
    /// </summary>
    /// <param name="seconds">The time to wait in seconds.</param>
    private IEnumerator WaitCoroutine(float seconds)
    {
        _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Waiting = true;
        yield return new WaitForSeconds(seconds);
        _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.SetWaitingToFalseAndContinue();
    }

    /// <summary>
    /// Turns pixel position into a unity position that unity can use based on the configured pixels per unit. Also inverts any value given because the camera has inverse movements because of the implementation.
    /// </summary>
    /// <param name="pixelPosition">Pixel position to turn into unit position</param>
    /// <returns>Unit position from pixel position</returns>
    public Vector2 PixelPositionToUnitPosition(Vector2Int pixelPosition)
    {
        return new Vector2((float)(pixelPosition.x * -1) / _pixelsPerUnit,
            (float)(pixelPosition.y * -1) / _pixelsPerUnit);
    }

    /// <summary>
    /// Makes a specified actor shout a specific phrase.
    /// </summary>
    /// <param name="actorName">The name of the actor to shout.</param>
    /// <param name="shoutName">The name of the scout.</param>
    /// <param name="allowRandomShouts">Whether random shouts should be allowed to play (true) or not (false)</param>
    public void Shout(string actorName, string shoutName, bool allowRandomShouts)
    {
        _shoutPlayer.Shout(_narrativeGameState.ObjectStorage.GetObject<ActorData>(actorName).ShoutVariants, shoutName, allowRandomShouts);
    }

    /// <summary>
    /// Forces a scene reload.
    /// Called in narrative scripts when a scene needs to be restarted.
    /// </summary>
    public void ReloadScene()
    {
        _sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
    }
}
