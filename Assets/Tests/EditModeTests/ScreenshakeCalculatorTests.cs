using NUnit.Framework;
using UnityEngine;

public class ScreenshakeCalculatorTests
{
    private ScreenshakeCalculator _screenshakeCalculator;
    private MockTimeGetter _timeGetter = new MockTimeGetter();
    
    [SetUp]
    public void Setup()
    {
        _screenshakeCalculator = new ScreenshakeCalculator(1, 1, new Vector2(1, 1), new Vector2(0, 0))
        { 
            TimeGetter = _timeGetter
        };
    }

    [Test]
    public void ScreenshakeRunsForCorrectAmountOfTime()
    {
        for (int i = 0; i < 1 / _timeGetter.DeltaTime; i++)
        {
            Assert.IsTrue(_screenshakeCalculator.Shaking);
            _screenshakeCalculator.Calculate();
        }
        
        Assert.IsFalse(_screenshakeCalculator.Shaking);
    }

    private void IncrementTime()
    {
        _timeGetter.CurrentTime += _timeGetter.DeltaTime;
    }
}

public class MockTimeGetter : ITimeGetter
{
    public float DeltaTime => 0.02f;
    public float CurrentTime { get; set; } = 0;
}