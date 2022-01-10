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

    private ScreenshakeCalculator _screenshakeCalculator;
    private Vector3[] _shakePositions;

    /// <summary>
    /// Create a new screenshake calculator and array to store positions before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _screenshakeCalculator = CreateScreenshakeCalculator();
        _shakePositions = new Vector3[NUMBER_OF_POSITIONS];
    }

    /// <summary>
    /// Tests if a screenshake runs for the correct amount of time by looping the
    /// number of times it would expect to run for then checking if ScreenshakeCalculator's
    /// IsShaking property is false.
    /// </summary>
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

    /// <summary>
    /// If the same input is inputted, the ScreenshakeCalculator should output the same positions every time.
    /// Runs the ScreenshakeCalculator twice and checks if the outputted positions are equal.
    /// </summary>
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

    /// <summary>
    /// The amplitude of a screen shake should not be greater than the amplitude specified.
    /// Gets all the positions and makes sure they are all within a range of -AMPLITUDE and AMPLITUDE.
    /// Also makes sure all values are not the same.
    /// </summary>
    [Test]
    public void ShakeIsConstrainedWithinSpecifiedAmplitude()
    {
        var animationKeys = new [] { new Keyframe(0, 1), new Keyframe(1, 1) };
        var screenshakeCalculator = CreateScreenshakeCalculator(animationCurve: new AnimationCurve(animationKeys));
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
        {
            _shakePositions[i] = screenshakeCalculator.Calculate(DELTA_TIME);
        }
        
        Assert.IsTrue(_shakePositions.All(position1 => _shakePositions.Any(position2 => position1 == position2)));
        Assert.IsTrue(_shakePositions.All(position => position.x <= AMPLITUDE && position.x >= -AMPLITUDE && position.y <= AMPLITUDE && position.y > -AMPLITUDE));
    }

    /// <summary>
    /// When amplitude is zero there should be no shake and all positions returned will be zero.
    /// </summary>
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

    /// <summary>
    /// Checks if the number of half oscillations is correct for the specified frequency
    /// by counting how many times the sign the x value of the returned position changes
    /// then dividing that by the shake duration to get the frequency multiplied by two
    /// </summary>
    [Test]
    public void NumberOfOscillationsMatchesFrequency()
    {
        var animationKeys = new [] { new Keyframe(0, 1), new Keyframe(1, 1) };
        var screenshakeCalculator = CreateScreenshakeCalculator(noiseScaleX: 0, noiseScaleY: 0, animationCurve: new AnimationCurve(animationKeys));
        int previousSign = Math.Sign(screenshakeCalculator.Calculate(DELTA_TIME).x);
        int halfOscillationCount = 0;
        for (int i = 1; i < NUMBER_OF_POSITIONS; i++)
        {
            int currentSign = Math.Sign(screenshakeCalculator.Calculate(DELTA_TIME).x);
            if (currentSign != previousSign && currentSign != 0)
            {
                halfOscillationCount++;
                previousSign = currentSign;
            }
        }
        Assert.IsTrue(Mathf.Approximately(halfOscillationCount / SHAKE_DURATION, FREQUENCY * 2));
    }

    /// <summary>
    /// Method used to construct a ScreenshakeCalculator object.
    /// Override specific values to customise the ScreenshakeCalculator.
    /// </summary>
    /// <param name="shakeDuration">The duration of the shake.</param>
    /// <param name="frequency">The frequency of the shake's oscillation.</param>
    /// <param name="amplitude">The amplitude of the shake (how "big" it is)</param>
    /// <param name="noiseScaleX">How much the noise should be scaled on the X axis.</param>
    /// <param name="noiseScaleY">How much the noise should be scaled on the Y axis.</param>
    /// <param name="noiseOffsetX">How much the noise should be offset on the X axis.</param>
    /// <param name="noiseOffsetY">How much the noise should be offset on the Y axis.</param>
    /// <param name="animationCurve">The animation curve used to calculate the falloff of the shake.</param>
    /// <returns>The ScreenshakeCalculator object created.</returns>
    private static ScreenshakeCalculator CreateScreenshakeCalculator(float shakeDuration = SHAKE_DURATION, float frequency = FREQUENCY, float amplitude = AMPLITUDE, float noiseScaleX = 1, float noiseScaleY = 1, float noiseOffsetX = 0, float noiseOffsetY = 1, AnimationCurve animationCurve = null)
    {
        return new ScreenshakeCalculator(shakeDuration, frequency, amplitude, new Vector2(noiseScaleX, noiseScaleY), new Vector2(noiseOffsetX, noiseOffsetY), animationCurve);
    }
}