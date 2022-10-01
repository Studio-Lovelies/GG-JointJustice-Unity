using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.Demo
{
    public class DynamicAudio
    {
        private readonly StoryProgresser _storyProgresser = new StoryProgresser();
        
        [SetUp]
        public void Setup()
        {
            _storyProgresser.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            _storyProgresser.TearDown();
        }

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            TestTools.StartGame("DynamicAudio");
        }

        [UnityTest]
        public IEnumerator CanSwitchBetweenVariantsOfDynamicMusicWithAcceptableDelay()
        {
            // Primary and secondary audio sources can be slightly out-of-sync; this is acceptable to a certain degree
            // This constant defines the accepted tolerance in [PCM samples](https://en.wikipedia.org/wiki/Pulse-code_modulation)
            const int ACCEPTABLE_TOLERANCE_IN_SAMPLES = 15000; // around 0.3 seconds; required for GitHub Actions runners
            
            var finishedFadingDelay = new WaitForSeconds(2.5f);
            
            var primaryMusicSource = GameObject.Find("MusicPrimary").GetComponent<AudioSource>();
            var secondaryAMusicSource = GameObject.Find("MusicSecondaryA").GetComponent<AudioSource>();
            var secondaryBMusicSource = GameObject.Find("MusicSecondaryB").GetComponent<AudioSource>();
            
            // Plays dynamic song variant
            yield return _storyProgresser.ProgressStory();
            Assert.IsTrue(primaryMusicSource.isPlaying);
            Assert.IsFalse(secondaryAMusicSource.isPlaying);
            Assert.IsFalse(secondaryBMusicSource.isPlaying);
            
            yield return _storyProgresser.ProgressStory();
            yield return finishedFadingDelay;
            
            // Plays dynamic song variant
            Assert.IsTrue(primaryMusicSource.isPlaying);
            Assert.IsTrue(secondaryAMusicSource.isPlaying);
            Assert.IsFalse(secondaryBMusicSource.isPlaying);
            Assert.AreEqual(secondaryAMusicSource.timeSamples, primaryMusicSource.timeSamples, ACCEPTABLE_TOLERANCE_IN_SAMPLES);
            
            yield return _storyProgresser.ProgressStory();
            yield return finishedFadingDelay;
            
            // Plays different dynamic song variant
            Assert.IsTrue(primaryMusicSource.isPlaying);
            Assert.IsFalse(secondaryAMusicSource.isPlaying);
            Assert.IsTrue(secondaryBMusicSource.isPlaying);
            Assert.AreEqual(secondaryBMusicSource.timeSamples, primaryMusicSource.timeSamples, ACCEPTABLE_TOLERANCE_IN_SAMPLES);

            yield return _storyProgresser.ProgressStory();
            yield return finishedFadingDelay;
            
            // Plays yet another different dynamic song variant
            Assert.IsTrue(primaryMusicSource.isPlaying);
            Assert.IsTrue(secondaryAMusicSource.isPlaying);
            Assert.IsFalse(secondaryBMusicSource.isPlaying);
            Assert.AreEqual(secondaryAMusicSource.timeSamples, primaryMusicSource.timeSamples, ACCEPTABLE_TOLERANCE_IN_SAMPLES);

            yield return _storyProgresser.ProgressStory();
            yield return finishedFadingDelay;
            
            // Plays different dynamic song
            Assert.IsTrue(primaryMusicSource.isPlaying);
            Assert.IsTrue(secondaryAMusicSource.isPlaying);
            Assert.IsFalse(secondaryBMusicSource.isPlaying);
            Assert.AreEqual(secondaryAMusicSource.timeSamples, primaryMusicSource.timeSamples, ACCEPTABLE_TOLERANCE_IN_SAMPLES);

            yield return _storyProgresser.ProgressStory();
            yield return finishedFadingDelay;
            
            // Plays static song
            Assert.IsTrue(primaryMusicSource.isPlaying);
            Assert.IsFalse(secondaryAMusicSource.isPlaying);
            Assert.IsFalse(secondaryBMusicSource.isPlaying);
        }
    }
}