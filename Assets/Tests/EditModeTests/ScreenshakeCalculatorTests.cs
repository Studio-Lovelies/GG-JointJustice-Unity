using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class ScreenshakeCalculatorTests
{
    private const float DELTA_TIME = 0.02f;
    private const float FREQUENCY = 10;
    private const float SHAKE_DURATION = 50;
    private const int NUMBER_OF_POSITIONS = (int)(SHAKE_DURATION / DELTA_TIME);
    private const float AMPLITUDE = 0.1f;
    private readonly Vector2 NOISE_SCALE = new Vector2(1, 1);

    private ScreenshakeCalculator _screenshakeCalculator;
    private Vector3[] _shakePositions;

    [SetUp]
    public void Setup()
    {
        _screenshakeCalculator = CreateScreenshakeCalculator();
        _shakePositions = new Vector3[NUMBER_OF_POSITIONS];
    }

    [Test]
    public void ScreenshakeRunsForCorrectAmountOfTime()
    {
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            _screenshakeCalculator.Calculate(DELTA_TIME);
            if (i != NUMBER_OF_POSITIONS - 1)
            {
                Assert.IsTrue(_screenshakeCalculator.IsShaking);
            }
        }

        Assert.IsFalse(_screenshakeCalculator.IsShaking);
    }

    [Test]
    public void ScreenShakesWithTheSameValuesAreIdentical()
    {
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            _shakePositions[i] = _screenshakeCalculator.Calculate(DELTA_TIME);
        }

        var screenShakeCalculatorComparison = CreateScreenshakeCalculator();
        
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            Assert.AreEqual(_shakePositions[i], screenShakeCalculatorComparison.Calculate(DELTA_TIME));
        }
    }

    [Test]
    public void ShakeIsConstrainedWithinSpecifiedAmplitude()
    {
        var animationKeys = new [] { new Keyframe(0, 1), new Keyframe(1, 1) };
        var screenshakeCalculator = CreateScreenshakeCalculator(animationCurve: new AnimationCurve(animationKeys));
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            _shakePositions[i] = screenshakeCalculator.Calculate(DELTA_TIME);
        }
        
        Assert.IsTrue(_shakePositions.Any(position => position != Vector3.zero));
        Assert.IsTrue(_shakePositions.All(position => !Mathf.Approximately(position.x,AMPLITUDE) && !Mathf.Approximately(position.x,AMPLITUDE)));
    }

    [Test]
    public void NoShakeWhenAmplitudeIsZero()
    {
        var screenshakeCalculator = CreateScreenshakeCalculator(amplitude: 0);
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            
            _shakePositions[i] = screenshakeCalculator.Calculate(DELTA_TIME);
        }
        
        Assert.IsTrue(_shakePositions.All(position => position == Vector3.zero));
    }

    [Test]
    public void NumberOfOscillationsMatchesFrequency()
    {
        var animationKeys = new [] { new Keyframe(0, 1), new Keyframe(1, 1) };
        var screenshakeCalculator = CreateScreenshakeCalculator(noiseScaleX: 0, noiseScaleY: 0, animationCurve: new AnimationCurve(animationKeys));
        int previousSign = Math.Sign(screenshakeCalculator.Calculate(DELTA_TIME).x);
        int oscillationCount = 0;
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            int currentSign = Math.Sign(screenshakeCalculator.Calculate(DELTA_TIME).x);
            if (currentSign != previousSign)
            {
                oscillationCount++;
                previousSign = currentSign;
            }
        }
        Assert.IsTrue(Mathf.Approximately(oscillationCount / SHAKE_DURATION, FREQUENCY * 2));
    }

    private ScreenshakeCalculator CreateScreenshakeCalculator(float shakeDuration = SHAKE_DURATION, float frequency = FREQUENCY, float amplitude = AMPLITUDE, float noiseScaleX = 1, float noiseScaleY = 1, float noiseOffsetX = 0, float noiseOffsetY = 1, AnimationCurve animationCurve = null)
    {
        return new ScreenshakeCalculator(shakeDuration, frequency, amplitude, new Vector2(noiseScaleX, noiseScaleY), new Vector2(noiseOffsetX, noiseOffsetY), animationCurve);
    }
}