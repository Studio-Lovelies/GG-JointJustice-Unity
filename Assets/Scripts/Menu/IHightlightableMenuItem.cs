using UnityEngine.Events;

public interface IHightlightableMenuItem
{
    UnityEvent OnMenuItemMouseOver { get; }
    void Select();
    void SetHighlighted(bool highlighted);
}
