using System.Collections.Generic;
using Ink;
using Ink.Runtime;
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
    public void StoryCanContinueOnSpokenLine()
    {
        const string TEST_LINE = "Test Line\n";
        
        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);
        
        var printingText = "";
        _appearingDialogueController.Setup(mock => mock.PrintText(TEST_LINE)).Callback<string>(line => printingText = line);
        _storyPlayer.Continue();
        Assert.AreEqual(TEST_LINE, printingText);
    }

    [Test]
    public void StoryCanContinueOnActionLine()
    {
        const string TEST_LINE = "&ACTION\n";

        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);

        ActionDecoder actionDecoder = new ActionDecoder();
        _actionDecoder.Setup(mock => mock.IsAction(TEST_LINE)).Returns(actionDecoder.IsAction(TEST_LINE));
        _actionDecoder.Setup(mock => mock.OnNewActionLine(TEST_LINE)).Verifiable();
        _storyPlayer.Continue();
        _actionDecoder.Verify(mock => mock.OnNewActionLine(It.IsAny<string>()));
    }

    [Test]
    public void StoryCannotContinueWhenPrintingText()
    {
        const string TEST_LINE = "Test Line\n";
        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);

        var printingText = "";
        _appearingDialogueController.Setup(mock => mock.PrintText(TEST_LINE)).Callback<string>(line => printingText = line);
        _appearingDialogueController.Setup(mock => mock.PrintingText).Returns(true);
        _storyPlayer.Continue();
        Assert.AreEqual(string.Empty, printingText);

        _appearingDialogueController.Setup(mock => mock.PrintingText).Returns(false);
        _storyPlayer.Continue();
        Assert.AreEqual(TEST_LINE, printingText);
    }

    [Test]
    public void ChoiceMenuInitialisedAtChoiceInDialogueMode()
    {
        const string TEST_LINE = "+ [1]\n-> DONE\n+ [2]\n-> DONE";
        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);

        _choiceMenu.Setup(mock => mock.Initialise(It.IsAny<List<Choice>>())).Verifiable();
        _storyPlayer.Continue();
        _choiceMenu.Verify();
    }

    private NarrativeScript CreateNarrativeScript(string scriptText)
    {
        var parser = new InkParser(scriptText);
        var story = parser.Parse().ExportRuntime().ToJson();
        return new NarrativeScript(new TextAsset(story));
    }
}
