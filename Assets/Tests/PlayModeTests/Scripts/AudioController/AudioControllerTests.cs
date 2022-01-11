using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
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

        [UnityTest]
        public IEnumerator AudioController_PlaySong_FadesBetweenSongs()
        {
            GameObject audioControllerGameObject = new GameObject("AudioController");
            audioControllerGameObject.AddComponent<AudioListener>(); // required to prevent excessive warnings
            global::AudioController audioController = audioControllerGameObject.AddComponent<global::AudioController>();

            // expect error due to missing DirectorActionDecoder
            LogAssert.Expect(LogType.Error, "Audio Controller doesn't have an action decoder to attach to");
            yield return null;
            AudioSource audioSource = audioControllerGameObject.transform.Find("Music Player").GetComponent<AudioSource>();

            FieldInfo type = audioController.GetType().GetField("_transitionDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            if (type is null) // needed to satisfy Intellisense's "possible NullReferenceException" in line below conditional
            {
                Assert.IsNotNull(type);
            }
            float transitionDuration = (float)type.GetValue(audioController);

            FieldInfo settingsMusicVolumeType = audioController.GetType().GetField("_settingsMusicVolume", BindingFlags.NonPublic | BindingFlags.Instance);
            if (settingsMusicVolumeType is null) // needed to satisfy Intellisense's "possible NullReferenceException" in line below conditional
            {
                Assert.IsNotNull(settingsMusicVolumeType);
            }
            float settingsMusicVolume = (float)settingsMusicVolumeType.GetValue(audioController);

            // setup and verify steady state of music playing for a while
            var firstSong = Resources.Load<AudioClip>($"{MUSIC_PATH}aBoyAndHisTrial");
            audioController.PlaySong(firstSong);
            yield return new WaitForSeconds(transitionDuration);

            Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(firstSong.name, audioSource.clip.name);

            // transition into new song
            var secondSong = Resources.Load<AudioClip>($"{MUSIC_PATH}aKissFromARose");
            audioController.PlaySong(secondSong);
            yield return new WaitForSeconds(transitionDuration/10f);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(firstSong.name, audioSource.clip.name);

            yield return new WaitForSeconds(transitionDuration);

            // expect new song to be playing at full volume, as we're done transitioning
            Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(secondSong.name, audioSource.clip.name);

            // transition into new song
            var thirdSong = Resources.Load<AudioClip>($"{MUSIC_PATH}investigationJoonyer");
            audioController.PlaySong(thirdSong);
            yield return new WaitForSeconds(transitionDuration/10f);

            // expect old song to still be playing, but no longer at full volume, as we're transitioning
            Assert.AreNotEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(secondSong.name, audioSource.clip.name);

            yield return new WaitForSeconds(transitionDuration);

            // expect new song to be playing at full volume, as we're done transitioning
            Assert.AreEqual(audioSource.volume, settingsMusicVolume);
            Assert.AreEqual(thirdSong.name, audioSource.clip.name);
        }
    }
}
