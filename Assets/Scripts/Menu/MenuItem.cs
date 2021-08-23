using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Attach this class to every item in a menu.
/// Used to set highlighting of each individual menu item
/// and disable interactivity of the associate selectable.
/// Using EventSystems to handle highlighting when hovering over and selecting of the selectable.
/// Communicates with a IHighlightEnabler to set highlighting.
/// </summary>
public class MenuItem : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    private Selectable _selectable;
    private MenuController _menuController;
    private IHighlightEnabler _highlightEnabler;
    
    private void Awake()
    {
        _selectable = GetComponent<Selectable>();
        _menuController = GetComponentInParent<MenuController>();
        _menuController.OnSetInteractable.AddListener(interactable => _selectable.interactable = interactable);

        if (!TryGetComponent(out _highlightEnabler))
        {
            Debug.LogError("Unable to find component with HighlightableEnabler interface.");
        }
        else
        {
            _highlightEnabler.SetHighlighted(false);
        }
    }

    private void OnEnable()
    {
        _highlightEnabler?.SetHighlighted(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_selectable.interactable) return;
        
        _selectable.Select();
        _highlightEnabler?.SetHighlighted(true);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _menuController.SelectedButton = _selectable;
        _highlightEnabler?.SetHighlighted(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _highlightEnabler?.SetHighlighted(false);
    }
}
