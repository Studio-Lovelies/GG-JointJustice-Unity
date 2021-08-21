using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Fade To Image Settings", menuName = "Fade To Image Settings")]
public class FadeToImageSettings : ScriptableObject
{
    [field: SerializeField, Tooltip("The time it takes to fully fade an image in seconds.")]
    public float Time { get; private set; }
    
    [field: SerializeField, Tooltip("The sprite to be faded to")]
    public Sprite Sprite { get; private set; }
    
    [field: SerializeField, Tooltip("The colour of the image")]
    public Color Color { get; private set; }
    
    [field: SerializeField, Tooltip("The animation curve used by the fadeout. Use this to adjust the smoothness of the animation.")]
    public AnimationCurve AnimationCurve { get; private set; }
}
