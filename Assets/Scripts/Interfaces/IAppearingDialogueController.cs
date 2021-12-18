public interface IAppearingDialogueController
{
    float CharactersPerSecond { set; }
    float DefaultPunctuationDelay { set; }
    bool SkippingDisabled { get; set; }
    bool ContinueDialogue { get; set; }
    bool AutoSkip { get; set; }
    bool AppearInstantly { get; set; }
    bool TextBoxHidden { set; }
}