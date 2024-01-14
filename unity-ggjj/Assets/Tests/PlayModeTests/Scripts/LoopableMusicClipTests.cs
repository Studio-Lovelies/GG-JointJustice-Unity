using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts
{
    public class PlayLoopTests
    {
        private LoopableMusicClip _playLoop;
        private int _second;

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            _playLoop = new LoopableMusicClip();
            var initialize = _playLoop.Initialize("Tests/static.ogg");
            yield return initialize;
            _second = _playLoop.TimeToSamples(1);
        }
        
        [TearDown]
        public void UnityTearDown()
        {
            _playLoop.SetCurrentPlaybackHead(0);
        }
        
        [Test]
        public void EnsureMP3Fails()
        {
            Assert.Throws<NotSupportedException>(() => {
                _playLoop.Initialize("Tests/static.mp3").MoveNext();
            });
        }
        
        [Test]
        public void Handle5SecondsWithLoop()
        {
            var data = new float[_playLoop.TimeToSamples(5)];
            _playLoop.GenerateStream(data);            

            for (var i = 0; i < data.Length; i++)
            {
                if (i < _second)
                {
                    Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(0.75f).Within(0.05f) : Is.EqualTo(-0.75f).Within(0.05f));
                    continue;
                }

                Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(0.5f).Within(0.05f) : Is.EqualTo(-0.5f).Within(0.05f));
            }
        }
        
        [Test]
        public void HandleIntroOnly()
        {
            var data = new float[_playLoop.TimeToSamples(1)];
            _playLoop.GenerateStream(data);            

            for (var i = 0; i < data.Length; i++)
            {
                Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(0.75f).Within(0.05f) : Is.EqualTo(-0.75f).Within(0.05f));
            }
        }
        
        [Test]
        public void Handle5SecondsWithLoopAndOutro()
        {
            _playLoop.ContinueLooping = false;

            var data = new float[_playLoop.TimeToSamples(5)];
            _playLoop.GenerateStream(data);

            for (var i = 0; i < data.Length; i++)
            {
                if (i < _second)
                {
                    Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(0.75f).Within(0.05f) : Is.EqualTo(-0.75f).Within(0.05f));
                    continue;
                }
                
                if (i < _second * 2)
                {
                    Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(0.5f).Within(0.05f) : Is.EqualTo(-0.5f).Within(0.05f));
                    continue;
                }
                
                if (i < _second * 3)
                {
                    Assert.That(data[i], i % 2 != 1 ? Is.EqualTo(0.25f).Within(0.05f) : Is.EqualTo(-0.25f).Within(0.05f));
                    continue;
                }
                
                Assert.That(data[i], Is.EqualTo(0f).Within(0.05f));
            }
        }

        [Test]
        public void TimeToSamplesAndBackwards()
        {
            const int SECONDS = 5;
            const int SAMPLES = 44100 * 2 * SECONDS;
            var time = _playLoop.SamplesToTime(SAMPLES);
            Assert.That(time, Is.EqualTo(SECONDS));
            
            var calculatedSamples = _playLoop.TimeToSamples(time);
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
            var clip = _playLoop.Clip;
            Assert.That(clip, Is.Not.Null);
            clip = _playLoop.Clip;
            Assert.That(clip, Is.Not.Null);
            clip = _playLoop.Clip;
            Assert.That(clip, Is.Not.Null);
        }
    }
}