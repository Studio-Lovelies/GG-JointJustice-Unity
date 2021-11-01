using System;
using System.Collections.Generic;
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
    /// Enables and sets the sprite renderer sprite to the correct sprite.
    /// Calls the shaker components Shake method to shake the sprite.
    /// </summary>
    /// <param name="shoutVariant">An array of alternate sprites
    /// that have a chance to randomly appear instead. Set this to null to disable random variations.</param>
    /// <param name="canPlayRandomShout"></param>
    public void PlayShout(SpriteAudioClipPair shoutVariant)
    {
        _spriteRenderer.enabled = true;
        _spriteRenderer.sprite = shoutVariant.Sprite;
        _audioController.PlaySFX(shoutVariant.AudioClip);
        _objectShaker.Shake(_frequency, _amplitude, _duration, true);
    }

    /// <summary>
    /// Checks if a shout variant should be played.
    /// </summary>
    /// <returns>Whether or not a shout variant should be played.</returns>
    public bool ShouldPlayShoutVariant()
    {
        return Random.Range(0f, 1f) < _randomShoutChance;
    }
}
