using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.AudioController
{
    // NOTE: As there's no audio hardware present when running headless (i.e. inside automated build processes),
    //       things like ".isPlaying" or ".time"  of AudioSources cannot be asserted, as they will remain
    //       `false` / `0.0f` respectively.
    //       To test this locally, activate `Edit` -> `Project Settings` -> `Audio` -> `Disable Unity Audio`.
    public class AudioControllerTests
    {
        private const string MUSIC_PATH = "Audio/Music/Static/";
        
        private global::AudioController _audioController;
        private AudioSource _audioSource;
        private VolumeManager _volumeManager;
        
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            var audioSourceGameObject = GameObject.Find("MusicPrimary");
            _audioController = Object.FindObjectOfType<global::AudioController>();
            _audioSource = audioSourceGameObject.GetComponent<AudioSource>();
            _volumeManager = audioSourceGameObject.GetComponent<VolumeManager>();
        }
        
        [UnityTest]
        public IEnumerator FadesBetweenSongs()
        {
            const float TRANSITION_DURATION = 2f;

            // setup and verify steady state of music playing for a while
            var firstSong = LoadSong("aBoyAndHisTrial");
            _audioController.PlayStaticSong(firstSong, TRANSITION_DURATION);

            yield return TestTools.WaitForState(() => _audioSource.volume == _volumeManager.MaximumVolume, TRANSITION_DURATION*2);
            Assert.AreEqual(firstSong.name, _audioSource.clip.name);

            // transition into new song
            var secondSong = LoadSong("aKissFromARose");
            _audioController.PlayStaticSong(secondSong, TRANSITION_DURATION);
            yield return TestTools.WaitForState(() => _audioSource.volume != _volumeManager.MaximumVolume, TRANSITION_DURATION);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(_audioSource.volume, _volumeManager.MaximumVolume);
            Assert.AreEqual(firstSong.name, _audioSource.clip.name);

            // expect new song to be playing at full volume, as we're done transitioning
            yield return TestTools.WaitForState(() => _audioSource.volume == _volumeManager.MaximumVolume, TRANSITION_DURATION*2);
            Assert.AreEqual(secondSong.name, _audioSource.clip.name);

            // transition into new song
            var thirdSong = LoadSong("investigationJoonyer");
            _audioController.PlayStaticSong(thirdSong, TRANSITION_DURATION);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            yield return TestTools.WaitForState(() => _audioSource.volume != _volumeManager.MaximumVolume, TRANSITION_DURATION);
            Assert.AreEqual(secondSong.name, _audioSource.clip.name);

            // expect new song to be playing at full volume, as we're done transitioning
            yield return TestTools.WaitForState(() => _audioSource.volume == _volumeManager.MaximumVolume, TRANSITION_DURATION*2);
            Assert.AreEqual(thirdSong.name, _audioSource.clip.name);
        }

        [UnityTest]
        public IEnumerator SongsCanBePlayedWithoutFadeIn()
        {
            var song = LoadSong("aBoyAndHisTrial");
            _audioController.PlayStaticSong(song, 0);
            yield return null;
            Assert.AreEqual(song.name, _audioSource.clip.name);
            Assert.AreEqual(_volumeManager.MaximumVolume, _audioSource.volume);
        }

        [UnityTest]
        public IEnumerator SongsCanBeFadedOut()
        {
            const float TRANSITION_DURATION_IN_SECONDS = 2;
            yield return SongsCanBePlayedWithoutFadeIn();
            var expectedEndTimeOfFadeOut = Time.time + TRANSITION_DURATION_IN_SECONDS;
            _audioController.FadeOutSong(TRANSITION_DURATION_IN_SECONDS);
            yield return TestTools.WaitForState(() => _audioSource.volume == 0);

            // This should take at least TRANSITION_DURATION_IN_SECONDS
            Assert.GreaterOrEqual(Time.time, expectedEndTimeOfFadeOut);
            Assert.AreEqual(0, _audioSource.volume);
        }

        public static AudioClip LoadSong(string songName)
        {
            return Resources.Load<AudioClip>($"{MUSIC_PATH}{songName}");
        }
    }
}
