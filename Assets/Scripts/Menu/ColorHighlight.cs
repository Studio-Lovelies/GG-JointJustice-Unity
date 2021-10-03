using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorHighlight : MonoBehaviour, IHighlight
{
    [Tooltip("The color that the menu item should be when it is highlighted.")]
    [SerializeField] private Color _highlightColor;

    private Color _originalColor;
    private Image _image;

    /// <summary>
    /// Store required values on awake.
    /// </summary>
    private void Awake()
    {
        _image = GetComponent<Image>();
        _originalColor = _image.color;
        SetHighlighted(false);
    }

    /// <summary>
    /// Sets the color depending on whether isHighlighted is true or false.
    /// </summary>
    /// <param name="isHighlighted">Whether the object is highlighted (true) or not (false).</param>
    public void SetHighlighted(bool isHighlighted)
    {
        _image.color = isHighlighted ? _highlightColor : _originalColor;
    }
}
