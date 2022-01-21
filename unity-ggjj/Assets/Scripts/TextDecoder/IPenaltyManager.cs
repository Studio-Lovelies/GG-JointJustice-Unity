public interface IPenaltyManager
{
    void OnCrossExaminationStart();
    void OnCrossExaminationEnd();
    void Decrement();
}