using System.Collections.Generic;

public interface IEvidenceController
{
    List<ActorData> CurrentProfiles { get; }
    List<Evidence> CurrentEvidence { get; }

    void AddEvidence(Evidence evidence);
    void RemoveEvidence(Evidence evidence);
    void AddRecord(ActorData actor);
    void RequirePresentEvidence();
    void SubstituteEvidence(Evidence initialEvidence, Evidence substituteEvidence);
}
