using System.Collections.Generic;

public interface IEvidenceController
{
    List<ActorData> CurrentProfiles { get; }
    List<Evidence> CurrentEvidence { get; }

    void AddEvidence(string evidenceName);
    void RemoveEvidence(string evidenceName);
    void AddToCourtRecord(string actorName);
    void RequirePresentEvidence();
    void SubstituteEvidence(string initialEvidenceName, string substituteEvidenceName);
}
