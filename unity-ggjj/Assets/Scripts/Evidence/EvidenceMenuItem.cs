using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(MenuItem))]
public class EvidenceMenuItem : MonoBehaviour, ISelectHandler
{
    [SerializeField, Tooltip("Drag the Image component used to display the evidence's icon here")]
    private Image _image;

    [SerializeField, Tooltip("The evidence menu this evidence menu item is a child of")]
    private EvidenceMenu _evidenceMenu;
    
    private ICourtRecordObject _courtRecordObject;
    private Selectable _selectable;

    private Selectable Selectable => _selectable == null ? _selectable = GetComponent<Selectable>() : _selectable;

    /// <summary>
    /// Disable the selectable then reenable it after one frame so that menu items
    /// aren't selected on the same frame the menu opens
    /// </summary>
    private void OnEnable()
    {
        Selectable.enabled = false;
        StartCoroutine(EnableOnNextFrame());
    }

    private IEnumerator EnableOnNextFrame()
    {
        yield return null;
        Selectable.enabled = true;
    }

    /// <summary>
    /// When evidence is assigned to this menu item its Image component will be automatically updated.
    /// </summary>
    public ICourtRecordObject CourtRecordObject
    {
        get => _courtRecordObject;
        set
        {
            _courtRecordObject = value;
            _image.sprite = value.Icon;
        }
    }

    /// <summary>
    /// When this menu item is selected it should tell the attached
    /// EvidenceMenu to update its evidence name, description, and icon
    /// </summary>
    /// <param name="eventData">Event data required by ISelectHandler</param>
    public void OnSelect(BaseEventData eventData)
    {
        _evidenceMenu.UpdateEvidenceInfo(_courtRecordObject);
    }

    /// <summary>
    /// When this menu item is clicked it should tell the attached EvidenceMenu to call its _onEvidenceClicked event.
    /// To be subscribed to the attached button component's OnClick event.
    /// </summary>
    public void OnMenuItemClicked()
    {
        _evidenceMenu.OnEvidenceClicked(_courtRecordObject);
    }
}
