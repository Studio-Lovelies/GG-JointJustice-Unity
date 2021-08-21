using UnityEngine.Events;

public interface IMenuItem
{
    public UnityEvent OnMouseOver { get; }
    void SetHighlighted(bool highlighted);
    void Select(MenuNavigator menuNavigator);
    void SetInteractable(bool interactable);
    void AddOnClickListener(UnityAction listener);
}
