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

    private ScreenshakeCalculator _screenshakeCalculator;
    private float _time;

    [SetUp]
    public void Setup()
    {
        _screenshakeCalculator = CreateScreenshakeCalculator();
        _time = 0;
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
        Vector3[] shakePositions = new Vector3[NUMBER_OF_POSITIONS];
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            shakePositions[i] = _screenshakeCalculator.Calculate(DELTA_TIME);
        }

        var screenShakeCalculatorComparison = CreateScreenshakeCalculator();
        
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            Assert.AreEqual(shakePositions[i], screenShakeCalculatorComparison.Calculate(DELTA_TIME));
        }
    }

    [Test]
    public void ShakeIsConstrainedWithinSpecifiedAmplitude()
    {
        Vector3[] shakePositions = new Vector3[NUMBER_OF_POSITIONS];
        Keyframe[] animationKeys = { new Keyframe(0, 1), new Keyframe(1, 1) };
        var screenshakeCalculator = new ScreenshakeCalculator(SHAKE_DURATION, FREQUENCY, AMPLITUDE, new Vector2(1, 1),
            new Vector2(0, 0), new AnimationCurve(animationKeys));
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            shakePositions[i] = screenshakeCalculator.Calculate(DELTA_TIME);
        }
        
        Assert.IsTrue(shakePositions.Max(vector3 => Mathf.Approximately(vector3.x, AMPLITUDE));
    }

    private ScreenshakeCalculator CreateScreenshakeCalculator()
    {
        return new ScreenshakeCalculator(SHAKE_DURATION, FREQUENCY, AMPLITUDE, new Vector2(1, 1), new Vector2(0, 0));
    }
}