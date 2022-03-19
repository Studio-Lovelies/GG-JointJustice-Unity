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
        private static IEnumerable<TestCaseData> NarrativeScripts => Resources.LoadAll<TextAsset>("InkDialogueScripts/Case1").Select(narrativeScript => new TestCaseData(narrativeScript).SetName(narrativeScript.name));

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
        }
    
        [UnityTest]
        [TestCaseSource(nameof(NarrativeScripts))]
        public IEnumerator NarrativeScriptsRunWithNoErrors(TextAsset narrativeScriptText)
        {
            yield return null;
            var narrativeScriptPlayer = Object.FindObjectOfType<NarrativeScriptPlayerComponent>();
            // var narrativeScript = new NarrativeScript(narrativeScriptText);
            // narrativeScriptPlayer.LoadScriptByReference(narrativeScript);
            //
            // var storyProgresser = new StoryProgresser();
            //
            // while (narrativeScript.Story.canContinue)
            // {
            //     yield return storyProgresser.ProgressStory();
            // }
        }
    }
}
