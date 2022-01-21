using System.Collections.Generic;
using Ink;
using Moq;
using NUnit.Framework;
using UnityEngine;

public class NarrativeScriptTests
{
    public readonly string TestScript = "This is a test script" +
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
        var parser = new InkParser(TestScript);
        var story = parser.Parse().ExportRuntime();

        var methodCalls = new Dictionary<int, int>();
        for (int i = 0; i < 10; i++)
        {
            methodCalls.Add(i, 0);
        }

        var objectPreloaderMock = new Mock<IActionDecoder>();
        
        objectPreloaderMock.Setup(mock => mock
            .OnNewActionLine("&ACTOR:Arin"))
            .Callback(() => methodCalls[0]++);
        
        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&ACTOR:Dan"))
            .Callback(() => methodCalls[1]++);
        
        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&SPEAK:Arin"))
            .Callback(() => methodCalls[2]++);
        
        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&SET_ACTOR_POSITION:1,Jory"))
            .Callback(() => methodCalls[3]++);

        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&SCENE:TMPHCourt"))
            .Callback(() => methodCalls[4]++);
        
        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&SHOW_ITEM:BentCoins,Right"))
            .Callback(() => methodCalls[5]++);
        
        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&ADD_EVIDENCE:StolenDinos"))
            .Callback(() => methodCalls[6]++);
        
        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&ADD_RECORD:TutorialBoy"))
            .Callback(() => methodCalls[7]++);
        
        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&PLAY_SFX:Damage1"))
            .Callback(() => methodCalls[8]++);
        
        objectPreloaderMock.Setup(mock => mock
                .OnNewActionLine("&PLAY_SONG:ABoyAndHisTrial"))
            .Callback(() => methodCalls[9]++);
        
        new NarrativeScript(new TextAsset(story.ToJson()), DialogueControllerMode.Dialogue, objectPreloaderMock.Object);

        foreach (var pair in methodCalls)
        {
            Assert.AreEqual(1, pair.Value);
        }
    }
}