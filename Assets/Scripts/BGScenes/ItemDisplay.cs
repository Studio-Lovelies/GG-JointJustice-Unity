using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField, Tooltip("Drag the image that displays on the left here.")]
    private Image _itemLeft;

    [FormerlySerializedAs("_itemCenter")] [SerializeField, Tooltip("Drag the image that displays in the center here.")]
    private Image _itemMiddle;
        
    [SerializeField, Tooltip("Drag the image that displays on the right here.")]
    private Image _itemRight;

    private Image _visibleItem;

    /// <summary>
    /// Hides the current item being displayed, and shows a new item.
    /// </summary>
    /// <param name="itemSprite">The sprite of the item to display.</param>
    /// <param name="displayPosition">The position of the item to display.</param>
    public void ShowItem(Sprite itemSprite, ItemDisplayPosition displayPosition)
    {
        HideItem();
        switch (displayPosition)
        {
            case ItemDisplayPosition.Left:
                EnableAndSetImage(_itemLeft, itemSprite);
                break;
            case ItemDisplayPosition.Middle:
                EnableAndSetImage(_itemMiddle, itemSprite);
                break;
            case ItemDisplayPosition.Right:
                EnableAndSetImage(_itemRight, itemSprite);
                break;
        }
    }

    /// <summary>
    /// Hides the item currently being displayed.
    /// </summary>
    public void HideItem()
    {
        if (_visibleItem == null)
        {
            return;
        }
        
        _visibleItem.enabled = false;
        _visibleItem = null;
    }

    /// <summary>
    /// Used by ShowItem method to enable a certain image
    /// and set its sprite to show the correct item.
    /// </summary>
    /// <param name="image"></param>
    private void EnableAndSetImage(Image image, Sprite sprite)
    {
        image.sprite = sprite;
        image.enabled = true;
        _visibleItem = image;
    }
}
