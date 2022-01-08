using System.Collections.Generic;
using Ink;
using Moq;
using NUnit.Framework;


public class ScriptReaderTests
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
                                        "&SCENE:TMPH_Court\n" +
                                        "&SCENE:TMPH_Court\n" +
                                        "&SHOW_ITEM:Bent_Coins,Right\n" +
                                        "-> NamedContainer\n" +
                                        "=== NamedContainer ===\n" +
                                        "&SHOW_ITEM:Bent_Coins,Right\n" +
                                        "&ADD_EVIDENCE:Stolen_Dinos\n" +
                                        "&ADD_EVIDENCE:Stolen_Dinos\n" +
                                        "&ADD_RECORD:Tutorial_Boy\n" +
                                        "&ADD_RECORD:Tutorial_Boy\n" +
                                        "&PLAY_SFX:damage1\n" +
                                        "&PLAY_SFX:damage1\n" +
                                        "&PLAY_SONG:aBoyAndHisTrial\n" +
                                        "&PLAY_SONG:aBoyAndHisTrial\n" +
                                        "-> END";
    
    [Test]
    public void ScriptReaderRunsCorrectNumberOfActions()
    {
        var parser = new InkParser(TestScript);
        var story = parser.Parse().ExportRuntime();

        var methodCalls = new Dictionary<int, int>();
        for (int i = 0; i < 10; i++)
        {
            methodCalls.Add(i, 0);
        }

        var objectLoaderMock = new Mock<IObjectPreloader>();
        
        objectLoaderMock.Setup(mock => mock
            .SetActiveActor("Arin"))
            .Callback(() => methodCalls[0]++);
        
        objectLoaderMock.Setup(mock => mock
                .SetActiveActor("Dan"))
            .Callback(() => methodCalls[1]++);
        
        objectLoaderMock.Setup(mock => mock
                .SetActiveSpeaker("Arin"))
            .Callback(() => methodCalls[2]++);
        
        objectLoaderMock.Setup(mock => mock
                .AssignActorToSlot("Jory",1))
            .Callback(() => methodCalls[3]++);

        objectLoaderMock.Setup(mock => mock
                .SetScene("TMPH_Court"))
            .Callback(() => methodCalls[4]++);
        
        objectLoaderMock.Setup(mock => mock
                .ShowItem("Bent_Coins", ItemDisplayPosition.Right))
            .Callback(() => methodCalls[5]++);
        
        objectLoaderMock.Setup(mock => mock
                .AddEvidence("Stolen_Dinos"))
            .Callback(() => methodCalls[6]++);
        
        objectLoaderMock.Setup(mock => mock
                .AddToCourtRecord("Tutorial_Boy"))
            .Callback(() => methodCalls[7]++);
        
        objectLoaderMock.Setup(mock => mock
                .PlaySfx("damage1"))
            .Callback(() => methodCalls[8]++);
        
        objectLoaderMock.Setup(mock => mock
                .PlaySong("aBoyAndHisTrial"))
            .Callback(() => methodCalls[9]++);

        ScriptReader.ReadScript(story, objectLoaderMock.Object);
        
        foreach (var pair in methodCalls)
        {
            Assert.AreEqual(1, pair.Value);
        }
    }
}