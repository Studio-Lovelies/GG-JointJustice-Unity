using UnityEngine;

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
    /// Get the requires components on awake.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _screenShaker = GetComponent<ScreenShaker>();
    }
    
    /// <summary>
    /// Call 
    /// </summary>
    /// <param name="actorData"></param>
    public void Objection(ActorData actorData)
    {
        PlayShout(_objectionSprite, actorData.ShoutVariations);
    }

    public void HoldIt(ActorData actorData)
    {
        PlayShout(_holdItSprite, actorData.ShoutVariations);
    }

    public void TakeThat(ActorData actorData)
    {
        PlayShout(_takeThatSprite, actorData.ShoutVariations);
    }

    private void PlayShout(Sprite defaultSprite, Sprite[] spriteVariations)
    {
        _spriteRenderer.sprite = ShouldUseSpriteVariation() ?
            defaultSprite :
            spriteVariations[Random.Range(0, spriteVariations.Length)];
        _screenShaker.Shake();
    }

    private bool ShouldUseSpriteVariation()
    {
        return Random.Range(0f, 1f) > _randomShoutChance;
    }

    private Sprite GetSpriteVariant(Sprite[] spriteVariations)
    {
        
    }
}
