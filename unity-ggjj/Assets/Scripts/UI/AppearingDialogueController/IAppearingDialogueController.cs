public interface IAppearingDialogueController
{
    bool SpeedupText { get; set; }
    float CharacterDelay { get; set; }
    float DefaultPunctuationDelay { set; }
    bool SkippingDisabled { get; set; }
    bool ContinueDialogue { get; set; }
    bool AutoSkip { get; set; }
    bool AppearInstantly { get; set; }
    bool TextBoxHidden { set; }
    bool IsPrintingText { get; }
    
    void PrintText(string isAny);
    void StopPrintingText();
}