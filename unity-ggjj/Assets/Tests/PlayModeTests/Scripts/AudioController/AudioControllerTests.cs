using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEditor.SceneManagement;
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
        private const string MUSIC_PATH = "Audio/Music/";

        private AudioSource _audioSource;
        private global::AudioController _audioController;
        
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode("TestScene", new LoadSceneParameters());
            var audioControllerGameObject = new GameObject("AudioController");
            audioControllerGameObject.AddComponent<AudioListener>(); // required to prevent excessive warnings
            _audioController = audioControllerGameObject.AddComponent<global::AudioController>();
            _audioController.enabled = true;
            _audioSource = audioControllerGameObject.transform.Find("Music Player").GetComponent<AudioSource>();
        }
        
        [UnityTest]
        public IEnumerator AudioController_PlaySong_FadesBetweenSongs()
        {
            const float TRANSITION_DURATION = 2f;
            var settingsMusicVolume = TestTools.GetField<float>(_audioController, "_settingsMusicVolume");

            // setup and verify steady state of music playing for a while
            var firstSong = LoadSong("aBoyAndHisTrial");
            _audioController.PlaySong(firstSong, TRANSITION_DURATION);
            yield return new WaitForSeconds(TRANSITION_DURATION);

            Assert.AreEqual(_audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(firstSong.name, _audioSource.clip.name);

            // transition into new song
            var secondSong = LoadSong("aKissFromARose");
            _audioController.PlaySong(secondSong, TRANSITION_DURATION);
            yield return new WaitForSeconds(TRANSITION_DURATION/10f);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(_audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(firstSong.name, _audioSource.clip.name);

            yield return new WaitForSeconds(TRANSITION_DURATION);

            // expect new song to be playing at full volume, as we're done transitioning
            Assert.AreEqual(_audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(secondSong.name, _audioSource.clip.name);

            // transition into new song
            var thirdSong = LoadSong("investigationJoonyer");
            _audioController.PlaySong(thirdSong, TRANSITION_DURATION);
            yield return new WaitForSeconds(TRANSITION_DURATION/10f);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(_audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(secondSong.name, _audioSource.clip.name);

            yield return new WaitForSeconds(TRANSITION_DURATION);

            // expect new song to be playing at full volume, as we're done transitioning
            Assert.AreEqual(_audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(thirdSong.name, _audioSource.clip.name);
        }

        [UnityTest]
        public IEnumerator SongsCanBePlayedWithoutFadeIn()
        {
            var song = LoadSong("aBoyAndHisTrial");
            _audioController.PlaySong(song, 0);
            yield return null;
            Assert.AreEqual(song.name, _audioSource.clip.name);
            Assert.AreEqual(TestTools.GetField<float>(_audioController, "_settingsMusicVolume"), _audioSource.volume);
        }

        [UnityTest]
        public IEnumerator SongsCanBeFadedOut()
        {
            const float TRANSITION_DURATION = 2;
            yield return SongsCanBePlayedWithoutFadeIn();
            _audioController.FadeOutSong(TRANSITION_DURATION);
            yield return new WaitForSeconds(TRANSITION_DURATION);
            Assert.AreEqual(0, _audioSource.volume);
        }

        public static AudioClip LoadSong(string songName)
        {
            return Resources.Load<AudioClip>($"{MUSIC_PATH}{songName}");
        }
    }
}
