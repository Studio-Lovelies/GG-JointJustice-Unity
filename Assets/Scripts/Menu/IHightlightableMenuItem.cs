using UnityEngine.Events;

public interface IHightlightableMenuItem
{
    public UnityEvent OnMenuItemMouseOver { get; }
    void SetHighlighted(bool highlighted);
    void Select();
}
