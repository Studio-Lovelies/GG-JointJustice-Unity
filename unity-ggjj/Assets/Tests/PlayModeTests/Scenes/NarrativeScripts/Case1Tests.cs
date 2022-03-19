using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.NarrativeScripts
{
    public class Case1Tests
    {
        private static IEnumerable<TestCaseData> NarrativeScripts => Resources.LoadAll<TextAsset>("InkDialogueScripts/Case1").Select(narrativeScript => new TestCaseData(narrativeScript).SetName(narrativeScript.name).Returns(null));

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
        }
    
        [UnityTest]
        [TestCaseSource(nameof(NarrativeScripts))]
        public IEnumerator NarrativeScriptsRunWithNoErrors(TextAsset narrativeScriptText)
        {
            var narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            var narrativeScript = new NarrativeScript(narrativeScriptText);
            narrativeGameState.NarrativeScriptStorage.NarrativeScript = narrativeScript;
            narrativeGameState.StartGame();
            var storyProgresser = new StoryProgresser();
            
            while (narrativeScript.Story.canContinue)
            {
                yield return storyProgresser.ProgressStory();
            }
        }
    }
}
