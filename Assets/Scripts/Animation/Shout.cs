using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(ScreenShaker))]
public class Shout : MonoBehaviour
{
    [Tooltip("The probability that a shout will be replaced by a variation."), Range(0, 1)]
    [SerializeField] private float _randomShoutChance;

    [Tooltip("Drag the sprite used for the the 'objection' shout here.")]
    [SerializeField] private Sprite _objectionSprite;
    
    [Tooltip("Drag the sprite used for the the 'hold it' shout here.")]
    [SerializeField] private Sprite _holdItSprite;
    
    [Tooltip("Drag the sprite used for the the 'take that' shout here.")]
    [SerializeField] private Sprite _takeThatSprite;
    
    private SpriteRenderer _spriteRenderer;
    private ScreenShaker _screenShaker;

    /// <summary>
    /// Get the required components on awake.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _screenShaker = GetComponent<ScreenShaker>();
    }
    
    /// <summary>
    /// Call this method to play the "objection" animation.
    /// </summary>
    /// <param name="actorData">An actor data containing an array of shout variants.</param>
    public void Objection(ActorData actorData)
    {
        PlayShout(_objectionSprite, actorData.ShoutVariants);
    }

    /// <summary>
    /// Call this method to play the "hold it" animation.
    /// </summary>
    /// <param name="actorData">An actor data containing an array of shout variants.</param>
    public void HoldIt(ActorData actorData)
    {
        PlayShout(_holdItSprite, actorData.ShoutVariants);
    }

    /// <summary>
    /// Call this method to play the "take that" animation.
    /// </summary>
    /// <param name="actorData">An actor data containing an array of shout variants.</param>
    public void TakeThat(ActorData actorData)
    {
        PlayShout(_takeThatSprite, actorData.ShoutVariants);
    }

    /// <summary>
    /// Enables and sets the sprite renderer sprite to the correct sprite.
    /// Calls the shaker components Shake method to shake the sprite.
    /// </summary>
    /// <param name="defaultSprite">The sprite that should be displayed.</param>
    /// <param name="spriteVariants">An array of alternate sprites
    /// that have a chance to randomly appear instead.</param>
    private void PlayShout(Sprite defaultSprite, Sprite[] spriteVariants)
    {
        _spriteRenderer.enabled = true;
        _spriteRenderer.sprite = spriteVariants.Length > 0 && Random.Range(0f, 1f) > _randomShoutChance
            ? defaultSprite
            : spriteVariants[Random.Range(0, spriteVariants.Length)];
        _screenShaker.Shake();
    }
}
