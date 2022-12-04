using Moq;
using NUnit.Framework;
using TextDecoder.Parser;
using UnityEngine;
using UnityEngine.TestTools;


namespace Tests.EditModeTests.Suites.TextDecoderTests.ActionDecoderTests
{
    public class EnumTests
    {
        [Test]
        public void ParseActionLineWithValidEnumValue()
        {
            const string ACTOR_DATA_NAME = "NewActorData";
            var actorData = ScriptableObject.CreateInstance<ActorData>();
            actorData.name = ACTOR_DATA_NAME;
       
            var narrativeGameStateMock = new Mock<INarrativeGameState>();
            narrativeGameStateMock.Setup(mock => mock.ObjectStorage.GetObject<ICourtRecordObject>(ACTOR_DATA_NAME)).Returns(actorData);
            narrativeGameStateMock.Setup(mock => mock.SceneController.ShowItem(actorData, ItemDisplayPosition.Left));
        
            var decoder = new ActionDecoder
            {
                NarrativeGameState = narrativeGameStateMock.Object
            };

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
            var narrativeGameStateMock = new Mock<INarrativeGameState>();
            narrativeGameStateMock.Setup(mock => mock.SceneController.ShowItem(ScriptableObject.CreateInstance<ActorData>(), ItemDisplayPosition.Left));

            var decoder = new ActionDecoder
            {
                NarrativeGameState = narrativeGameStateMock.Object
            };

            const string LINE_TO_PARSE = " &SHOW_ITEM:a,Lleft \n\n\n";
            const string LOG_MESSAGE = "Attempting to parse:\n" + LINE_TO_PARSE;
            Debug.Log(LOG_MESSAGE);
            Assert.Throws<ScriptParsingException>(() => { decoder.InvokeMatchingMethod(LINE_TO_PARSE); }, "'Lleft' is incorrect as parameter #2 (itemPos) for action 'SHOW_ITEM': Cannot convert 'Lleft' into an ItemDisplayPosition (valid values include: 'Left, Right, Middle')");

            LogAssert.Expect(LogType.Log, LOG_MESSAGE);
            LogAssert.NoUnexpectedReceived();

            narrativeGameStateMock.Verify(controller => controller.SceneController.ShowItem(ScriptableObject.CreateInstance<ActorData>(), ItemDisplayPosition.Left), Times.Never);
        }

        [Test]
        public void ParseActionLineWithEmptyEnumValue()
        {
            var narrativeGameStateMock = new Mock<INarrativeGameState>();
            narrativeGameStateMock.Setup(mock => mock.SceneController.ShowItem(ScriptableObject.CreateInstance<ActorData>(), ItemDisplayPosition.Left));

            var decoder = new ActionDecoder
            {
                NarrativeGameState = narrativeGameStateMock.Object
            };

            const string LINE_TO_PARSE = " &SHOW_ITEM:a,\n\n\n";
            const string LOG_MESSAGE = "Attempting to parse:\n" + LINE_TO_PARSE;
            Debug.Log(LOG_MESSAGE);
            Assert.Throws<ScriptParsingException>(() => { decoder.InvokeMatchingMethod(LINE_TO_PARSE); }, "'' is incorrect as parameter #2 (itemPos) for action 'SHOW_ITEM': Cannot convert '' (empty) into an ItemDisplayPosition (valid values include: 'Left, Right, Middle')");

            LogAssert.Expect(LogType.Log, LOG_MESSAGE);
            LogAssert.NoUnexpectedReceived();

            narrativeGameStateMock.Verify(controller => controller.SceneController.ShowItem(ScriptableObject.CreateInstance<ActorData>(), ItemDisplayPosition.Left), Times.Never);
        }

    }
}