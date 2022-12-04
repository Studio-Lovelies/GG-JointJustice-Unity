using Moq;
using NUnit.Framework;

namespace Tests.EditModeTests.Suites
{
    public class NarrativeScriptStorageTests
    {
        private const string TEST_SCRIPT_NAME = "TMPHGameOver";
        
        private Mock<INarrativeGameState> _narrativeGameStateMock;
        private NarrativeScriptStorage _narrativeScriptStorage;

        [SetUp]
        public void SetUp()
        {
            _narrativeGameStateMock = new Mock<INarrativeGameState>();
            _narrativeGameStateMock.Setup(mock => mock.BGSceneList.InstantiateBGScenes(It.IsAny<INarrativeScript>()));
            _narrativeScriptStorage = new NarrativeScriptStorage(_narrativeGameStateMock.Object);
        }

        [Test]
        public void GameOverScriptCanBeSet()
        {
            Assert.IsNull(_narrativeScriptStorage.GameOverScript);
            _narrativeScriptStorage.SetGameOverScript(TEST_SCRIPT_NAME);
            Assert.AreEqual(TEST_SCRIPT_NAME, _narrativeScriptStorage.GameOverScript.Name);
        }

        [Test]
        public void FailureScriptsCanBeAdded()
        {
            Assert.AreEqual(0, _narrativeScriptStorage.FailureScripts.Count);
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(i, _narrativeScriptStorage.FailureScripts.Count);
                _narrativeScriptStorage.AddFailureScript(TEST_SCRIPT_NAME);
            }
            Assert.AreEqual(5, _narrativeScriptStorage.FailureScripts.Count);
        }

        [Test]
        public void FailureScriptsCanBeRetrieved()
        {
            FailureScriptsCanBeAdded();
            Assert.IsNotNull(_narrativeScriptStorage.GetRandomFailureScript());
        }
    }
}