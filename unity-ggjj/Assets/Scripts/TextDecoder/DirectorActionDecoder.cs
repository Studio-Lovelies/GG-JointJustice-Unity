using UnityEngine;

public class DirectorActionDecoder : MonoBehaviour
{
    public const char ACTION_TOKEN = '&';

    private Game _game;
    
    public ActionDecoder Decoder { get; } = new ActionDecoder();

    private void Awake()
    {
        _game = GetComponentInParent<Game>();
        _game.DirectorActionDecoder = this;
        // We wrap this in an Action so we have no ties to UnityEngine in the ActionDecoder
        Decoder.OnActionDone += () => _game.NarrativeScriptPlayer.Continue();
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
            _game.NarrativeScriptPlayer.Continue();
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