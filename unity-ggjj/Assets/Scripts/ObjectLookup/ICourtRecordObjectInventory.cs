public interface ICourtRecordObjectInventory
{
    public int Count { get; }
    public ICourtRecordObject GetObjectInList(int index);
}