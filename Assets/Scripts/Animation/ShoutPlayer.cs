using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(ObjectShaker))]
public class ShoutPlayer : MonoBehaviour
{
    [Tooltip("Drag the AudioController here.")]
    [SerializeField] private AudioController _audioController;

    [Tooltip("Drag the SpeechPanel game object here.")]
    [SerializeField] private GameObject _speechPanel;

    [Tooltip("The duration of the shake in seconds.")]
    [SerializeField] private float _duration = 0.5f;
    
    [Tooltip("The frequency of the shake's sine wave in oscillations per second.")]
    [SerializeField] private float _frequency = 10f;
    
    [Tooltip("The maximum distance the object can move from its starting point.")]
    [SerializeField] private float _amplitude = 0.1f;

    [Tooltip("The probability that a shout will be replaced by a variation."), Range(0, 1)]
    [SerializeField] private float _randomShoutChance;

    private SpriteRenderer _spriteRenderer;
    private ObjectShaker _objectShaker;

    /// <summary>
    /// Get the required components on awake.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _objectShaker = GetComponent<ObjectShaker>();
    }

    /// <summary>
    /// Enables and sets the sprite renderer sprite to the correct sprite.
    /// Calls the shaker components Shake method to shake the sprite.
    /// </summary>
    /// <param name="shoutVariants">The array of all possible shouts to play.</param>
    /// <param name="shoutName">The name of the shout to play</param>
    /// <param name="allowRandomShouts">Whether random shouts should be allowed to play (true) or not (false)</param>
    public void Shout(Pair<Sprite, AudioClip>[] shoutVariants, string shoutName, bool allowRandomShouts)
    {
        var shout = new Shout(shoutName, shoutVariants, allowRandomShouts, _randomShoutChance);

        _spriteRenderer.enabled = true;
        _spriteRenderer.sprite = shout.Sprite;
        _objectShaker.Shake(_frequency, _amplitude, _duration, true);
        _audioController.PlaySfx(shout.AudioClip);
        _speechPanel.SetActive(false);
    }
}
