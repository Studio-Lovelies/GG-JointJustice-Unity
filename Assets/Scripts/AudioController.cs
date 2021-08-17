using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour, IAudioController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    // Start is called before the first frame update
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Audio Controller doesn't have an action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.SetAudioController(this);
        }
    }

    public void PlaySong(string song)
    {
        Debug.LogWarning("PlayBGSong not implemented");
    }

    public void PlaySFX(string SFX)
    {
        Debug.LogWarning("PlaySFX not implemented");
    }
}
