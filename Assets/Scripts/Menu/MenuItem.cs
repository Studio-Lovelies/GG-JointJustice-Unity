using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Attach this class to every item in a menu that can be selectable (has a Selectable component e.g. a Button).
/// Used to set highlighting of each individual menu item
/// and disable interactivity of the associate Selectable.
/// Using EventSystems to handle highlighting when hovering over and selecting of the selectable.
/// Communicates with a IHighlightEnabler to set highlighting.
/// </summary>
public class MenuItem : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    private Selectable _selectable;
    private Menu _menu;
    private IHighlight _highlight;
    
    private void Awake()
    {
        _selectable = GetComponent<Selectable>();
        _menu = GetComponentInParent<Menu>();
        _menu.OnSetInteractable.AddListener(interactable => _selectable.interactable = interactable);

        if (!TryGetComponent<IHighlight>(out _highlight))
        {
            Debug.LogError("Unable to find component with IHighlight interface.");
        }
        else
        {
            _highlight.SetHighlighted(false);
        }
    }

    private void OnEnable()
    {
        _highlight?.SetHighlighted(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_selectable.interactable) return;
        
        _selectable.Select();
        _highlight?.SetHighlighted(true);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _menu.SelectedButton = _selectable;
        _highlight?.SetHighlighted(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _highlight?.SetHighlighted(false);
    }
}
