using NUnit.Framework;
using Moq;
using TextDecoder.Parser;
using UnityEngine;
using UnityEngine.TestTools;

public class ActionDecoderEnumTests
{
    [Test]
    public void ParseActionLineWithValidEnumValue()
    {
        var decoder = new ActionDecoder();
       
        const string ACTOR_DATA_NAME = "NewActorData";
        var actorData = ScriptableObject.CreateInstance<ActorData>();
        actorData.name = ACTOR_DATA_NAME;
       
        var narrativeGameStateMock = new Mock<INarrativeGameState>();
        narrativeGameStateMock.Setup(mock => mock.ObjectStorage.GetObject<ICourtRecordObject>(ACTOR_DATA_NAME)).Returns(actorData);
        narrativeGameStateMock.Setup(mock => mock.SceneController.ShowItem(actorData, ItemDisplayPosition.Left));
        decoder.NarrativeGameState = narrativeGameStateMock.Object;

        var lineToParse = $" &SHOW_ITEM:{ACTOR_DATA_NAME},Left \n\n\n";
        var logMessage = "Attempting to parse:\n" + lineToParse;
        Debug.Log(logMessage);
        Assert.DoesNotThrow(() => { decoder.InvokeMatchingMethod(lineToParse); });
        LogAssert.Expect(LogType.Log, logMessage);
        LogAssert.NoUnexpectedReceived();

        narrativeGameStateMock.Verify(mock => mock.SceneController.ShowItem(actorData, ItemDisplayPosition.Left), Times.Once);
    }

    [Test]
    public void ParseActionLineWithInvalidEnumValue()
    {
        var decoder = new ActionDecoder();
        var narrativeGameStateMock = new Mock<INarrativeGameState>();
        narrativeGameStateMock.Setup(mock => mock.SceneController.ShowItem(ScriptableObject.CreateInstance<ActorData>(), ItemDisplayPosition.Left));
        decoder.NarrativeGameState = narrativeGameStateMock.Object;

        const string lineToParse = " &SHOW_ITEM:a,Lleft \n\n\n";
        const string logMessage = "Attempting to parse:\n" + lineToParse;
        Debug.Log(logMessage);
        Assert.Throws<ScriptParsingException>(() => { decoder.InvokeMatchingMethod(lineToParse); }, "'Lleft' is incorrect as parameter #2 (itemPos) for action 'SHOW_ITEM': Cannot convert 'Lleft' into an ItemDisplayPosition (valid values include: 'Left, Right, Middle')");

        LogAssert.Expect(LogType.Log, logMessage);
        LogAssert.NoUnexpectedReceived();

        narrativeGameStateMock.Verify(controller => controller.SceneController.ShowItem(ScriptableObject.CreateInstance<ActorData>(), ItemDisplayPosition.Left), Times.Never);
    }

}