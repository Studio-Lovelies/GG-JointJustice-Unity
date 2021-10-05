using UnityEngine;

public class ScreenshakeCalculator
{
    private readonly float _duration;
    private readonly float _amplitude;
    private readonly Vector2 _noiseScale;
    private readonly Vector2 _noiseOffset;
    private readonly AnimationCurve _animationCurve;
    private float _time = 1;

    public bool Shaking => _time > 0;
    public ITimeGetter TimeGetter { private get; set; } = new TimeGetter();

    /// <summary>
    /// Initialised private methods on object construction.
    /// </summary>
    /// <param name="duration">The duration of the shake in seconds.</param>
    /// <param name="amplitude">The maximum distance the camera can move from its starting position in units.</param>
    /// <param name="noiseScale">How much the noise should be scaled on each axis. Bigger numbers mean a faster shake.</param>
    /// <param name="noiseOffset">The position that noise should begin calculating from.</param>
    /// <param name="animationCurve">An animation curve used to add smoothing to the camera shake.</param>
    public ScreenshakeCalculator(float duration, float amplitude, Vector2 noiseScale, Vector2 noiseOffset, AnimationCurve animationCurve = null)
    {
        _duration = duration;
        _amplitude = amplitude;
        _noiseScale = noiseScale;
        _noiseOffset = noiseOffset;
        _animationCurve = animationCurve ?? new AnimationCurve();
    }

    /// <summary>
    /// Calculates the current time and uses it to calculate
    /// the current position of the camera.
    /// </summary>
    /// <returns>The new position of the camera.</returns>
    public Vector3 Calculate()
    {
        _time -= TimeGetter.DeltaTime / _duration;
        return GetNoise() * _amplitude * _animationCurve.Evaluate(_time);
    }
    
    /// <summary>
    /// Uses perlin noise to calculate a position that the camera should be.
    /// Allows for seemingly consistently random but smooth shaking.
    /// </summary>
    /// <returns>The calculated noise values.</returns>
    private Vector2 GetNoise()
    {
        return new Vector2(Mathf.PerlinNoise(_noiseOffset.x, TimeGetter.CurrentTime * _noiseScale.x) - 0.5f,
            Mathf.PerlinNoise(_noiseOffset.y, TimeGetter.CurrentTime * _noiseScale.y) - 0.5f).normalized;
    }
}

/// <summary>
/// Class that allows ScreenShakeCalculator to access time
/// values when it is not being tested.
/// </summary>
public class TimeGetter : ITimeGetter
{
    public float DeltaTime => Time.deltaTime;
    public float CurrentTime => Time.time;
}

/// <summary>
/// Interface that allows ScreenShakeCalculator to access
/// mock time values for when it is being tested.
/// </summary>
public interface ITimeGetter
{
    public float DeltaTime { get; }
    public float CurrentTime { get; }
}