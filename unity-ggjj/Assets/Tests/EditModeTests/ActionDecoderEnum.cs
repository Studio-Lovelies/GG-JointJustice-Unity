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
        const string ACTOR_DATA_NAME = "NewActorData";
        ActionDecoder decoder = new ActionDecoder();
        Mock<ISceneController> sceneControllerMock = new Moq.Mock<ISceneController>();
        var narrativePlayerMock = new Mock<INarrativeScriptPlayer>();
        var actorData = ScriptableObject.CreateInstance<ActorData>();
        actorData.name = ACTOR_DATA_NAME;

        narrativePlayerMock.Setup(mock => mock.ActiveNarrativeScript.ObjectStorage.GetObject<ICourtRecordObject>(ACTOR_DATA_NAME)).Returns(actorData);
        sceneControllerMock.Setup(controller => controller.ShowItem(actorData, ItemDisplayPosition.Left));

        decoder.SceneController = sceneControllerMock.Object;
        decoder.NarrativeScriptPlayer = narrativePlayerMock.Object;

        var lineToParse = $" &SHOW_ITEM:{ACTOR_DATA_NAME},Left \n\n\n";
        var logMessage = "Attempting to parse:\n" + lineToParse;
        Debug.Log(logMessage);
        Assert.DoesNotThrow(() => { decoder.InvokeMatchingMethod(lineToParse); });
        LogAssert.Expect(LogType.Log, logMessage);
        LogAssert.NoUnexpectedReceived();

        sceneControllerMock.Verify(controller => controller.ShowItem(actorData, ItemDisplayPosition.Left), Times.Once);
    }

    [Test]
    public void ParseActionLineWithInvalidEnumValue()
    {
        ActionDecoder decoder = new ActionDecoder();
        Mock<ISceneController> sceneControllerMock = new Moq.Mock<ISceneController>();
        sceneControllerMock.Setup(controller => controller.ShowItem(ScriptableObject.CreateInstance<ActorData>(), ItemDisplayPosition.Left));
        decoder.SceneController = sceneControllerMock.Object;

        const string lineToParse = " &SHOW_ITEM:a,Lleft \n\n\n";
        const string logMessage = "Attempting to parse:\n" + lineToParse;
        Debug.Log(logMessage);
        Assert.Throws<ScriptParsingException>(() => { decoder.InvokeMatchingMethod(lineToParse); }, "'Lleft' is incorrect as parameter #2 (itemPos) for action 'SHOW_ITEM': Cannot convert 'Lleft' into an ItemDisplayPosition (valid values include: 'Left, Right, Middle')");

        LogAssert.Expect(LogType.Log, logMessage);
        LogAssert.NoUnexpectedReceived();

        sceneControllerMock.Verify(controller => controller.ShowItem(ScriptableObject.CreateInstance<ActorData>(), ItemDisplayPosition.Left), Times.Never);
    }

}