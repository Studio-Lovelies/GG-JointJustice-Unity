using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _amplitude = 0.1f;
    [SerializeField] private Vector2 _seed = new Vector2(234, 456);
    [SerializeField] private Vector2 _noiseScale = new Vector2(30, 30);
    
    private Transform _transform;
    private Vector3 _cameraOffset;
    
    /// <summary>
    /// Cache transform component on awake
    /// </summary>
    private void Awake()
    {
        _transform = transform;
        _cameraOffset = _transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Shake(0.5f);
        }
    }
    
    /// <summary>
    /// Call this method to begin shaking screen.
    /// </summary>
    /// <param name="time">The time (in seconds) that the shake will last.</param>
    public void Shake(float time)
    {
        StartCoroutine(ShakeCoroutine(time));
    }

    /// <summary>
    /// Coroutine that causes the camera to shake over a specified amount of time.
    /// </summary>
    /// <param name="time">The number of seconds to shake the camera for.</param>
    private IEnumerator ShakeCoroutine(float time)
    {
        var screenshakeCalculator = new ScreenshakeCalculator(time, _amplitude, _noiseScale, _seed, _animationCurve);
        while (screenshakeCalculator.Shaking)
        {
            _transform.localPosition = screenshakeCalculator.Calculate() + _cameraOffset;
            yield return null;
        }

        _transform.localPosition = _cameraOffset;
    }
}
