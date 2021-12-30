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
    public void SetSpeaker(ActorData actorData)
    {
        gameObject.SetActive(actorData.DisplayName != "");

        CurrentActor = actorData;
        _text.text = actorData.DisplayName;
        _image.color = actorData.DisplayColor;
    }

    /// <summary>
    /// The most recently set ActorData, reflects the name currently shown in the NameBox.
    /// </summary>
    public ActorData CurrentActor { get; private set; }
}
