using System.Collections.Generic;

public interface IEvidenceController
{
    List<ActorData> CurrentProfiles { get; }
    List<EvidenceData> CurrentEvidence { get; }

    void AddEvidence(EvidenceData evidenceData);
    void RemoveEvidence(EvidenceData evidenceData);
    void AddRecord(ActorData actorData);
    void RequirePresentEvidence();
    void SubstituteEvidence(EvidenceData initialEvidenceData, EvidenceData substituteEvidenceData);
}
