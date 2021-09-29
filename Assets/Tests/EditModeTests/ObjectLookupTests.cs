using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ObjectLookupTests
{
    private ObjectLookup<Evidence> _objectLookup;
    private int ItemCount => _objectLookup.AvailableObjectCount;

    [SetUp]
    public void Setup()
    {
        _objectLookup = new ObjectLookup<Evidence>(CreateScriptableObjectList<Evidence>(50), null);
    }

    [Test]
    public void ObjectsCanBeAddedToObjectLookup()
    {
        Assert.AreEqual(0, _objectLookup.CurrentObjectCount);
        AddObjectsInRange(0, ItemCount);
        Assert.AreEqual(ItemCount, _objectLookup.CurrentObjectCount);
        Assert.Throws<ArgumentException>(() => _objectLookup.AddObject("51"));
        _objectLookup.AddObject("1");
        Assert.AreEqual(ItemCount, _objectLookup.CurrentObjectCount);
    }
    
    [Test]
    public void CanGetObjectFromLookup()
    {
        for (int i = 0; i < ItemCount; i++)
        {
            Assert.AreEqual(i.ToString(), _objectLookup[i.ToString()].name);
        }
    }
    
    [Test]
    public void CanGetObjectFromCurrentObjects()
    {
        AddObjectsInRange(0, ItemCount);
        for (int i = 0; i < ItemCount; i++)
        {
            Assert.AreEqual(i.ToString(), _objectLookup[i].name);
        }
    }

    [Test]
    public void ObjectsCanBeRemovedFromObjectLookup()
    {
        AddObjectsInRange(0, ItemCount);
        for (int i = 0; i < ItemCount; i++)
        {
            _objectLookup.RemoveObject(i.ToString());
        }
        Assert.AreEqual(0, _objectLookup.CurrentObjectCount);
        Assert.Throws<ArgumentException>(() => _objectLookup.RemoveObject("-1"));
        _objectLookup.RemoveObject("0");
        Assert.AreEqual(0, _objectLookup.CurrentObjectCount);
    }

    [Test]
    public void ObjectsCanBeSubstitutedWithOtherObjects()
    {
        AddObjectsInRange(0, ItemCount);
        for (int i = 0; i < ItemCount; i++)
        {
            Evidence evidenceStore = _objectLookup[i];
            _objectLookup.SubstituteObject(i.ToString(), _objectLookup[ItemCount - 1 - i]);
            _objectLookup.SubstituteObject((ItemCount - 1 - i).ToString(), evidenceStore);
        }
        for (int i = 0; i < ItemCount; i++)
        {
            Assert.AreEqual((ItemCount - 1 - i).ToString(), _objectLookup[i].name);
        }
    }

    /// <summary>
    /// Adds objects from the dictionary of available objects to the list of current objects.
    /// Assumes objects have integer names.Adds all object within the range specified.
    /// </summary>
    /// <param name="lowerBoundary">The lower boundary of the range (inclusive).</param>
    /// <param name="upperBoundary">The upper boundary of the range (exclusive).</param>
    private void AddObjectsInRange(int lowerBoundary, int upperBoundary)
    {
        for (int i = lowerBoundary; i < upperBoundary; i++)
        {
            _objectLookup.AddObject(i.ToString());
        }
    }

    /// <summary>
    /// Create a list of scriptable objects to be used in a test.
    /// </summary>
    /// <param name="listCount">The number of items in the list.</param>
    /// <typeparam name="T">The type of scriptable object in the list.</typeparam>
    /// <returns>The created list of scriptable objects.</returns>
    private List<T> CreateScriptableObjectList<T>(int listCount) where T : ScriptableObject
    {
        List<T> objectList = new List<T>();
        for (int i = 0; i < listCount; i++)
        {
            T obj = ScriptableObject.CreateInstance<T>();
            obj.name = i.ToString();
            objectList.Add(obj);
        }

        return objectList;
    }
}
