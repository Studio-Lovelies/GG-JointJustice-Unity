using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LabelSwitcher : MonoBehaviour
{
    [SerializeField] private string _defaultText;
    [SerializeField] private string _alternateText;

    private TextMeshProUGUI _text;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        SetDefault();
    }

    public void SetDefault()
    {
        _text.text = _defaultText;
    }

    public void SetAlternate()
    {
        _text.text = _alternateText;
    }
}
