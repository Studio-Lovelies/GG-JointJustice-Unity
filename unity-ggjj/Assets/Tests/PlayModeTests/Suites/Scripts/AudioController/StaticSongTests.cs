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
    public class StaticSongTests
    {
        private const string MUSIC_PATH = "Audio/Music/";
        
        private global::AudioController _audioController;
        private AudioSource _musicPrimaryAudioSource;
        private VolumeManager _musicPrimaryVolumeManager;

        private AudioSource _musicSecondaryAAudioSource;
        private VolumeManager _musicSecondaryAVolumeManager;
        
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            
            var musicPrimaryGameObject = GameObject.Find("MusicPrimary");
            _musicPrimaryAudioSource = musicPrimaryGameObject.GetComponent<AudioSource>();
            _musicPrimaryVolumeManager = musicPrimaryGameObject.GetComponent<VolumeManager>();
            
            var musicSecondaryAGameObject = GameObject.Find("MusicSecondaryA");
            _musicSecondaryAAudioSource = musicSecondaryAGameObject.GetComponent<AudioSource>();
            _musicSecondaryAVolumeManager = musicSecondaryAGameObject.GetComponent<VolumeManager>();

            _audioController = Object.FindObjectOfType<global::AudioController>();
        }
        
        [UnityTest]
        public IEnumerator FadesBetweenSongs()
        {
            const float TRANSITION_DURATION = 2f;

            // setup and verify steady state of music playing for a while
            var firstSong = LoadSong("aBoyAndHisTrial");
            Assert.IsNotNull(firstSong);
            _audioController.PlayStaticSong(firstSong, TRANSITION_DURATION);

            yield return TestTools.WaitForState(() => _musicPrimaryAudioSource.volume == _musicPrimaryVolumeManager.MaximumVolume, TRANSITION_DURATION*2);
            Assert.AreEqual(firstSong.name, _musicPrimaryAudioSource.clip.name);

            // transition into new song
            var secondSong = LoadSong("aKissFromARose");
            Assert.IsNotNull(secondSong);
            _audioController.PlayStaticSong(secondSong, TRANSITION_DURATION);
            yield return TestTools.WaitForState(() => _musicPrimaryAudioSource.volume != _musicPrimaryVolumeManager.MaximumVolume, TRANSITION_DURATION);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(_musicPrimaryAudioSource.volume, _musicPrimaryVolumeManager.MaximumVolume);
            Assert.AreEqual(firstSong.name, _musicPrimaryAudioSource.clip.name);

            // expect new song to be playing at full volume, as we're done transitioning
            yield return TestTools.WaitForState(() => _musicPrimaryAudioSource.volume == _musicPrimaryVolumeManager.MaximumVolume, TRANSITION_DURATION*2);
            Assert.AreEqual(secondSong.name, _musicPrimaryAudioSource.clip.name);

            // transition into new song
            var thirdSong = LoadSong("investigationJoonyer");
            Assert.IsNotNull(thirdSong);
            _audioController.PlayStaticSong(thirdSong, TRANSITION_DURATION);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            yield return TestTools.WaitForState(() => _musicPrimaryAudioSource.volume != _musicPrimaryVolumeManager.MaximumVolume, TRANSITION_DURATION);
            Assert.AreEqual(secondSong.name, _musicPrimaryAudioSource.clip.name);

            // expect new song to be playing at full volume, as we're done transitioning
            yield return TestTools.WaitForState(() => _musicPrimaryAudioSource.volume == _musicPrimaryVolumeManager.MaximumVolume, TRANSITION_DURATION*2);
            Assert.AreEqual(thirdSong.name, _musicPrimaryAudioSource.clip.name);
        }

        [UnityTest]
        public IEnumerator SongsCanBePlayedWithoutFadeIn()
        {
            var song = LoadSong("aBoyAndHisTrial");
            _audioController.PlayStaticSong(song, 0);
            yield return null;
            Assert.AreEqual(song.name, _musicPrimaryAudioSource.clip.name);
            Assert.AreEqual(_musicPrimaryVolumeManager.MaximumVolume, _musicPrimaryAudioSource.volume);
        }

        [UnityTest]
        public IEnumerator SongsCanBeFadedOut()
        {
            const float TRANSITION_DURATION_IN_SECONDS = 2;
            yield return SongsCanBePlayedWithoutFadeIn();
            var expectedEndTimeOfFadeOut = Time.time + TRANSITION_DURATION_IN_SECONDS;
            _audioController.FadeOutSong(TRANSITION_DURATION_IN_SECONDS);
            yield return TestTools.WaitForState(() => _musicPrimaryAudioSource.volume == 0);

            // This should take at least TRANSITION_DURATION_IN_SECONDS
            Assert.GreaterOrEqual(Time.time, expectedEndTimeOfFadeOut);
            Assert.AreEqual(0, _musicPrimaryAudioSource.volume);
        }

        private static AudioClip LoadSong(string songName)
        {
            return Resources.Load<AudioClip>($"{MUSIC_PATH}Static/{songName}");
        }
    }
}
