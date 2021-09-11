using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditModeTests
{
    public class ObjectDictionaryTests
    {
        /// <summary>
        /// Test to check if ObjectDictionary objects are created properly.
        /// They should not be null and have a count equal to the number of objects
        /// to the list passed to the currentObjectList parameter.
        /// </summary>
        [Test]
        public void CreateDictionary()
        {
            var objectDictionary = new ObjectDictionary<GameObject>(CreateGameObjectList(500), CreateGameObjectList(0));
            Assert.NotNull(objectDictionary);
            Assert.AreEqual(0, objectDictionary.Count);
           
            objectDictionary = new ObjectDictionary<GameObject>(CreateGameObjectList(500), CreateGameObjectList(150));
            Assert.NotNull(objectDictionary);
            Assert.AreEqual(150, objectDictionary.Count);
            
            var evidenceDictionary = new ObjectDictionary<Evidence>(CreateScriptableObjectList<Evidence>(500), CreateScriptableObjectList<Evidence>(20));
            Assert.NotNull(evidenceDictionary);
            Assert.AreEqual(20, evidenceDictionary.Count);
            
            var actorDictionary = new ObjectDictionary<ActorData>(CreateScriptableObjectList<ActorData>(500), CreateScriptableObjectList<ActorData>(49));
            Assert.NotNull(evidenceDictionary);
            Assert.AreEqual(49, actorDictionary.Count);
        }

        /// <summary>
        /// Test to check if objects are added correctly.
        /// The dictionary count should be equal to the number of objects added.
        /// </summary>
        [Test]
        public void AddToObjectDictionary()
        {
            int objectDictionaryCount = 5;
            var objectDictionary = new ObjectDictionary<GameObject>(CreateGameObjectList(objectDictionaryCount), CreateGameObjectList(0));
            for (int i = 0; i < objectDictionaryCount; i++)
            {
                objectDictionary.AddObject(i.ToString());
            }
            Assert.AreEqual(objectDictionaryCount, objectDictionary.Count);
        }

        /// <summary>
        /// Test to check if objects are removed correctly.
        /// Count should be equal to the original count minus the number of objects removed.
        /// </summary>
        [Test]
        public void RemoveFromObjectDictionary()
        {
            int objectDictionaryCount = 150;
            int numberOfObjectsToRemove = 30;
            var objectDictionary = new ObjectDictionary<GameObject>(CreateGameObjectList(objectDictionaryCount), CreateGameObjectList(objectDictionaryCount));
            for (int i = 0; i < numberOfObjectsToRemove; i++)
            {
                objectDictionary.RemoveObject(i.ToString());
            }
            Assert.AreEqual(objectDictionaryCount - numberOfObjectsToRemove, objectDictionary.Count);
            Assert.AreEqual("50", objectDictionary["50"].name);
        }

        /// <summary>
        /// Test to check if objects can be retrieved from the dictionary.
        /// In this case the name of the object should be equal to its key in the dictionary.
        /// </summary>
        [Test]
        public void GetObjectFromDictionary()
        {
            int objectDictionaryCount = 100;
            var objectDictionary = new ObjectDictionary<GameObject>(CreateGameObjectList(objectDictionaryCount), CreateGameObjectList(objectDictionaryCount));
            for (int i = 0; i < objectDictionaryCount; i++)
            {
                Assert.AreEqual(i.ToString(), objectDictionary[i.ToString()].name);
            }
        }

        /// <summary>
        /// Test to check if values can be swapped with other values.
        /// Swaps the first 50 objects with the remaining 50 objects in the dictionary
        /// then checks if their names are correct.
        /// </summary>
        [Test]
        public void SubstituteValueInDictionaryWithAlt()
        {
            int objectDictionaryCount = 100;
            var objectDictionary = new ObjectDictionary<GameObject>(CreateGameObjectList(objectDictionaryCount), CreateGameObjectList(objectDictionaryCount));
            for (int i = 0; i < objectDictionaryCount; i++)
            {
                objectDictionary.SubstituteValueWithAlt(i.ToString(), objectDictionary[(objectDictionary.Count - (i + 1)).ToString()]);
            }

            for (int i = 0; i < objectDictionaryCount / 2; i++)
            { 
                Assert.AreEqual((objectDictionaryCount - (i + 1)).ToString(), objectDictionary[i.ToString()].name);
            }

            for (int i = 50; i < objectDictionaryCount; i++)
            {
                Assert.AreEqual(i.ToString(), objectDictionary[i.ToString()].name);
            }
        }

        /// <summary>
        /// Test to check if objects can be retrieved from the dictionary by index.
        /// In this case an object at an index will have the same name as the index number.
        /// </summary>
        [Test]
        public void GetObjectFromDictionaryAtIndex()
        {
            int objectDictionaryCount = 1000;
            var objectDictionary = new ObjectDictionary<GameObject>(CreateGameObjectList(objectDictionaryCount), CreateGameObjectList(objectDictionaryCount));
            for (int i = 0; i < objectDictionaryCount; i++)
            {
                Assert.AreEqual(i.ToString(), objectDictionary[i].name);
            }
        }

        /// <summary>
        /// Creates a list of objects to use in above tests.
        /// </summary>
        /// <param name="count">The number of objects in the list.</param>
        /// <typeparam name="T">The type of object to create a list of.</typeparam>
        /// <returns>The list of objects created.</returns>
        private List<T> CreateObjectList<T>(int count) where T : new()
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
        private List<T> CreateScriptableObjectList<T>(int count) where T : ScriptableObject
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
        private List<GameObject> CreateGameObjectList(int count)
        {
            var gameObjectList = CreateObjectList<GameObject>(count);
            for (int i = 0; i < gameObjectList.Count; i++)
            {
                gameObjectList[i].name = i.ToString();
            }
            return gameObjectList;
        }
    }
}