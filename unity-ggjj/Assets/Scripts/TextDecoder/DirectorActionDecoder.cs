using UnityEngine;

public class DirectorActionDecoder : MonoBehaviour
{
    [SerializeField] private Game _game;
    
    private NarrativeScriptPlayer _narrativeScriptPlayer;
    
    public ActionDecoder Decoder { get; } = new ActionDecoder();

    private void Awake()
    {
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
            _narrativeScriptPlayer.Continue();
        }
    }
    #endregion
}