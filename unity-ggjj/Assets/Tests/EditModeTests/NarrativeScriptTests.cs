using System.Collections.Generic;
using System.Linq;
using Ink;
using Moq;
using NUnit.Framework;
using UnityEngine;

public class NarrativeScriptTests
{
    public const string TEST_SCRIPT = "This is a test script" +
                                      "&ACTOR:Arin\n" +
                                      "&ACTOR:Arin\n" +
                                      "&ACTOR:Dan\n" +
                                      "&ACTOR:Dan\n" +
                                      "&SPEAK:Arin\n" +
                                      "&SPEAK:Arin\n" +
                                      "&SET_ACTOR_POSITION:1,Jory\n" +
                                      "&SET_ACTOR_POSITION:1,Jory\n" +
                                      "&SCENE:TMPHCourt\n" +
                                      "&SCENE:TMPHCourt\n" +
                                      "&SHOW_ITEM:BentCoins,Right\n" +
                                      "-> NamedContainer\n" +
                                      "=== NamedContainer ===\n" +
                                      "&SHOW_ITEM:Bent_Coins,Right\n" +
                                      "&ADD_EVIDENCE:StolenDinos\n" +
                                      "&ADD_EVIDENCE:StolenDinos\n" +
                                      "&ADD_RECORD:TutorialBoy\n" +
                                      "&ADD_RECORD:TutorialBoy\n" +
                                      "&PLAY_SFX:Damage1\n" +
                                      "&PLAY_SFX:Damage1\n" +
                                      "&PLAY_SONG:ABoyAndHisTrial\n" +
                                      "&PLAY_SONG:ABoyAndHisTrial\n" +
                                      "-> END";

    [Test]
    public void ReadScriptRunsCorrectNumberOfActions()
    {
        var parser = new InkParser(TEST_SCRIPT);
        var story = parser.Parse().ExportRuntime();

        var uniqueActionLines = TEST_SCRIPT.Split('\n')
            .Distinct()
            .Where(line => line.StartsWith("&"))
            .ToList();
        var methodCalls = new List<string>();

        var objectPreloaderMock = new Mock<IActionDecoder>();
        objectPreloaderMock.Setup(mock => mock.OnNewActionLine(It.IsAny<string>()))
            .Callback<string>(line => methodCalls.Add(line));

        _ = new NarrativeScript(new TextAsset(story.ToJson()), objectPreloaderMock.Object);
        
        foreach (var uniqueActionLine in uniqueActionLines)
        {
            Assert.AreEqual(1, methodCalls.Count(line => line == uniqueActionLine));
        }
    }
}