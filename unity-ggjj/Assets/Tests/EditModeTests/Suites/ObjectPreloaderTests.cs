using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.EditModeTests.Suites
{
    public class ObjectPreloaderTests
    {
        private ObjectStorage _objectStorage;
        private ObjectPreloader _objectPreloader;

        private static IEnumerable<MethodInfo> AvailableActionMethods => typeof(ActionDecoderBase).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(method => new Regex("^[A-Z_]+$").IsMatch(method.Name)).ToArray();
        private static IEnumerable<string> ActorLoadingActions => AvailableActionMethods.Where(method => method.GetParameters().Any(parameter => parameter.Name.Contains("actor"))).Select(methodInfo => methodInfo.Name);
        private static IEnumerable<string> EvidenceLoadingActions => AvailableActionMethods.Where(method => method.GetParameters().Any(parameter => parameter.Name.Contains("evidence"))).Select(methodInfo => methodInfo.Name);
        private static IEnumerable<string> StaticMusicLoadingActions => AvailableActionMethods.Where(method => method.GetParameters().Any(parameter => parameter.Name.Contains("staticSong"))).Select(methodInfo => methodInfo.Name);
        private static IEnumerable<string> DynamicMusicLoadingActions => AvailableActionMethods.Where(method => method.GetParameters().Any(parameter => parameter.Name.Contains("dynamicSong"))).Select(methodInfo => methodInfo.Name);
        private static IEnumerable<string> SfxLoadingActions => AvailableActionMethods.Where(method => method.GetParameters().Any(parameter => parameter.Name.Contains("sfx"))).Select(methodInfo => methodInfo.Name);

        [SetUp]
        public void SetUp()
        {
            _objectStorage = new ObjectStorage();
            _objectPreloader = new ObjectPreloader(_objectStorage);
        }

        [Test]
        [TestCaseSource(nameof(ActorLoadingActions))]
        public void ObjectPreloaderCanLoadActors(string action)
        {
            ObjectPreloaderCanLoadObjects<ActorData>("Actors", action);
        }
        
        [Test]
        [TestCaseSource(nameof(EvidenceLoadingActions))]
        public void ObjectPreloaderCanLoadEvidence(string action)
        {
            ObjectPreloaderCanLoadObjects<EvidenceData>("Evidence", action);
        }
        
        [Test]
        [TestCaseSource(nameof(SfxLoadingActions))]
        public void ObjectPreloaderCanLoadSfx(string action)
        {
            ObjectPreloaderCanLoadObjects<AudioClip>("Audio/SFX", action);
        }
        
        [Test]
        [TestCaseSource(nameof(StaticMusicLoadingActions))]
        public void ObjectPreloaderCanLoadStaticMusic(string action)
        {
            ObjectPreloaderCanLoadObjects<AudioClip>("Audio/Music/Static", action);
        }
        
        [Test]
        [TestCaseSource(nameof(DynamicMusicLoadingActions))]
        public void ObjectPreloaderCanLoadDynamicMusic(string action)
        {
            ObjectPreloaderCanLoadObjects<DynamicMusicData>("Audio/Music/Dynamic", action);
        }

        /// <summary>
        /// Loads all objects at a given path, attempts to use them in an action
        /// then checks to see if they have been added to the object storage.
        /// </summary>
        /// <param name="path">The path to load objects from</param>
        /// <param name="action">The action to perform on the objects</param>
        /// <typeparam name="T">The type of object to load.</typeparam>
        private void ObjectPreloaderCanLoadObjects<T>(string path, string action) where T : Object
        {
            var resourcesInsidePath = Resources.LoadAll(path, typeof(T));

            foreach (var genericResource in resourcesInsidePath)
            {
                var actorPositionParameter = action == "SET_ACTOR_POSITION" ? "1," : "";
                var playSongParameter = action.StartsWith("PLAY_SONG") ? ",2" : "";
                var showItemParameter = action == "SHOW_ITEM" ? ",Left" : "";

                Debug.Log(genericResource);
                var typeSpecificResource = (T)genericResource;
                _objectPreloader.InvokeMatchingMethod($"&{action}:{actorPositionParameter}{typeSpecificResource.name}{playSongParameter}{showItemParameter}");
                var storedActor = _objectStorage.GetObject<T>(typeSpecificResource.name);
                Assert.AreEqual(storedActor, typeSpecificResource);
            }
        }
    }
}