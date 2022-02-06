using Ink;
using Moq;
using NUnit.Framework;
using UnityEngine;

public class StoryPlayerTests
{
    private StoryPlayer _storyPlayer;

    private readonly Mock<INarrativeScriptPlaylist> _narrativeScriptPlaylist = new Mock<INarrativeScriptPlaylist>();
    private readonly Mock<IAppearingDialogueController> _appearingDialogueController = new Mock<IAppearingDialogueController>();
    private readonly Mock<IActionDecoder> _actionDecoder = new Mock<IActionDecoder>();
    private readonly Mock<IChoiceMenu> _choiceMenu = new Mock<IChoiceMenu>();

    [SetUp]
    public void SetUp()
    {
        _storyPlayer = new StoryPlayer(_narrativeScriptPlaylist.Object, _appearingDialogueController.Object, _actionDecoder.Object, _choiceMenu.Object);
    }

    [Test]
    public void StoryCanContinue()
    {
        const string TEST_LINE = "Test Line\n";
        
        var parser = new InkParser(TEST_LINE);
        var story = parser.Parse().ExportRuntime().ToJson();

        var narrativeScript = new NarrativeScript(new TextAsset(story));
        _storyPlayer.ActiveNarrativeScript = narrativeScript;

        var printingText = "";
        _appearingDialogueController.Setup(mock => mock.PrintText(It.IsAny<string>())).Callback<string>(line => printingText = line);
        _storyPlayer.Continue();
        Assert.AreEqual(TEST_LINE, printingText);
    }
}
