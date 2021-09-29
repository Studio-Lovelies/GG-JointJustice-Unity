using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ObjectLookupTests
{
    private ObjectLookup<Evidence> _objectLookup;
    private int ItemCount => _objectLookup.AvailableObjectCount;

    /// <summary>
    /// Create a new ObjectLookup object on SetUp
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _objectLookup = new ObjectLookup<Evidence>(CreateScriptableObjectList<Evidence>(50), null);
    }

    /// <summary>
    /// Test if objects can be added from the dictionary of available objects to the list of current objects
    /// by adding to it and checking if CurrentObjectCount equals the expected number.
    /// Tries to objects that are not in the dictionary of available
    /// objects and objects that are already in the list.
    /// </summary>
    [Test]
    public void ObjectsCanBeAddedToCurrentObjectsList()
    {
        Assert.AreEqual(0, _objectLookup.CurrentObjectCount);
        AddObjectsInRange(0, ItemCount);
        Assert.AreEqual(ItemCount, _objectLookup.CurrentObjectCount);
        Assert.Throws<ArgumentException>(() => _objectLookup.AddObject("51"));
        _objectLookup.AddObject("1");
        Assert.AreEqual(ItemCount, _objectLookup.CurrentObjectCount);
    }
    
    /// <summary>
    /// Test if objects can be retrieved from the dictionary of available objects by name.
    /// </summary>
    [Test]
    public void CanGetObjectFromAvailableObjects()
    {
        for (int i = 0; i < ItemCount; i++)
        {
            Assert.AreEqual(i.ToString(), _objectLookup[i.ToString()].name);
        }
    }
    
    /// <summary>
    /// Test if objects can be retrieved from the list of current objects by index.
    /// </summary>
    [Test]
    public void CanGetObjectFromCurrentObjects()
    {
        AddObjectsInRange(0, ItemCount);
        for (int i = 0; i < ItemCount; i++)
        {
            Assert.AreEqual(i.ToString(), _objectLookup[i].name);
        }
    }

    /// <summary>
    /// Test if objects can be removed from the list of current objects.
    /// First adds objects then removes them all and checks if there are 0 objects in the list.
    /// Tries to remove objects not in the dictionary and objects in the dictionary but not the list.
    /// </summary>
    [Test]
    public void ObjectsCanBeRemovedFromCurrentObjectsList()
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

    /// <summary>
    /// Test if objects can be substituted with other objects using the SubstituteObject method.
    /// Reverses the order of the list of current objects, then loops through checking if all
    /// the names are in the expected order.
    /// </summary>
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
