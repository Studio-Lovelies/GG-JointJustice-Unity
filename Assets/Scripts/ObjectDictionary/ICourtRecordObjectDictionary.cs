public interface ICourtRecordObjectDictionary
{
    public int Count { get; }
    public ICourtRecordObject GetObjectAtIndex(int index);
}