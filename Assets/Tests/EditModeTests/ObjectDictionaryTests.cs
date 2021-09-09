using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditModeTests
{
    public class ObjectDictionaryTests
    {
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

        [Test]
        public void GetObjectFromDictionaryAtIndex()
        {
            int objectDictionaryCount = 1000;
            var objectDictionary = new ObjectDictionary<GameObject>(CreateGameObjectList(objectDictionaryCount), CreateGameObjectList(objectDictionaryCount));
            for (int i = 0; i < objectDictionaryCount; i++)
            {
                Assert.AreEqual(i.ToString(), objectDictionary.GetObjectAtIndex(i).name);
            }
        }

        private List<T> CreateObjectList<T>(int count) where T : new()
        {
            var list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new T());
            }
            return list;
        }
        
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