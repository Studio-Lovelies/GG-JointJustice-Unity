public interface IActionDecoder
{
    void OnNewActionLine(string actionLine);
    bool IsAction(string nextLine);
}