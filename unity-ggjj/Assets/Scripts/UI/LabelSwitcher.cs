using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LabelSwitcher : MonoBehaviour
{
    [SerializeField] private string _defaultText;
    [SerializeField] private string _alternateText; 
    [SerializeField] private TextMeshProUGUI _text;
    
    private void Awake()
    {
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
