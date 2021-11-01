using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(ObjectShaker))]
public class ShoutPlayer : MonoBehaviour
{
    [Tooltip("The duration of the shake in seconds.")]
    [SerializeField] private float _duration = 0.5f;
    
    [Tooltip("The frequency of the shake's sine wave in oscillations per second.")]
    [SerializeField] private float _frequency = 10f;
    
    [Tooltip("The maximum distance the object can move from its starting point.")]
    [SerializeField] private float _amplitude = 0.1f;
    
    [Tooltip("The probability that a shout will be replaced by a variation."), Range(0, 1)]
    [SerializeField] private float _randomShoutChance;

    [Tooltip("Drag the sprite used for the the 'objection' shout here.")]
    [SerializeField] private Sprite _objectionSprite;
    
    [Tooltip("Drag the sprite used for the the 'hold it' shout here.")]
    [SerializeField] private Sprite _holdItSprite;
    
    [Tooltip("Drag the sprite used for the the 'take that' shout here.")]
    [SerializeField] private Sprite _takeThatSprite;

    [Tooltip("Drag the audio controller here.")]
    [SerializeField] private AudioController _audioController;
    
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
    /// Call this method to play the "objection" animation.
    /// </summary>
    /// <param name="actorData">An actor data containing an array of shout variants.</param>
    public void PlayObjection(ActorData actorData)
    {
        PlayShout(_objectionSprite, actorData.Objection, actorData.ShoutVariants);
    }

    /// <summary>
    /// Call this method to play the "hold it" animation.
    /// </summary>
    /// <param name="actorData">An actor data containing an array of shout variants.</param>
    public void PlayHoldIt(ActorData actorData)
    {
        PlayShout(_holdItSprite, actorData.HoldIt, actorData.ShoutVariants);
    }

    /// <summary>
    /// Call this method to play the "take that" animation.
    /// </summary>
    /// <param name="actorData">An actor data containing an array of shout variants.</param>
    public void PlayTakeThat(ActorData actorData)
    {
        PlayShout(_takeThatSprite, actorData.TakeThat, actorData.ShoutVariants);
    }

    /// <summary>
    /// Call this method to play a specific shout
    /// </summary>
    /// <param name="actorData">An actor data containing an array of shout variants.</param>
    /// <param name="shoutName">The name of the shout.</param>
    public void PlayShoutout(ActorData actorData, string shoutName)
    {
        try
        {
            SpriteAudioClipPair shoutVariant = actorData.ShoutVariants.Single(shout => shout.Name == shoutName);
            PlayShout(shoutVariant.Sprite, shoutVariant.AudioClip, null);
        }
        catch (InvalidOperationException exception)
        {
            Debug.LogError($"{exception.GetType().Name}: Shout {shoutName} could not be found in the array of shouts for actor {actorData.name}.");
        }
    }

    /// <summary>
    /// Enables and sets the sprite renderer sprite to the correct sprite.
    /// Calls the shaker components Shake method to shake the sprite.
    /// </summary>
    /// <param name="defaultSprite">The sprite that should be displayed.</param>
    /// <param name="defaultAudio">The audio clip that should be played.</param>
    /// <param name="shoutVariants">An array of alternate sprites
    /// that have a chance to randomly appear instead. Set this to null to disable random variations.</param>
    private void PlayShout(Sprite defaultSprite, AudioClip defaultAudio, SpriteAudioClipPair[] shoutVariants)
    {
        _spriteRenderer.enabled = true;

        if (ShouldPlayerShoutVariant(shoutVariants))
        {
            SpriteAudioClipPair shoutVariant = shoutVariants[Random.Range(0, shoutVariants.Length)];
            _spriteRenderer.sprite = shoutVariant.Sprite;
            _audioController.PlaySFX(shoutVariant.AudioClip);
        }
        else
        {
            _spriteRenderer.sprite = defaultSprite;
            _audioController.PlaySFX(defaultAudio);
        }
        
        _objectShaker.Shake(_frequency, _amplitude, _duration, true);
    }

    /// <summary>
    /// Checks if a shout variant should be played.
    /// </summary>
    /// <param name="shoutVariants">The array of shout variants.</param>
    /// <returns>Whether or not a shout variant should be played.</returns>
    private bool ShouldPlayerShoutVariant(SpriteAudioClipPair[] shoutVariants)
    {
        return shoutVariants != null &&
               shoutVariants.Length != 0 &&
               Random.Range(0f, 1f) < _randomShoutChance;
    }
}
