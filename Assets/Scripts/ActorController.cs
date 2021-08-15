using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour, IActorController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    // Start is called before the first frame update
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Actor Controller doesn't have a action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.SetActorController(this);
        }

    }

    public void ShowActor()
    {
        Debug.LogWarning("ShowActor not implemented");
    }

    public void HideActor()
    {
        Debug.LogWarning("HideActor not implemented");
    }

    public void SetActiveActor(string actor)
    {
        Debug.LogWarning("SetActiveActor not implemented");
    }

    public void SetAnimation(string animation)
    {
        Debug.LogWarning("SetAnimation not implemented");
    }
}
