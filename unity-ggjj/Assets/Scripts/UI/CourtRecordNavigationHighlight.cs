using UnityEngine;
using UnityEngine.UI;

public class CourtRecordNavigationHighlight : MonoBehaviour, IHighlight
{
    private static readonly int IsHighlighted = Animator.StringToHash("IsHighlighted");
    private Animator _animator;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }
    
    public void SetHighlighted(bool isHighlighted)
    {
        var imageColor = _image.color;
        imageColor.a = isHighlighted ? 0.75f : 1f;
        _image.color = imageColor;
        
        _animator.SetBool(IsHighlighted, isHighlighted);
    }
}
