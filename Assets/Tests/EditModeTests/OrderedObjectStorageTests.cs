using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditModeTests
{
    public class OrderedObjectStorageTests
    {
        /// <summary>
        /// Test to check if OrderedObjectStorage objects are created properly.
        /// They should not be null and have a count equal to the number of objects
        /// in the list passed to their objectList parameter.
        /// </summary>
        [Test]
        public void CreateOrderedObjectStorage()
        {
            var orderedObjectStorage = new OrderedObjectStorage<GameObject>(CreateGameObjectList(500));
            Assert.NotNull(orderedObjectStorage);
            Assert.AreEqual(500, orderedObjectStorage.Count);
            
            orderedObjectStorage = new OrderedObjectStorage<GameObject>(CreateGameObjectList(0));
            Assert.NotNull(orderedObjectStorage);
            Assert.AreEqual(0, orderedObjectStorage.Count);

            var evidenceDictionary = new OrderedObjectStorage<Evidence>(CreateScriptableObjectList<Evidence>(49));
            Assert.NotNull(evidenceDictionary);
            Assert.AreEqual(49, evidenceDictionary.Count);

            var actorDictionary = new OrderedObjectStorage<ActorData>(CreateScriptableObjectList<ActorData>(30));
            Assert.NotNull(actorDictionary);
            Assert.AreEqual(30, actorDictionary.Count);
        }

        /// <summary>
        /// Test to check if objects are added correctly.
        /// The OrderedObjectStorage count should be equal to the number of objects added.
        /// </summary>
        [Test]
        public void AddToOrderedObjectStorage()
        {
            const int numberOfObjectsToAdd = 5;
            var orderedObjectStorage = new OrderedObjectStorage<GameObject>(CreateGameObjectList(0));
            for (int i = 0; i < numberOfObjectsToAdd; i++)
            {
                orderedObjectStorage.Add(i.ToString(), new GameObject());
            }
            Assert.AreEqual(numberOfObjectsToAdd, orderedObjectStorage.Count);
        }
        
        /// <summary>
        /// Test to check if objects are removed correctly.
        /// Count should be equal to the original count minus the number of objects removed.
        /// </summary>
        [Test]
        public void RemoveFromObjectOrderedObjectStorage()
        {
            const int numberOfObjectsToAdd = 150;
            const int numberOfObjectsToRemove = 30;
            var orderedObjectStorage = new OrderedObjectStorage<GameObject>(CreateGameObjectList(numberOfObjectsToAdd));
            for (int i = 0; i < numberOfObjectsToRemove; i++)
            {
                orderedObjectStorage.Remove(i.ToString());
            }
            Assert.AreEqual(numberOfObjectsToAdd - numberOfObjectsToRemove, orderedObjectStorage.Count);
        }
        
        /// <summary>
        /// Test to check if objects can be retrieved from the OrderedObjectStorage instance.
        /// In this case the name of the object should be equal to its key in OrderedObjectStorage's dictionary.
        /// </summary>
        [Test]
        public void GetObjectFromOrderedObjectStorage()
        {
            const int numberOfObjectsToAdd = 100;
            var orderedObjectStorage = new OrderedObjectStorage<GameObject>(CreateGameObjectList(numberOfObjectsToAdd));
            for (int i = 0; i < numberOfObjectsToAdd; i++)
            {
                Assert.AreEqual(i.ToString(), orderedObjectStorage[i.ToString()].name);
            }
        }
        
        /// <summary>
        /// Test to check if objects can be swapped with other objects.
        /// Swaps the first 50 objects with the remaining 50 objects in the dictionary
        /// then checks if their names are correct.
        /// </summary>
        [Test]
        public void SubstituteObjectWithAlt()
        {
            const int numberOfObjectsToAdd = 100;
            var orderedObjectStorage = new OrderedObjectStorage<GameObject>(CreateGameObjectList(numberOfObjectsToAdd));
            for (int i = 0; i < numberOfObjectsToAdd; i++)
            {
                orderedObjectStorage.SubstituteObject(i.ToString(), orderedObjectStorage[(orderedObjectStorage.Count - (i + 1)).ToString()]);
            }
        
            for (int i = 0; i < numberOfObjectsToAdd / 2; i++)
            { 
                Assert.AreEqual((numberOfObjectsToAdd - (i + 1)).ToString(), orderedObjectStorage[i.ToString()].name);
            }
        
            for (int i = 50; i < numberOfObjectsToAdd; i++)
            {
                Assert.AreEqual(i.ToString(), orderedObjectStorage[i.ToString()].name);
            }
        }
        
        /// <summary>
        /// Test to check if objects can be retrieved from OrderedObjectStorage by index.
        /// In this case an object at an index will have the same name as the index number.
        /// Also tests that objects are added to storage in order.
        /// </summary>
        [Test]
        public void GetObjectAtIndex()
        {
            const int numberOfObjectsToAdd = 1000;
            var orderedObjectStorage = new OrderedObjectStorage<GameObject>(CreateGameObjectList(numberOfObjectsToAdd));
            for (int i = 0; i < numberOfObjectsToAdd; i++)
            {
                Assert.AreEqual(i.ToString(), orderedObjectStorage[i].name);
            }
        }

        /// <summary>
        /// Creates a list of objects to use in above tests.
        /// </summary>
        /// <param name="count">The number of objects in the list.</param>
        /// <typeparam name="T">The type of object to create a list of.</typeparam>
        /// <returns>The list of objects created.</returns>
        private static IEnumerable<T> CreateObjectList<T>(int count) where T : new()
        {
            var list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new T());
            }
            return list;
        }
        
        /// <summary>
        /// Creates a list of scriptable objects to use in above tests.
        /// </summary>
        /// <param name="count">The number of scriptable objects in the list.</param>
        /// <typeparam name="T">The type of scriptable object to create a list of.</typeparam>
        /// <returns>The list of scriptable objects created.</returns>
        private static IEnumerable<T> CreateScriptableObjectList<T>(int count) where T : ScriptableObject
        {
            var list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                T scriptableObject = ScriptableObject.CreateInstance<T>();
                scriptableObject.name = i.ToString();
                list.Add(scriptableObject);
            }
            return list;
        }
        
        /// <summary>
        /// Create a list of game objects and give them corresponding to the order they were created in.
        /// </summary>
        /// <param name="count">The number of game objects to create.</param>
        /// <returns>The list of game objects created.</returns>
        private IEnumerable<GameObject> CreateGameObjectList(int count)
        {
            var gameObjectList = CreateObjectList<GameObject>(count);
            var gameObjects = gameObjectList as GameObject[] ?? gameObjectList.ToArray();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].name = i.ToString();
            }
            return gameObjects;
        }
    }
}