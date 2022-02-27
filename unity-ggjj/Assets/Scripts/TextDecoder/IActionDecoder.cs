public interface IActionDecoder
{
    void InvokeMatchingMethod(string actionLine);
    bool IsAction(string nextLine);
}