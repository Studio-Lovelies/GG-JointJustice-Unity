using UnityEngine.Events;

public class ActionDecoder
{
    /// <summary>
    /// Forwarded from the DirectorActionDecoder
    /// </summary>
    private UnityEvent _onActionDone;

    public ActionDecoder(UnityEvent onActionDoneFromDirector)
    {
        this._onActionDone = onActionDoneFromDirector;
    }
}
