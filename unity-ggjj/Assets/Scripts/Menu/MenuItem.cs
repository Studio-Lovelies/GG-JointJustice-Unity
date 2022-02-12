using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    [Tooltip("Drag the menu item's text component here.")]
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    
    [field: Tooltip("Invoked when the mouse cursor enters this element or arrow keys are used to navigate to it")]
    [field: SerializeField] public UnityEvent OnItemSelect { get; private set; }
    
    [field: Tooltip("Invoked when the mouse cursor leaves this element or arrow keys are used to navigate away from it")]
    [field: SerializeField] public UnityEvent OnItemDeselect { get; private set; }
    
    private Menu _menu;
    private IHighlight _highlight;
    
    public Button Button { get; private set; }

    /// <summary>
    /// Use this to set the text of a menu item.
    /// </summary>
    public string Text
    {
        get => _textMeshProUGUI.text;
        set
        {
            if (_textMeshProUGUI == null)
            {
                Debug.LogError($"No text component has been assigned to {name}", gameObject);
                return;
            }

            _textMeshProUGUI.text = value;
        }
    }

    /// <summary>
    /// Get required components on awake.
    /// Add listeners to enable selectable and set highlighting on menu set interactable event.
    /// </summary>
    private void Awake()
    {
        Button = GetComponent<Button>();
        _menu = GetComponentInParent<Menu>();
        _menu.OnSetInteractable.AddListener(interactable =>
        {
            Button.enabled = interactable;
            enabled = interactable;
            _highlight.SetHighlighted(_menu.SelectedButton == Button);
        });
        
        _highlight = GetComponentInChildren<IHighlight>();
    }

    /// <summary>
    /// Remove the highlight on disable.
    /// </summary>
    private void OnDisable()
    {
        _highlight?.SetHighlighted(false);
    }

    /// <summary>
    /// Set menu item highlighted on mouse over.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Button.interactable) return;
        
        Button.Select();
        _highlight?.SetHighlighted(true);
    }

    /// <summary>
    /// Set menu item highlighted on select (using menu navigation keyboard buttons).
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        _menu.SelectedButton = Button;
        _highlight?.SetHighlighted(true);
        OnItemSelect.Invoke();
    }

    /// <summary>
    /// Remove highlighted on deselect.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        _highlight?.SetHighlighted(false);
        OnItemDeselect.Invoke();
    }
}
