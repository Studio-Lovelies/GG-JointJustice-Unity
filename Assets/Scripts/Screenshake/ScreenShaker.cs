using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float amplitude;
    [SerializeField] private Vector2 _seed;
    [SerializeField] private Vector2 _noiseScale;
    
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
    /// <param name="intensity">The intensity of the screen shake.</param>
    /// <param name="time">The time (in seconds) that the shake will last.</param>
    public void Shake(float time)
    {
        StartCoroutine(ShakeCoroutine(time));
    }

    private IEnumerator ShakeCoroutine(float time)
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime / time;
            Vector3 direction = GetDirection();
            transform.localPosition = (direction * amplitude * _animationCurve.Evaluate(t)) + _cameraOffset;
            yield return null;
        }

        transform.localPosition = _cameraOffset;
    }

    private Vector2 GetDirection()
    {
        return new Vector2(Mathf.PerlinNoise(_seed.x, Time.time * _noiseScale.x) - 0.5f,
            Mathf.PerlinNoise(_seed.y, Time.time * _noiseScale.y) - 0.5f).normalized;
    }
}
