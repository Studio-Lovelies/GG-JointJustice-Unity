public interface IEvidenceController
{
    void AddEvidence(string evidence);
    void RemoveEvidence(string evidence);
    void AddToCourtRecord(string actorName);
    void RequirePresentEvidence();
    void SubstituteEvidenceWithAlt(string evidence);
    void OnPresentEvidence(ICourtRecordObject evidence);
}
