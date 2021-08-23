using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField, Tooltip("The first button that will be selected")]
    private Button _initiallyHighlightedButton;
    
    private Selectable[] _selectables;

    private void Awake()
    {
        _selectables = GetComponentsInChildren<Selectable>();
    }
    
    public void SetMenuInteractable(bool interactable)
    {
        foreach (var button in _selectables)
        {
            button.interactable = interactable;
        }
    }

    public void SelectInitialButton()
    {
        if (_initiallyHighlightedButton == null)
        {
            _selectables[0].Select();
            return;
        }

        _initiallyHighlightedButton.Select();
    }
}
