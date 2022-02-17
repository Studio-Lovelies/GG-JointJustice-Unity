using System.Collections.Generic;

public interface IEvidenceController
{
    List<ActorData> CurrentProfiles { get; }
    List<Evidence> CurrentEvidence { get; }

    void AddEvidence(Evidence evidence);
    void RemoveEvidence(string evidenceName);
    void AddToCourtRecord(ActorData actor);
    void RequirePresentEvidence();
    void SubstituteEvidence(string initialEvidenceName, Evidence substituteEvidence);
}
