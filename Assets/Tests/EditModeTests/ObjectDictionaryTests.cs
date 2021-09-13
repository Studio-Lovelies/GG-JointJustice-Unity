using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditModeTests
{
    public class ObjectDictionaryTests
    {
        private EvidenceDictionary _evidenceDictionary;

        private string[] _evidenceNames =
        {
            "Attorneys_Badge",
            "Bent_Coins",
            "Jory_Srs_Letter",
            "Jorys_Backpack",
            "Livestream_Recording",
            "Stolen_Dinos",
            "Switch"
        };

        [SetUp]
        public void Setup()
        {
            _evidenceDictionary = new GameObject("EvidenceDictionary").AddComponent<EvidenceDictionary>();
            _evidenceDictionary.InitialiseDictionaries();
            foreach (var name in _evidenceNames)
            {
                _evidenceDictionary.AddObject(name);
            }
        }

        /// <summary>
        /// Test to check if objects are added correctly.
        /// The dictionary count should be equal to the number of objects added.
        /// </summary>
        [Test]
        public void AddToObjectDictionary()
        {
            Assert.AreEqual(7, _evidenceDictionary.Count);
        }

        /// <summary>
        /// Test to check if objects are removed correctly.
        /// Count should be equal to the original count minus the number of objects removed.
        /// </summary>
        [Test]
        public void RemoveFromObjectDictionary()
        {
            foreach (var name in _evidenceNames)
            {
                _evidenceDictionary.RemoveObject(name);
            }
            Assert.AreEqual(0, _evidenceDictionary.Count);
        }

        /// <summary>
        /// Test to check if objects can be retrieved from the dictionary.
        /// In this case the name of the object should be equal to its key in the dictionary.
        /// </summary>
        [Test]
        public void GetObjectFromDictionary()
        {
            foreach (var name in _evidenceNames)
            {
                Evidence evidence = _evidenceDictionary[name];
                Assert.AreEqual(name, evidence.name);
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
            foreach (var name in _evidenceNames)
            {
                Evidence evidence = ScriptableObject.CreateInstance<Evidence>();
                evidence.name = "newObject";
                _evidenceDictionary.SubstituteValueWithAlt(name, evidence);
                Assert.AreEqual("newObject", _evidenceDictionary[name].name);
            }
        }

        /// <summary>
        /// Test to check if objects can be retrieved from the dictionary by index.
        /// In this case an object at an index will have the same name as the index number.
        /// </summary>
        [Test]
        public void GetObjectFromDictionaryAtIndex()
        {
            for (int i = 0; i < _evidenceNames.Length; i++)
            {
                Assert.AreEqual(_evidenceDictionary[i].name, _evidenceNames[i]);
            }
        }
    }
}