using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameBox : MonoBehaviour
{
    [Tooltip("Drag a TextMeshProUGUI component here")]
    [SerializeField] private TextMeshProUGUI _text;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    /// <summary>
    /// Sets the text and color of the name box to that of the specified actor
    /// </summary>
    /// <param name="actorData">An ActorData object to get the name and color from</param>
    /// <param name="speakingType">The speaking type that is in use</param>
    public void SetSpeaker(ActorData actorData, SpeakingType speakingType)
    {
        gameObject.SetActive(true);
        var nameToDisplay = actorData.DisplayName;

        if (speakingType == SpeakingType.SpeakingWithUnknownName)
        {
            nameToDisplay = "???";
        }

        _text.text = nameToDisplay;
        _image.color = actorData.DisplayColor;
    }

    public void SetSpeakerToNarrator()
    {
        gameObject.SetActive(false);
    }
}
