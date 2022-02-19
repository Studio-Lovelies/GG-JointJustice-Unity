using System.Collections.Generic;
using System.Linq;
using Ink;
using Ink.Runtime;
using Moq;
using NUnit.Framework;
using UnityEngine;

public class StoryPlayerTests
{
    private NarrativeScriptPlayer _narrativeScriptPlayer;

    private readonly Mock<INarrativeScriptPlaylist> _narrativeScriptPlaylist = new Mock<INarrativeScriptPlaylist>();
    private readonly Mock<IAppearingDialogueController> _appearingDialogueController = new Mock<IAppearingDialogueController>();
    private readonly Mock<IActionDecoder> _actionDecoder = new Mock<IActionDecoder>();
    private readonly Mock<IChoiceMenu> _choiceMenu = new Mock<IChoiceMenu>();

    [SetUp]
    public void SetUp()
    {
        _narrativeScriptPlayer = new NarrativeScriptPlayer(_narrativeScriptPlaylist.Object, _appearingDialogueController.Object, _actionDecoder.Object, _choiceMenu.Object);
    }

    [Test]
    public void StoryCanContinueOnSpokenLine()
    {
        const string TEST_LINE = "Test Line\n";
        
        _narrativeScriptPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);
        
        var printingText = "";
        _appearingDialogueController.Setup(mock => mock.PrintText(TEST_LINE)).Callback<string>(line => printingText = line);
        _narrativeScriptPlayer.Continue();
        Assert.AreEqual(TEST_LINE, printingText);
    }

    [Test]
    public void StoryCanContinueOnActionLine()
    {
        const string TEST_LINE = "&ACTION\n";

        _narrativeScriptPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);

        var actionDecoder = new ActionDecoder();
        _actionDecoder.Setup(mock => mock.IsAction(TEST_LINE)).Returns(actionDecoder.IsAction(TEST_LINE));
        _actionDecoder.Setup(mock => mock.InvokeMatchingMethod(TEST_LINE)).Verifiable();
        _narrativeScriptPlayer.Continue();
        _actionDecoder.Verify(mock => mock.InvokeMatchingMethod(It.IsAny<string>()));
    }

    [Test]
    public void StoryCannotContinueWhenPrintingText()
    {
        const string TEST_LINE = "Test Line\n";
        _narrativeScriptPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);

        var printingText = "";
        _appearingDialogueController.Setup(mock => mock.PrintText(TEST_LINE)).Callback<string>(line => printingText = line);
        _appearingDialogueController.Setup(mock => mock.IsPrintingText).Returns(true);
        _narrativeScriptPlayer.Continue();
        Assert.AreEqual(string.Empty, printingText);

        _appearingDialogueController.Setup(mock => mock.IsPrintingText).Returns(false);
        _narrativeScriptPlayer.Continue();
        Assert.AreEqual(TEST_LINE, printingText);
    }

    [Test]
    public void ChoiceMenuIsInitialisedAtChoiceInDialogueMode()
    {
        const string TEST_SCRIPT = "+ [1]\n-> DONE\n+ [2]\n-> DONE";
        _narrativeScriptPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_SCRIPT);

        _choiceMenu.Setup(mock => mock.Initialise(It.IsAny<List<Choice>>())).Verifiable();
        _narrativeScriptPlayer.Continue();
        _choiceMenu.Verify();
    }
    
    [Test]
    public void ChoiceMenuIsNotInitialisedAtChoiceInCrossExaminationMode()
    {
        const string TEST_SCRIPT = "+ [1]\n-> DONE\n+ [2]\n-> DONE";
        _narrativeScriptPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_SCRIPT);

        _narrativeScriptPlayer.GameMode = GameMode.CrossExamination;
        bool callback = false;
        _choiceMenu.Setup(mock => mock.Initialise(It.IsAny<List<Choice>>())).Callback(() => callback = true);
        _narrativeScriptPlayer.Continue();
        Assert.IsFalse(callback);
    }

    [Test]
    public void ChoiceCanBeHandled()
    {
        const string TEST_LINE = "+ [1]\n1\n+ [2]\n2";
        _narrativeScriptPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_LINE);
        _narrativeScriptPlayer.Continue();
        Assert.IsTrue(_narrativeScriptPlayer.ActiveNarrativeScript.Story.currentChoices.Count > 0);
        _narrativeScriptPlayer.HandleChoice(0);
        Assert.IsFalse(_narrativeScriptPlayer.ActiveNarrativeScript.Story.currentChoices.Count > 0);
    }

    [Test]
    public void SubStoryCanBeStarted()
    {
        const string PARENT_STORY = "Parent Story\n";
        const string SUB_STORY = "Sub Story\n";

        var printedString = "";
        _appearingDialogueController.Setup(mock => mock.PrintText(It.IsAny<string>())).Callback<string>(line => printedString = line);
        
        _narrativeScriptPlayer.ActiveNarrativeScript = CreateNarrativeScript(PARENT_STORY);
        _narrativeScriptPlayer.StartSubStory(CreateNarrativeScript(SUB_STORY));
        Assert.AreEqual(SUB_STORY, printedString);
        _narrativeScriptPlayer.Continue();
        Assert.AreEqual(PARENT_STORY, printedString);
    }

    [Test]
    public void EvidenceCanBePresented()
    {
        const string TEST_SCRIPT = "&PRESENT_EVIDENCE\n+ [1]\n-> DONE\n+ [BentCoins]\nCorrect\n";
        _narrativeScriptPlayer.ActiveNarrativeScript = CreateNarrativeScript(TEST_SCRIPT);

        var actionDecoder = new ActionDecoder();
        _actionDecoder.Setup(mock => mock.IsAction(It.IsAny<string>())).Returns<string>(line => actionDecoder.IsAction(line));
        _appearingDialogueController.Setup(mock => mock.PrintText("Correct\n")).Verifiable();

        _narrativeScriptPlayer.Continue();
        var evidence = ScriptableObject.CreateInstance<Evidence>();
        evidence.name = "BentCoins";
        _narrativeScriptPlayer.GameMode = GameMode.CrossExamination;
        _narrativeScriptPlayer.PresentEvidence(evidence);
        _appearingDialogueController.Verify();
    }

    private static NarrativeScript CreateNarrativeScript(string scriptText)
    {
        var parser = new InkParser(scriptText);
        var story = parser.Parse().ExportRuntime().ToJson();
        return new NarrativeScript(new TextAsset(story));
    }
}
