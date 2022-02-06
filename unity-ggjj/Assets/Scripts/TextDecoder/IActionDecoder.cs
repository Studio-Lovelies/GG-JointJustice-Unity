public interface IActionDecoder
{
    NarrativeScriptPlayer NarrativeScriptPlayer { get; set; }
    
    void OnNewActionLine(string actionLine);
    bool IsAction(string nextLine);
}