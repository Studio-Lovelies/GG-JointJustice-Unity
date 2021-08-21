using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class MenuNavigatorTests
{
    private MockMenuItem[] _mockHighlightableMenuItems;
    private MenuNavigator _menuNavigator;

    private void Setup()
    {
        _mockHighlightableMenuItems = new MockMenuItem[]
        {
            new MockMenuItem(),
            new MockMenuItem(),
            new MockMenuItem(),
            new MockMenuItem(),
            new MockMenuItem(),
            new MockMenuItem(),
            new MockMenuItem(),
            new MockMenuItem(),
            new MockMenuItem(),
            new MockMenuItem()
        };
        _menuNavigator = new MenuNavigator(new Vector2Int(3, 0), 2, true, _mockHighlightableMenuItems.ToArray<IMenuItem>());
    }
    
    [Test, Order(0)]
    public void IncrementColumn50()
    {
        Setup();
        IncrementTest(50, new Vector2Int(1, 0));
        Assert.AreEqual(new Vector2Int(3, 0), _menuNavigator.CurrentPosition);
    }
    
    [Test, Order(1)]
    public void DecrementColumn3()
    {
        IncrementTest(3, new Vector2Int(-1, 0));
        Assert.AreEqual(new Vector2Int(0, 0),_menuNavigator.CurrentPosition);
    }
    
    [Test, Order(2)]
    public void DecrementColumn20()
    {
        IncrementTest(20, new Vector2Int(1, 0));
        Assert.AreEqual(new Vector2Int(0, 0), _menuNavigator.CurrentPosition);
    }
    
    [Test, Order(3)]
    public void CheckColumnIncrements()
    {
        for (int i = 0; i < _menuNavigator.ColumnCount; i++)
        {
            Assert.AreEqual(new Vector2Int(i, 0), _menuNavigator.CurrentPosition);
            _menuNavigator.IncrementPositionByVector(Vector2Int.right);
        }
        Assert.AreEqual(new Vector2Int(0, 0), _menuNavigator.CurrentPosition);
    }
    
    [Test, Order(4)]
    public void CheckColumnDecrements()
    {
        _menuNavigator.IncrementPositionByVector(Vector2Int.left);
        for (int i = _menuNavigator.ColumnCount - 1; i >= 0; i--)
        {
            Assert.AreEqual(new Vector2Int(i, 0), _menuNavigator.CurrentPosition);
            _menuNavigator.IncrementPositionByVector(Vector2Int.left);
        }
        Assert.AreEqual(new Vector2Int(4, 0), _menuNavigator.CurrentPosition);
    }
    
    [Test, Order(5)]
    public void IncrementColumn2()
    {
        IncrementTest(2, new Vector2Int(1, 0));
        Assert.AreEqual(new Vector2Int(1, 0),_menuNavigator.CurrentPosition);
    }
    
    [Test, Order(6)]
    public void IncrementRow40()
    {
        IncrementTest(40, new Vector2Int(0, 1));
        Assert.AreEqual(new Vector2Int(1, 0),_menuNavigator.CurrentPosition);
    }
    
    [Test, Order(7)]
    public void DecrementRow33()
    {
        IncrementTest(33, new Vector2Int(0, 1));
        Assert.AreEqual(new Vector2Int(1, 1),_menuNavigator.CurrentPosition);
    }
    
    [Test, Order(8)]
    public void IncrementColumn1IncrementRow1()
    { 
        IncrementTest(1, new Vector2Int(1, 1));
        Assert.AreEqual(new Vector2Int(2, 0),_menuNavigator.CurrentPosition);
    }
    
    [Test, Order(9)]
    public void IncrementColumn3IncrementRow5()
    {
        IncrementTest(1, new Vector2Int(3, 5));
        Assert.AreEqual(new Vector2Int(0, 1),_menuNavigator.CurrentPosition);
    }
    
    private void IncrementTest(int repetitions, Vector2Int increment)
    {
        for (int i = 0; i < repetitions; i++)
        {
            _menuNavigator.IncrementPositionByVector(increment);
        }
    }
}
