using UnityEngine.Events;

public interface IHightlightableMenuItem
{
    public UnityEvent OnMouseOver { get; }
    void SetHighlighted(bool highlighted);
    void Select(MenuNavigator menuNavigator);
}
