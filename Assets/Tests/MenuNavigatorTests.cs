using System.Linq;
using NUnit.Framework;

public class MenuNavigatorTests
{
    private MockHighlightableMenuItem[] _mockHighlightableMenuItems;
    private MenuNavigator _menuNavigator;
    
    private void Setup()
    {
        _mockHighlightableMenuItems = new MockHighlightableMenuItem[]
        {
            new MockHighlightableMenuItem(),
            new MockHighlightableMenuItem(),
            new MockHighlightableMenuItem(),
            new MockHighlightableMenuItem(),
            new MockHighlightableMenuItem()
        };
        _menuNavigator = new MenuNavigator(3, _mockHighlightableMenuItems.ToArray<IHightlightableMenuItem>());
    }
    
    [Test, Order(0)]
    public void Increment50()
    {
        Setup();
        IncrementTest(50);
        Assert.AreEqual(4, _menuNavigator.CurrentIndex);
    }

    [Test, Order(1)]
    public void Decrement3()
    {
        DecrementTest(3);
        Assert.AreEqual(1,_menuNavigator.CurrentIndex);
    }
    
    [Test, Order(2)]
    public void Decrement20()
    {
        DecrementTest(20);
        Assert.AreEqual(0, _menuNavigator.CurrentIndex);
    }
    
    [Test, Order(3)]
    public void CheckIncrements()
    {
        for (int i = 0; i < _menuNavigator.IndexCount; i++)
        {
            Assert.AreEqual(i, _menuNavigator.CurrentIndex);
            _menuNavigator.IncrementPosition();
        }
        Assert.AreEqual(4, _menuNavigator.CurrentIndex);
    }
    
    [Test, Order(4)]
    public void CheckDecrements()
    {
        for (int i = _menuNavigator.IndexCount - 1; i >= 0; i--)
        {
            Assert.AreEqual(i, _menuNavigator.CurrentIndex);
            _menuNavigator.DecrementPosition();
        }
        Assert.AreEqual(0, _menuNavigator.CurrentIndex);
    }
    
    [Test, Order(5)]
    public void Increment2()
    {
        IncrementTest(2);
        Assert.AreEqual(2,_menuNavigator.CurrentIndex);
    }

    private void IncrementTest(int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            _menuNavigator.IncrementPosition();
        }
    }

    private void DecrementTest(int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            _menuNavigator.DecrementPosition();
        }
    }
}
