using System;
using TMPro;
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
    [Tooltip("Drag the menu item's text component here.")]
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    
    private Menu _menu;
    private IHighlight _highlight;
    
    public Selectable Selectable { get; private set; }

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
        Selectable = GetComponent<Selectable>();
        _menu = GetComponentInParent<Menu>();
        _menu.OnSetInteractable.AddListener(interactable =>
        {
            Selectable.enabled = interactable;
            enabled = interactable;
            _highlight.SetHighlighted(_menu.SelectedButton == Selectable);
        });

        if (!TryGetComponent<IHighlight>(out _highlight))
        {
            Debug.LogWarning($"{name} was unable to find component with IHighlight interface on {gameObject.name}.");
        }
    }

    /// <summary>
    /// Remove the highlight on disable.
    /// </summary>
    private void OnDisable()
    {
        if (_highlight == null)
            return;

        _highlight.SetHighlighted(false);
    }

    /// <summary>
    /// Set menu item highlighted on mouse over.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Selectable.interactable) return;
        
        Selectable.Select();
        _highlight?.SetHighlighted(true);
    }

    /// <summary>
    /// Set menu item highlighted on select (using menu navigation keyboard buttons).
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        _menu.SelectedButton = Selectable;
        _highlight?.SetHighlighted(true);
    }

    /// <summary>
    /// Remove highlighted on deselect.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        _highlight?.SetHighlighted(false);
    }
}
