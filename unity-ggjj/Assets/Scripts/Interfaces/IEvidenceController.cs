public interface IEvidenceController
{
    void AddEvidence(string evidenceName);
    void RemoveEvidence(string evidenceName);
    void AddToCourtRecord(string actorName);
    void RequirePresentEvidence();
    void SubstituteEvidence(string initialEvidenceName, string substituteEvidenceName);
    void OnPresentEvidence(ICourtRecordObject evidence);
}
