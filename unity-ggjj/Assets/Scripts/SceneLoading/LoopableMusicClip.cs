using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles playing back .ogg files with loop markers
/// </summary>
/// <remarks>
/// Music with loop markers consist of up to 3 sections:
///   1. Intro
///   2. Loop
///   3. Outro (optional)
///
/// Inside the .ogg file, the loop markers are defined as tags with the names LOOP_START and LOOP_END in the format HH:MM:SS.mmm.
///
/// If ContinueLooping is true, a track will start with the intro and continue looping the loop section. If ContinueLooping is set to false during playback, it will play finish the current loop and then play the outro.
/// If ContinueLooping is false, a track will start with the intro, loop the loop section once, and then play the outro.
/// If no outro exists, the track will simply end after the last loop.
/// 
/// Usage:
///  1. Instantiate an instance of this class
///  2. Call `.Initialize(pathToOGGWithinMusicDirectory)` to load an .ogg file with loop markers.
///  3. Assign the `.Clip` property to an `AudioSource.clip` of your choice.
///  4. Call `AudioSource.Play()` to start playback. By default, the track will loop indefinitely.
///  5. Change the `.ContinueLooping` property to false to finish the current loop and play the outro, if it exists.
/// </remarks>
public class LoopableMusicClip
{
    private readonly string _clipName;
    private float[] _rawData;
    private int _preLoopSampleLength;
    private int _loopSampleLength;
    private int _fullTrackSampleLength;
    private int _sampleRate;
    private int _channelCount;

    public int TimeToSamples(double time)
    {
        return (int) (time * _sampleRate * _channelCount);
    }
    
    public double SamplesToTime(int samples)
    {
        return (double) samples / _sampleRate / _channelCount;
    }

    public bool ContinueLooping { get; set; } = true;

    private AudioClip _clip;
    public AudioClip Clip
    {
        get
        {
            if (_rawData == null || _rawData.Length == 0)
            {
                throw new InvalidOperationException($"Clip has not been initialized yet; call {nameof(Initialize)}(pathToOGGWithinMusicDirectory) first");
            }
            if (_clip != null)
            {
                return _clip;
            }

            _clip = AudioClip.Create("preLoop", _preLoopSampleLength + _loopSampleLength, _channelCount, _sampleRate, true, GenerateStream, SetCurrentPlaybackHead);
            return _clip;
        }
    }

    public IEnumerator Initialize(string pathToOGGWithinMusicDirectory)
    {
        if (!pathToOGGWithinMusicDirectory.EndsWith(".ogg"))
        {
            throw new NotSupportedException("Only ogg files are supported");
        }
        
        using var www = UnityWebRequestMultimedia.GetAudioClip(Application.streamingAssetsPath + "/Music/" + pathToOGGWithinMusicDirectory, AudioType.OGGVORBIS);
        yield return www.SendWebRequest();

        using var vorbis = new NVorbis.VorbisReader(new MemoryStream(www.downloadHandler.data));
        
        _rawData = new float[vorbis.TotalSamples * vorbis.Channels];
        vorbis.ReadSamples(_rawData, 0, (int) vorbis.TotalSamples * vorbis.Channels);
        var loopStart = vorbis.Tags.GetTagSingle("LOOP_START");
        var loopEnd = vorbis.Tags.GetTagSingle("LOOP_END");
        Debug.Log("loopStart: " + loopStart);
        Debug.Log("loopEnd: " + loopEnd);
        var loopStartSeconds = TimeSpan.Parse(loopStart).TotalSeconds;
        var loopEndSeconds = TimeSpan.Parse(loopEnd).TotalSeconds;
        Debug.Log("loopStartSeconds: " + loopStartSeconds);
        Debug.Log("loopEndSeconds: " + loopEndSeconds);
        Debug.Log("loopSeconds: " + (loopEndSeconds - loopStartSeconds));
        _sampleRate = vorbis.SampleRate;
        _channelCount = vorbis.Channels;
        
        _preLoopSampleLength = TimeToSamples(loopStartSeconds);
        _loopSampleLength = TimeToSamples(loopEndSeconds - loopStartSeconds);
        _fullTrackSampleLength = TimeToSamples(vorbis.TotalTime.TotalSeconds);

        Debug.Log("preLoopSampleLength: " + _preLoopSampleLength);
        Debug.Log("loopSampleLength: " + _loopSampleLength);
        Debug.Log("totalSamples: " + vorbis.TotalSamples);
    }

    public void SetCurrentPlaybackHead(int position)
    {
        _currentPlaybackHead = position;
    }

    private int _currentPlaybackHead;

    public void GenerateStream(float[] data)
    {
        int dataIndex = 0;

        while (dataIndex < data.Length)
        {
            if (_currentPlaybackHead < _preLoopSampleLength)
            {
                Debug.Log("1. Pre loop");
                var availableSampleLength = Math.Min(data.Length - dataIndex, _preLoopSampleLength - _currentPlaybackHead);
                Array.Copy(_rawData, _currentPlaybackHead, data, dataIndex, availableSampleLength);
                _currentPlaybackHead += availableSampleLength;
                dataIndex += availableSampleLength;
            }

            if (dataIndex >= data.Length)
            {
                break;
            }

            if (_currentPlaybackHead >= _preLoopSampleLength && _currentPlaybackHead < (_loopSampleLength + _preLoopSampleLength))
            {
                Debug.Log("5. Loop");
                var availableSampleLength = Math.Min(data.Length - dataIndex, _loopSampleLength + _preLoopSampleLength - _currentPlaybackHead);
                Array.Copy(_rawData, _currentPlaybackHead, data, dataIndex, availableSampleLength);
                _currentPlaybackHead += availableSampleLength;
                dataIndex += availableSampleLength;
            }

            if (dataIndex >= data.Length)
            {
                break;
            }

            if (_currentPlaybackHead >= (_loopSampleLength + _preLoopSampleLength))
            {
                if (!ContinueLooping)
                {
                    Debug.Log("7. End of loop");
                    var availableSamplesOfEnd = _fullTrackSampleLength - _currentPlaybackHead;
                    Array.Copy(_rawData, _currentPlaybackHead, data, dataIndex, availableSamplesOfEnd);
                    dataIndex += availableSamplesOfEnd;
                    _currentPlaybackHead = _fullTrackSampleLength;
                    break;
                }
                Debug.Log("3. Loop wrap around");
                _currentPlaybackHead = _preLoopSampleLength;
            }
        }

        if (dataIndex < data.Length)
        {
            Debug.Log("Clearing remaining data");
            Array.Clear(data, dataIndex, data.Length - dataIndex);
        }
    }
}