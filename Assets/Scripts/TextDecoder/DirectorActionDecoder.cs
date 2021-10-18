using UnityEngine;
using UnityEngine.Events;

public class DirectorActionDecoder : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    public ActionDecoder Decoder { get; } = new ActionDecoder();

    private void Awake()
    {
        // We wrap this in an Action so we have no ties to UnityEngine in the ActionDecoder
        Decoder.OnActionDone += () => _onActionDone.Invoke();
    }

    #region API
    /// <summary>
    /// Called whenever a new action is executed (encountered and then forwarded here) in the script
    /// </summary>
    /// <param name="line">The full line in the script containing the action and parameters</param>
    public void OnNewActionLine(string line)
    {
        var actionLine = new ActionLine(line);
        try
        {
            Decoder.OnNewActionLine(actionLine);
        }
        catch (UnknownCommandException e)
        {
            Debug.LogError($"Unknown action: {e.CommandName}");
        }
        catch (InvalidSyntaxException e)
        {
            Debug.LogError($"Invalid syntax: {line}");
            _onActionDone.Invoke();
        }
        catch (UnableToParseException e)
        {
            Debug.LogError($"Invalid parameters for {actionLine.Action}\n{line}\n{e.Message}");
            _onActionDone.Invoke();
        }
        catch (NotEnoughParametersException e)
        {
            Debug.LogError($"Not enough parameters for {actionLine.Action}\n{line}\n{e.Message}");
            _onActionDone.Invoke();
        }
    }

    #endregion
}