using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceController : MonoBehaviour, IEvidenceController
{
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    // Start is called before the first frame update
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Evidence Controller doesn't have an action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.SetEvidenceController(this);
        }

    }

    public void AddEvidence(string evidence)
    {
        Debug.LogWarning("AddEvidence not implemented");
    }

    public void RemoveEvidence(string evidence)
    {
        Debug.LogWarning("RemoveEvidence not implemented");
    }

    public void AddToCourtRecord(string actor)
    {
        Debug.LogWarning("AddToCourtRecord not implemented");
    }
}
