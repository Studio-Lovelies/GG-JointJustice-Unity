using NUnit.Framework;
using Moq;
using TextDecoder.Parser;
using UnityEngine;
using UnityEngine.TestTools;

public class ActionDecoderEnumTests
{
    /// <summary>
    /// Helper method to create a fully mocked ActionDecoder
    /// </summary>
    /// <returns>A fully mocked ActionDecoder</returns>
    private static ActionDecoder CreateMockedActionDecoder()
    {
        return new ActionDecoder()
        {
            ActorController = new Moq.Mock<IActorController>().Object,
            AppearingDialogueController = new Moq.Mock<IAppearingDialogueController>().Object,
            AudioController = new Moq.Mock<IAudioController>().Object,
            EvidenceController = new Moq.Mock<IEvidenceController>().Object,
            SceneController = new Moq.Mock<ISceneController>().Object,
        };
    }

    [Test]
    public void ParseActionLineWithValidEnumValue()
    {
        ActionDecoder decoder = CreateMockedActionDecoder();
        Mock<ISceneController> sceneControllerMock = new Moq.Mock<ISceneController>();
        sceneControllerMock.Setup(controller => controller.ShowItem("A", ItemDisplayPosition.Left));
        decoder.SceneController = sceneControllerMock.Object;

        const string lineToParse = " &SHOW_ITEM:A,Left \n\n\n";
        const string logMessage = "Attempting to parse:\n" + lineToParse;
        Debug.Log(logMessage);
        Assert.DoesNotThrow(() => { decoder.OnNewActionLine(lineToParse); });

        LogAssert.Expect(LogType.Log, logMessage);
        LogAssert.NoUnexpectedReceived();

        sceneControllerMock.Verify(controller => controller.ShowItem("A", ItemDisplayPosition.Left), Times.Once);
    }

    [Test]
    public void ParseActionLineWithInvalidEnumValue()
    {
        ActionDecoder decoder = CreateMockedActionDecoder();
        Mock<ISceneController> sceneControllerMock = new Moq.Mock<ISceneController>();
        sceneControllerMock.Setup(controller => controller.ShowItem("a", ItemDisplayPosition.Left));
        decoder.SceneController = sceneControllerMock.Object;

        const string lineToParse = " &SHOW_ITEM:a,Lleft \n\n\n";
        const string logMessage = "Attempting to parse:\n" + lineToParse;
        Debug.Log(logMessage);
        Assert.Throws<ScriptParsingException>(() => { decoder.OnNewActionLine(lineToParse); }, "'Lleft' is incorrect as parameter #2 (itemPos) for action 'SHOW_ITEM': Cannot convert 'Lleft' into an ItemDisplayPosition (valid values include: 'Left, Right, Middle')");

        LogAssert.Expect(LogType.Log, logMessage);
        LogAssert.NoUnexpectedReceived();

        sceneControllerMock.Verify(controller => controller.ShowItem("a", ItemDisplayPosition.Left), Times.Never);
    }

}