using System.Linq;
using NUnit.Framework;
using UnityEngine;

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
        _menuNavigator = new MenuNavigator(new Vector2Int(3, 0), 1, _mockHighlightableMenuItems.ToArray<IHightlightableMenuItem>());
    }
    
    [Test, Order(0)]
    public void Increment50()
    {
        Setup();
        IncrementTest(50);
        Assert.AreEqual(new Vector2Int(4, 0), _menuNavigator.CurrentPosition);
    }
    
    [Test, Order(1)]
    public void Decrement3()
    {
        DecrementTest(3);
        Assert.AreEqual(new Vector2Int(1, 0),_menuNavigator.CurrentPosition);
    }
    
    [Test, Order(2)]
    public void Decrement20()
    {
        DecrementTest(20);
        Assert.AreEqual(new Vector2Int(0, 0), _menuNavigator.CurrentPosition);
    }
    
    [Test, Order(3)]
    public void CheckIncrements()
    {
        for (int i = 0; i < _menuNavigator.ColumnCount; i++)
        {
            Assert.AreEqual(new Vector2Int(i, 0), _menuNavigator.CurrentPosition);
            _menuNavigator.IncrementPositionByVector(Vector2Int.right);
        }
        Assert.AreEqual(new Vector2Int(4, 0), _menuNavigator.CurrentPosition);
    }
    
    [Test, Order(4)]
    public void CheckDecrements()
    {
        for (int i = _menuNavigator.ColumnCount - 1; i >= 0; i--)
        {
            Assert.AreEqual(new Vector2Int(i, 0), _menuNavigator.CurrentPosition);
            _menuNavigator.IncrementPositionByVector(Vector2Int.left);
        }
        Assert.AreEqual(new Vector2Int(0, 0), _menuNavigator.CurrentPosition);
    }
    
    [Test, Order(5)]
    public void Increment2()
    {
        IncrementTest(2);
        Assert.AreEqual(new Vector2Int(2, 0),_menuNavigator.CurrentPosition);
    }
    
    private void IncrementTest(int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            _menuNavigator.IncrementPositionByVector(Vector2Int.right);
        }
    }
    
    private void DecrementTest(int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            _menuNavigator.IncrementPositionByVector(Vector2Int.left);
        }
    }
}
