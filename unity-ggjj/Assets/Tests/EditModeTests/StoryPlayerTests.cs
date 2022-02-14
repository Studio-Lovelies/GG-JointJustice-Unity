using System.Collections.Generic;
using System.Linq;
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

        var actionDecoder = new ActionDecoder();
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
        _appearingDialogueController.Setup(mock => mock.IsPrintingText).Returns(true);
        _storyPlayer.Continue();
        Assert.AreEqual(string.Empty, printingText);

        _appearingDialogueController.Setup(mock => mock.IsPrintingText).Returns(false);
        _storyPlayer.Continue();
        Assert.AreEqual(TEST_LINE, printingText);
    }

    [Test]
    public void ChoiceMenuIsInitialisedAtChoiceInDialogueMode()
    {
        const string TEST_SCRIPT = "+ [1]\n-> DONE\n+ [2]\n-> DONE";
        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_SCRIPT);

        _choiceMenu.Setup(mock => mock.Initialise(It.IsAny<List<Choice>>())).Verifiable();
        _storyPlayer.Continue();
        _choiceMenu.Verify();
    }
    
    [Test]
    public void ChoiceMenuIsNotInitialisedAtChoiceInCrossExaminationMode()
    {
        const string TEST_SCRIPT = "+ [1]\n-> DONE\n+ [2]\n-> DONE";
        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_SCRIPT);

        _storyPlayer.GameMode = GameMode.CrossExamination;
        bool callback = false;
        _choiceMenu.Setup(mock => mock.Initialise(It.IsAny<List<Choice>>())).Callback(() => callback = true);
        _storyPlayer.Continue();
        Assert.IsFalse(callback);
    }

    [Test]
    public void ChoiceCanBeHandled()
    {
        const string TEST_LINE = "+ [1]\n1\n+ [2]\n2";
        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);
        _storyPlayer.Continue();
        Assert.IsTrue(_storyPlayer.ActiveNarrativeScript.Story.currentChoices.Count > 0);
        _storyPlayer.HandleChoice(0);
        Assert.IsFalse(_storyPlayer.ActiveNarrativeScript.Story.currentChoices.Count > 0);
    }

    [Test]
    public void SubStoryCanBeStarted()
    {
        const string PARENT_STORY = "Parent Story\n";
        const string SUB_STORY = "Sub Story\n";

        var printedString = "";
        _appearingDialogueController.Setup(mock => mock.PrintText(It.IsAny<string>())).Callback<string>(line => printedString = line);
        
        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(PARENT_STORY);
        _storyPlayer.StartSubStory(CreateNarrativeScript(SUB_STORY));
        Assert.AreEqual(SUB_STORY, printedString);
        _storyPlayer.Continue();
        Assert.AreEqual(PARENT_STORY, printedString);
    }

    [Test]
    public void EvidenceCanBePresented()
    {
        const string TEST_SCRIPT = "&PRESENT_EVIDENCE\n+ [1]\n-> DONE\n+ [BentCoins]\nCorrect\n";
        _storyPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_SCRIPT);

        var actionDecoder = new ActionDecoder();
        _actionDecoder.Setup(mock => mock.IsAction(It.IsAny<string>())).Returns<string>(line => actionDecoder.IsAction(line));
        _appearingDialogueController.Setup(mock => mock.PrintText("Correct\n")).Verifiable();

        _storyPlayer.Continue();
        var evidence = ScriptableObject.CreateInstance<Evidence>();
        evidence.name = "BentCoins";
        _storyPlayer.GameMode = GameMode.CrossExamination;
        _storyPlayer.PresentEvidence(evidence);
        _appearingDialogueController.Verify();
    }

    private static NarrativeScript CreateNarrativeScript(string scriptText)
    {
        var parser = new InkParser(scriptText);
        var story = parser.Parse().ExportRuntime().ToJson();
        return new NarrativeScript(new TextAsset(story));
    }
}
