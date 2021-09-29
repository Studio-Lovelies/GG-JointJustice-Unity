public interface ICourtRecordObjectInventory
{
    public int Count { get; }
    public ICourtRecordObject GetObjectAtIndex(int index);
}