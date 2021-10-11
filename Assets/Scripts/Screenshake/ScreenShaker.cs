using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenShaker : MonoBehaviour
{
    [Tooltip("The time between frames of the screenshake. Allows for the framerate of the shake to be controlled.")]
    [SerializeField] private float _timeStep = 0.02f;

    [Tooltip()]
    [SerializeField] private float _duration;
    
    [Tooltip()]
    [SerializeField] private float _frequency;
    
    [Tooltip()]
    [SerializeField] private float _amplitude;
    
    [Tooltip("The offset applied to the noise used for shaking. Change this to change the start point for calculating noise.")]
    [SerializeField] private Vector2 _noiseOffset = new Vector2(234, 456);
    
    [Tooltip("How much the noise should be scaled. Higher values mean a shakier camera.")]
    [SerializeField] private Vector2 _noiseScale = new Vector2(30, 30);
    
    [Tooltip("Animation curve is used to alter the animation, e.g. it smoother or less uniform.")]
    [SerializeField] private AnimationCurve _animationCurve;

    [SerializeField] private UnityEvent _onShakeStart;
    [SerializeField] private UnityEvent _onShakeComplete;
    
    private Transform _transform;
    private Vector3 _cameraOffset;
    private Coroutine _shakeCoroutine;

    /// <summary>
    /// Cache transform component on awake
    /// </summary>
    private void Awake()
    {
        _transform = transform;
        _cameraOffset = _transform.position;
    }

    /// <summary>
    /// Shakes the screen, overriding the duration, frequency, and amplitude
    /// set in the inspector and combining frequency and amplitude into one value.
    /// </summary>
    /// <param name="intensity">How much the camera should shake. Combines amplitude and frequency into one value.</param>
    /// <param name="duration">The time (in seconds) that the shake will last.</param>
    /// <param name="shouldWaitForShake">Whether the system waits for the shake to complete before continuing.</param>
    public void Shake(float intensity, float duration, bool shouldWaitForShake)
    {
        StopCurrentlyRunningCoroutine();
        _shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, intensity * 10f, intensity / 10f, shouldWaitForShake));
    }
    
    /// <summary>
    /// Shakes the screen using the values set in the inspector.
    /// </summary>
    public void Shake()
    {
        StopCurrentlyRunningCoroutine();
        _shakeCoroutine = StartCoroutine(ShakeCoroutine(_duration, _frequency, _amplitude, true));
    }

    /// <summary>
    /// Stops the currently running coroutine (if there is one)
    /// </summary>
    private void StopCurrentlyRunningCoroutine()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }
    }

    /// <summary>
    /// Coroutine that causes the camera to shake over a specified amount of time.
    /// Creates a screenshake calculator and every time step gets the new position of the camera.
    /// </summary>
    /// <param name="duration">The number of seconds to shake the camera for.</param>
    /// <param name="frequency">The frequency of the shake's oscillation.</param>
    /// <param name="amplitude">The maximum distance from the objects original position.</param>
    /// <param name="isBlocking">Whether the system waits for the shake to complete before continuing.</param>
    private IEnumerator ShakeCoroutine(float duration, float frequency, float amplitude, bool isBlocking)
    {
        if (isBlocking)
        {
            _onShakeStart.Invoke();
        }

        var screenshakeCalculator = new ScreenshakeCalculator(duration, frequency, amplitude, _noiseScale, _noiseOffset, _animationCurve);
        while (screenshakeCalculator.IsShaking)
        {
            _transform.position = screenshakeCalculator.Calculate(_timeStep) + _cameraOffset;
            yield return new WaitForSeconds(_timeStep);
        }

        _transform.position = _cameraOffset;
        _shakeCoroutine = null;
        if (isBlocking)
        {
            _onShakeComplete.Invoke();
        }
    }
}
