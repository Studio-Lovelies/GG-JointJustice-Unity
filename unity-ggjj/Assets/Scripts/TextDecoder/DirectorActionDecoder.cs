using UnityEngine;

public class DirectorActionDecoder : MonoBehaviour
{
    public const char ACTION_TOKEN = '&';

    private NarrativeScriptPlayer _narrativeScriptPlayer;
    
    public ActionDecoder Decoder { get; } = new ActionDecoder();

    private void Awake()
    {
        var game = GetComponentInParent<Game>();
        game.DirectorActionDecoder = this;
        _narrativeScriptPlayer = GetComponentInParent<NarrativeScriptPlayer>();
        // We wrap this in an Action so we have no ties to UnityEngine in the ActionDecoder
        Decoder.OnActionDone += () => _narrativeScriptPlayer.Continue();
    }

    #region API
    /// <summary>
    /// Called whenever a new action is executed (encountered and then forwarded here) in the script
    /// </summary>
    /// <param name="line">The full line in the script containing the action and parameters</param>
    public void OnNewActionLine(string line)
    {
        try
        {
            Decoder.OnNewActionLine(line);
        }
        catch (TextDecoder.Parser.ScriptParsingException exception)
        {
            Debug.LogError(exception);
            _narrativeScriptPlayer.Continue();
        }
    }

    /// <summary>
    /// Determines if a line of dialogue is an action.
    /// </summary>
    /// <param name="line">The line to check.</param>
    /// <returns>If the line is an action (true) or not (false)</returns>
    public bool IsAction(string line)
    {
        return line[0] == ACTION_TOKEN;
    }
    #endregion
}