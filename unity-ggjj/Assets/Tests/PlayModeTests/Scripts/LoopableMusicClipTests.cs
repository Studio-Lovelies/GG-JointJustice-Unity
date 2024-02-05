using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts
{
    /// <summary>
    /// Tests that the LoopableMusicClip class handles basic playback while respecting looping rules
    /// </summary>
    /// <see cref="LoopableMusicClip"/>
    /// <remarks>
    /// This class works by using two .ogg files:
    ///     `static.ogg`:  a 3-second file with a loop marker between 0:01 and 0:02
    ///     `loopless.ogg`:a 3-second file without loop markers
    /// Both file aren't music, but a static signal that's set to 0.75, 0.5 and 0.25, each for 1 second.
    /// The right channel is the inverse of the left channel, so the signal is always stereo.
    ///    0.75: |------|      |
    /// L  0.50: |      |------|
    ///    0.25: |      |      |------
    /// ------------------------------------
    ///   -0.25: |      |      |------
    /// R -0.50: |      |------|
    ///   -0.75: |------|      |
    /// ------------------------------------
    ///          |      |      |
    ///        00:00  00:01  00:02
    /// </remarks>
    public class LoopableMusicClipTests
    {
        private const float TOLERANCE = 0.05f;
        private const float FIRST_SECOND = 0.75f;
        private const float SECOND_SECOND = 0.5f;
        private const float THIRD_SECOND = 0.25f;
        private const float SILENCE = 0.0f;
        
        private LoopableMusicClip _clipWithLoopMarkers;
        private LoopableMusicClip _clipWithoutLoopMarkers;
        private int _second;

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            {
                _clipWithLoopMarkers = new LoopableMusicClip();
                var initialize = _clipWithLoopMarkers.Initialize("Tests/has_loopmarkers.ogg");
                yield return initialize;
                _second = _clipWithLoopMarkers.TimeToSamples(1);
            }

            {
                _clipWithoutLoopMarkers = new LoopableMusicClip();
                var initialize = _clipWithoutLoopMarkers.Initialize("Tests/no_loopmarkers.ogg");
                yield return initialize;
            }
        } 
        
        [TearDown]
        public void UnityTearDown()
        {
            _clipWithLoopMarkers.SetCurrentPlaybackHead(0);
        }
        
        [Test]
        public void EnsureMP3Fails()
        {
            Assert.Throws<NotSupportedException>(() => {
                _clipWithLoopMarkers.Initialize("Tests/static.mp3").MoveNext();
            });
        }
        
        [Test]
        public void Handle5SecondsWithLoop()
        {
            var data = new float[_clipWithLoopMarkers.TimeToSamples(5)];
            _clipWithLoopMarkers.GenerateStream(data);            

            for (var i = 0; i < data.Length; i++)
            {
                if (i < _second)
                {
                    Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(FIRST_SECOND).Within(TOLERANCE) : Is.EqualTo(-FIRST_SECOND).Within(TOLERANCE));
                    continue;
                }

                Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(SECOND_SECOND).Within(TOLERANCE) : Is.EqualTo(-SECOND_SECOND).Within(TOLERANCE));
            }
        }
        
        [Test]
        public void HandleIntroOnly()
        {
            var data = new float[_clipWithLoopMarkers.TimeToSamples(1)];
            _clipWithLoopMarkers.GenerateStream(data);            

            for (var i = 0; i < data.Length; i++)
            {
                Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(FIRST_SECOND).Within(TOLERANCE) : Is.EqualTo(-FIRST_SECOND).Within(TOLERANCE));
            }
        }
        
        [Test]
        public void Handle5SecondsWithLoopAndOutro()
        {
            _clipWithLoopMarkers.ContinueLooping = false;

            var data = new float[_clipWithLoopMarkers.TimeToSamples(5)];
            _clipWithLoopMarkers.GenerateStream(data);

            for (var i = 0; i < data.Length; i++)
            {
                if (i < _second)
                {
                    Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(FIRST_SECOND).Within(TOLERANCE) : Is.EqualTo(-FIRST_SECOND).Within(TOLERANCE));
                    continue;
                }
                
                if (i < _second * 2)
                {
                    Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(SECOND_SECOND).Within(TOLERANCE) : Is.EqualTo(-SECOND_SECOND).Within(TOLERANCE));
                    continue;
                }
                
                if (i < _second * 3)
                {
                    Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(THIRD_SECOND).Within(TOLERANCE) : Is.EqualTo(-THIRD_SECOND).Within(TOLERANCE));
                    continue;
                }
                
                Assert.That(data[i], Is.EqualTo(SILENCE).Within(TOLERANCE));
            }
        }

        [Test]
        public void TimeToSamplesAndBackwards()
        {
            const int SECONDS = 5;
            const int SAMPLES = 44100 * 2 * SECONDS;
            var time = _clipWithLoopMarkers.SamplesToTime(SAMPLES);
            Assert.That(time, Is.EqualTo(SECONDS));
            
            var calculatedSamples = _clipWithLoopMarkers.TimeToSamples(time);
            Assert.That(calculatedSamples, Is.EqualTo(SAMPLES));
        }
        
        [Test]
        public void GettingClipPreInitializationFails()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var newLoop = new LoopableMusicClip();
                _ = newLoop.Clip;
            });
        }
        
        [Test]
        public void GettingClipThriceWorks()
        {
            var clip = _clipWithLoopMarkers.Clip;
            Assert.That(clip, Is.Not.Null);
            clip = _clipWithLoopMarkers.Clip;
            Assert.That(clip, Is.Not.Null);
            clip = _clipWithLoopMarkers.Clip;
            Assert.That(clip, Is.Not.Null);
        }
        
        [Test]
        public void FileWithoutLoopMarkersLoopsFullFile()
        {
            // fetch first loop of (intro, loop, outro) and part of second loop of (intro and loop)
            var dataWhileLooping = new float[_clipWithoutLoopMarkers.TimeToSamples(5)];
            _clipWithoutLoopMarkers.GenerateStream(dataWhileLooping);
            
            for (var i = 0; i < dataWhileLooping.Length; i++)
            {
                var currentSecond = i / _clipWithoutLoopMarkers.TimeToSamples(1);
                switch (currentSecond)
                {
                    case 0:
                        Assert.That(dataWhileLooping[i], i % 2 != 1 ? Is.EqualTo(FIRST_SECOND).Within(TOLERANCE) : Is.EqualTo(-FIRST_SECOND).Within(TOLERANCE));
                        continue;
                    case 1:
                        Assert.That(dataWhileLooping[i], i % 2 != 1 ? Is.EqualTo(SECOND_SECOND).Within(TOLERANCE) : Is.EqualTo(-SECOND_SECOND).Within(TOLERANCE));
                        continue;
                    case 2:
                        Assert.That(dataWhileLooping[i], i % 2 != 1 ? Is.EqualTo(THIRD_SECOND).Within(TOLERANCE) : Is.EqualTo(-THIRD_SECOND).Within(TOLERANCE));
                        continue;
                    case 3:
                        Assert.That(dataWhileLooping[i], i % 2 != 1 ? Is.EqualTo(FIRST_SECOND).Within(TOLERANCE) : Is.EqualTo(-FIRST_SECOND).Within(TOLERANCE));
                        continue;
                    case 4:
                        Assert.That(dataWhileLooping[i], i % 2 != 1 ? Is.EqualTo(SECOND_SECOND).Within(TOLERANCE) : Is.EqualTo(-SECOND_SECOND).Within(TOLERANCE));
                        break;
                }
            }
        }
        
        [Test]
        public void FileWithoutLoopMarkersStopsLoopingAfterDisablingLoop()
        {
            // fetch intro, loop, outro, intro and loop
            var dataWhileLooping = new float[_clipWithoutLoopMarkers.TimeToSamples(5)];
            _clipWithoutLoopMarkers.GenerateStream(dataWhileLooping);
            
            // disable looping
            _clipWithoutLoopMarkers.ContinueLooping = false;
            
            // fetch outro, intro, loop, outro (only first outro should be heard)
            var dataAfterLooping = new float[_clipWithoutLoopMarkers.TimeToSamples(4)];
            _clipWithoutLoopMarkers.GenerateStream(dataAfterLooping);
            
            for (var i = 0; i < dataAfterLooping.Length; i++)
            {
                var currentSecond = i / _clipWithoutLoopMarkers.TimeToSamples(1);
                if (currentSecond == 0)
                {
                    Assert.That(dataAfterLooping[i], i % 2 != 1 ? Is.EqualTo(THIRD_SECOND).Within(TOLERANCE) : Is.EqualTo(-THIRD_SECOND).Within(TOLERANCE));
                    continue;
                }

                Assert.That(dataAfterLooping[i], i % 2 != 1 ? Is.EqualTo(SILENCE).Within(TOLERANCE) : Is.EqualTo(-SILENCE).Within(TOLERANCE));
            }
        }
    }
}