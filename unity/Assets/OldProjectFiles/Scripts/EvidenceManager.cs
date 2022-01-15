using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public static EvidenceManager instance;
    void Awake()
    {
        instance = this;
    }

    public List<Evidence> evidences = new List<Evidence>();
    public List<Evidence> playersCourtRecordEvidence = new List<Evidence>();
    public List<Evidence> playersCourtRecordProfiles = new List<Evidence>();

    public void SetCourtRecord(List<string> evidencesToSet)
    {
        List<Evidence> auxEvidence = new List<Evidence>();
        List<Evidence> auxProfiles = new List<Evidence>();

        foreach (string evidence in evidencesToSet)
        {
            if (!GetEvidence(evidence).isProfile)
            {
                auxEvidence.Add(GetEvidence(evidence));
            }
            else
            {
                auxProfiles.Add(GetEvidence(evidence));
            }
        }

        playersCourtRecordEvidence = auxEvidence;
        playersCourtRecordProfiles = auxProfiles;
    }

    public void AddToCourtRecord(string evidenceToAdd)
    {
        if (!GetEvidence(evidenceToAdd).isProfile)
        {
            playersCourtRecordEvidence.Add(GetEvidence(evidenceToAdd));
        }
        else
        {
            playersCourtRecordProfiles.Add(GetEvidence(evidenceToAdd));
        }
    }

    public Evidence GetEvidence(string evidence)
    {
        return evidences.Find(x => x.name == evidence);
    }
}
