using UnityEngine;

public class ObjectShakeCalculator
{
    private const float WAVELENGTH_TO_RADIANS = 2 * Mathf.PI;
    
    private readonly float _duration;
    private readonly float _frequency;
    private readonly float _amplitude;
    private readonly Vector2 _noiseScale;
    private readonly Vector2 _noiseOffset;
    private readonly AnimationCurve _animationCurve;
    private float _completion = 1;
    private float _time;
    
    public bool IsShaking => !Mathf.Approximately(_completion, 0);

    /// <summary>
    /// Creates ScreenshakeCalculator instance with pre-defined intensity
    /// </summary>
    /// <param name="duration">The duration of the shake in seconds.</param>
    /// <param name="frequency">The frequency that the object will oscillate at.</param>
    /// <param name="amplitude">The maximum distance the camera can move from its starting position in units.</param>
    /// <param name="noiseScale">How much the noise should be scaled on each axis. Bigger numbers mean a faster shake.</param>
    /// <param name="noiseOffset">The position that noise should begin calculating from.</param>
    /// <param name="animationCurve">An animation curve used to add smoothing to the camera shake.</param>
    public ObjectShakeCalculator(float duration, float frequency, float amplitude, Vector2 noiseScale, Vector2 noiseOffset, AnimationCurve animationCurve = null)
    {
        _duration = duration;
        _frequency = frequency;
        _amplitude = amplitude;
        _noiseScale = noiseScale;
        _noiseOffset = noiseOffset;
        _animationCurve = animationCurve ?? new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    }

    /// <summary>
    /// Calculates the current time and uses it to calculate
    /// the current position of the camera.
    /// </summary>
    /// <param name="deltaTime">The time that has passes since the last frame.</param>
    /// <returns>The new position of the camera.</returns>
    public Vector3 Calculate(float deltaTime)
    {
        _completion -= deltaTime / _duration;
        _completion = Mathf.Clamp(_completion, 0, 1);
        var sin = Mathf.Sin(_time * WAVELENGTH_TO_RADIANS);
        _time += deltaTime * _frequency;
        return GetNoise() * sin * _amplitude * _animationCurve.Evaluate(_completion);
    }

    /// <summary>
    /// Uses perlin noise to calculate a position that the camera should be.
    /// Allows for seemingly consistently random but smooth shaking.
    /// Sin and Cos are used to sample points on a circle.
    /// </summary>
    /// <returns>The calculated noise values.</returns>
    private Vector2 GetNoise()
    {
        float x = (Mathf.PerlinNoise(_noiseOffset.x, Mathf.Sin(_completion) * _noiseScale.y) - 0.5f) * 2;
        float y = (Mathf.PerlinNoise(_noiseOffset.y, Mathf.Cos(_completion) * _noiseScale.x) - 0.5f) * 2;
        return new Vector2(x, y).normalized;
    }
}